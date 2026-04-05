namespace UD.Core.Helper.Services
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using System.Linq.Dynamic.Core;
    using System.Linq.Expressions;
    using UD.Core.Helper.Configuration;
    using UD.Core.Helper.Paging;
    using UD.Core.Helper.Validation;
    public interface IBaseService<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto> : IBaseInfrastructureService<TContext, TEntity>
    where TContext : DbContext
    where TEntity : class, IBaseEntity
    where TEntityDto : IEntityDto
    where TEntityListDto : IEntityDto
    where TSearchDto : ISearchAndPaginateDto
    {
        Task<TEntityDto?> GetBySearch(TSearchDto searchDto, CancellationToken cancellationToken = default);
        Task<TEntityListDto[]> GetAll(TSearchDto searchDto, CancellationToken cancellationToken = default);
        Task<Paginate<TEntityListDto>> GetAllPaginate(TSearchDto searchDto, bool loadInfo = true, CancellationToken cancellationToken = default);
        Task Delete(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);
        Task DeleteByPredicate(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default);
        Task DeleteRange(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default);
    }
    public abstract class BaseService<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto> : BaseInfrastructureService<TContext, TEntity>, IBaseService<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto>
    where TContext : DbContext
    where TEntity : class, IBaseEntity
    where TEntityDto : IEntityDto
    where TEntityListDto : IEntityDto
    where TSearchDto : ISearchAndPaginateDto
    {
        protected BaseService(TContext Context, IMapper Mapper) : base(Context, Mapper) { }
        protected abstract IQueryable<TEntity> ApplyFiltering(IQueryable<TEntity> query, TSearchDto searchDto);
        public virtual Task<TEntityDto?> GetBySearch(TSearchDto searchDto, CancellationToken cancellationToken = default) => this.ApplyFiltering(this.DbSet, searchDto).AsNoTracking().ProjectTo<TEntityDto>(this.Mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);
        public virtual async Task<TEntityListDto[]> GetAll(TSearchDto searchDto, CancellationToken cancellationToken = default) => (await this.GetAllPaginate(searchDto, false, cancellationToken)).items;
        public virtual Task<Paginate<TEntityListDto>> GetAllPaginate(TSearchDto searchDto, bool loadInfo = true, CancellationToken cancellationToken = default)
        {
            Guard.ThrowIfNull(searchDto, nameof(searchDto));
            return searchDto.ToPagedList(this.ApplyFiltering(this.DbSet, searchDto).AsNoTracking().ProjectTo<TEntityListDto>(this.Mapper.ConfigurationProvider), loadInfo, cancellationToken);
        }
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
            if (entities != null)
            {
                foreach (var entity in entities) { await this.Delete(entity, false, cancellationToken); }
                if (autoSave) { await this.Context.SaveChangesAsync(cancellationToken); }
            }
        }
    }
}