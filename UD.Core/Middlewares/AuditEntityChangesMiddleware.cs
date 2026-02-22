namespace UD.Core.Middlewares
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Net;
    using UD.Core.Extensions;
    public sealed class AuditEntityChangesMiddleware<TContext> where TContext : DbContext
    {
        private readonly RequestDelegate next;
        private readonly ILogger<AuditEntityChangesMiddleware<TContext>> logger;
        public AuditEntityChangesMiddleware(RequestDelegate next, ILogger<AuditEntityChangesMiddleware<TContext>> logger)
        {
            this.next = next;
            this.logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var dbContext = context.RequestServices.GetService<TContext>();
            if (dbContext == null)
            {
                await this.next(context);
                return;
            }
            await this.next(context);
            if (context.Response.StatusCode > (int)HttpStatusCode.OK && context.Response.StatusCode < (int)HttpStatusCode.BadRequest && this.logger.IsEnabled(LogLevel.Trace) && dbContext.ChangeTracker.HasChanges())
            {
                var changes = new List<object>();
                foreach (var entry in dbContext.ChangeTracker.Entries())
                {
                    if (entry.State.Includes(EntityState.Added, EntityState.Modified, EntityState.Deleted))
                    {
                        changes.Add(new
                        {
                            tablename = entry.Entity.GetType().GetTableName(true),
                            state = entry.State.ToString(),
                            key = entry.CurrentValues.ToObject()?.ToString() ?? "N/A",
                            changes = entry.Properties.Where(x => x.IsModified || entry.State == EntityState.Added).Select(x => new
                            {
                                name = x.Metadata.GetColumnName(),
                                old = x.OriginalValue,
                                @new = x.CurrentValue
                            }).ToArray()
                        });
                    }
                }
                if (changes.Count > 0) { this.logger.LogTrace("Entity Audit - User: {UserId} | Changes: {@Changes}", context.User?.FindFirst("sub")?.Value, changes); }
            }
        }
    }
}