namespace UD.Core.Middlewares
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using UD.Core.Attributes;
    using UD.Core.Extensions;
    public sealed class TransactionMiddleware<TContext> where TContext : DbContext
    {
        private readonly RequestDelegate _next;
        public TransactionMiddleware(RequestDelegate next)
        {
            this._next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var endPoint = context.GetEndpoint();
            if (endPoint?.Metadata?.GetMetadata<DisableTransactionAttribute>() != null)
            {
                await this._next(context);
                return;
            }
            var method = context.Request.Method;
            if (!(HttpMethods.IsPost(method) || HttpMethods.IsPut(method) || HttpMethods.IsPatch(method) || HttpMethods.IsDelete(method)))
            {
                await this._next(context);
                return;
            }
            var dbContext = context.RequestServices.GetService<TContext>();
            if (dbContext == null)
            {
                await this._next(context);
                return;
            }
            if (dbContext.Database.CurrentTransaction != null)
            {
                await this._next(context);
                return;
            }
            var strategy = dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async (cancellationToken) =>
            {
                await using var tran = await dbContext.Database.BeginTransactionAsync(cancellationToken);
                try
                {
                    await this._next(context);
                    var status = context.Response.StatusCode;
                    if (status.Between(StatusCodes.Status200OK, StatusCodes.Status400BadRequest - 1) && !context.IsTransactionRollbackRequired()) { await tran.CommitAsync(cancellationToken); } //if (dbContext.ChangeTracker.HasChanges()) { await dbContext.SaveChangesAsync(cancellationToken); }
                    else { await tran.RollbackAsync(cancellationToken); }
                }
                catch
                {
                    try { await tran.RollbackAsync(CancellationToken.None); } catch { }
                    throw;
                }
            }, context.RequestAborted);
        }
    }
}