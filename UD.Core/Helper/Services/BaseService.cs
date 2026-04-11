namespace UD.Core.Helper.Services
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using System.Linq.Dynamic.Core;
    using System.Linq.Expressions;
    using UD.Core.Extensions;
    using UD.Core.Helper.Configuration;
    using UD.Core.Helper.Paging;
    using UD.Core.Helper.Validation;
    public interface IBaseService<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto> : IBaseReadOnlyService<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto>
    where TContext : DbContext
    where TEntity : class, IBaseEntity
    where TEntityDto : IEntityDto
    where TEntityListDto : IEntityDto
    where TSearchDto : ISearchAndPaginateDto
    {
        Task Delete(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);
        Task DeleteByPredicate(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default);
        Task DeleteRange(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default);
    }
    public abstract class BaseService<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto> : BaseReadOnlyService<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto>, IBaseReadOnlyService<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto>
    where TContext : DbContext
    where TEntity : class, IBaseEntity
    where TEntityDto : IEntityDto
    where TEntityListDto : IEntityDto
    where TSearchDto : ISearchAndPaginateDto
    {
        protected BaseService(TContext Context, IMapper Mapper) : base(Context, Mapper) { }
        public virtual async Task Delete(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            if (entity != null)
            {
                if (this.Context.Entry(entity).State == EntityState.Detached) { this.DbSet.Attach(entity); }
                this.DbSet.Remove(entity);
                if (autoSave) { await this.Context.SaveChangesAsync(cancellationToken); }
            }
        }
        public virtual async Task DeleteByPredicate(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            Guard.ThrowIfNull(predicate, nameof(predicate));
            var entities = await this.DbSet.Where(predicate).ToArrayAsync(cancellationToken);
            await this.DeleteRange(entities, autoSave, cancellationToken);
        }
        public virtual async Task DeleteRange(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            if (!entities.IsNullOrEmptyOrAllNull())
            {
                foreach (var entity in entities) { await this.Delete(entity, false, cancellationToken); }
                if (autoSave) { await this.Context.SaveChangesAsync(cancellationToken); }
            }
        }
    }
}