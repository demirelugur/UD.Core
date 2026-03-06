namespace UD.Core.Middlewares
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using UD.Core.Attributes;
    public sealed class TransactionMiddleware<TContext> where TContext : DbContext
    {
        private readonly RequestDelegate next;
        public TransactionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            var endPoint = httpContext.GetEndpoint();
            if (endPoint?.Metadata?.GetMetadata<DisableTransactionAttribute>() != null)
            {
                await this.next(httpContext);
                return;
            }
            var method = httpContext.Request.Method;
            if (!(HttpMethods.IsPost(method) || HttpMethods.IsPut(method) || HttpMethods.IsPatch(method) || HttpMethods.IsDelete(method)))
            {
                await this.next(httpContext);
                return;
            }
            var dbContext = httpContext.RequestServices.GetService<TContext>();
            if (dbContext == null)
            {
                await this.next(httpContext);
                return;
            }
            var strategy = dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async (cancellationToken) =>
            {
                using (var tran = await dbContext.Database.BeginTransactionAsync(cancellationToken))
                {
                    try
                    {
                        await this.next(httpContext);
                        var status = httpContext.Response.StatusCode;
                        if (status >= StatusCodes.Status200OK && status < StatusCodes.Status400BadRequest)
                        {
                            if (dbContext.ChangeTracker.HasChanges()) { await dbContext.SaveChangesAsync(cancellationToken); }
                            await tran.CommitAsync(cancellationToken);
                        }
                        else { await tran.RollbackAsync(cancellationToken); }
                    }
                    catch
                    {
                        try { await tran.RollbackAsync(CancellationToken.None); } catch { }
                        throw;
                    }
                }
            }, httpContext.RequestAborted);
        }
    }
}