namespace UD.Core.Base
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;
    /// <summary>
    /// Generic bir repository arayüzü.
    /// </summary>
    /// <typeparam name="T">Repository tarafından yönetilecek entity türü.</typeparam>
    /// <typeparam name="TContext">DbContext türü.</typeparam>
    public interface IGenericRepository<T, TContext> where T : class where TContext : DbContext
    {
        TContext Context { get; }
        DbSet<T> DbSet { get; }
        Task DeleteAsync(T entity, CancellationToken cancellationtoken = default);
        Task DeleteByKeyAsync(CancellationToken cancellationtoken, params object[] keyvalues);
        Task DeleteByPredicateAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationtoken = default);
        Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationtoken = default);
    }
    /// <summary>
    /// Generic bir repository&#39;nin temel implementasyonu.
    /// </summary>
    /// <typeparam name="T">Repository tarafından yönetilecek entity türü.</typeparam>
    /// <typeparam name="TContext">DbContext türü.</typeparam>
    public abstract class BaseGenericRepository<T, TContext> : IGenericRepository<T, TContext> where T : class where TContext : DbContext, new()
    {
        public BaseGenericRepository(TContext context)
        {
            this.Context = context;
        }
        public TContext Context { get; }
        public DbSet<T> DbSet => this.Context.Set<T>();
        public virtual Task DeleteAsync(T entity, CancellationToken cancellationtoken = default) // Not: cancellationtoken kullanılmasa da, override edilen yerlerde gerektiğinde kullanılabilmesi için eklenmesi gerekiyor.
        {
            if (entity != null)
            {
                if (this.Context.Entry(entity).State == EntityState.Detached) { this.DbSet.Attach(entity); }
                this.DbSet.Remove(entity);
            }
            return Task.CompletedTask;
        }
        public async Task DeleteByKeyAsync(CancellationToken cancellationtoken, params object[] keyvalues) => await this.DeleteAsync(await this.DbSet.FindAsync(keyvalues, cancellationtoken), cancellationtoken);
        public async Task DeleteByPredicateAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationtoken = default) => await this.DeleteRangeAsync(await this.DbSet.Where(predicate).ToArrayAsync(cancellationtoken), cancellationtoken);
        public async Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationtoken = default)
        {
            if (entities != null) { foreach (var entity in entities) { await this.DeleteAsync(entity, cancellationtoken); } }
        }
    }
}