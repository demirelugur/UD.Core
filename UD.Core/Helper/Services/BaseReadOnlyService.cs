namespace UD.Core.Helper.Services
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using UD.Core.Helper.Configuration;
    using UD.Core.Helper.Paging;
    using UD.Core.Helper.Validation;
    public interface IBaseReadOnlyService<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto> : IBaseInfrastructureService<TContext, TEntity>
    where TContext : DbContext
    where TEntity : class, IBaseEntity
    where TEntityDto : IEntityDto
    where TEntityListDto : IEntityDto
    where TSearchDto : ISearchAndPaginateDto
    {
        Task<TEntityDto?> GetBySearch(TSearchDto searchDto, CancellationToken cancellationToken = default);
        Task<TEntityListDto[]> GetAll(TSearchDto searchDto, CancellationToken cancellationToken = default);
        Task<Paginate<TEntityListDto>> GetAllPaginate(TSearchDto searchDto, bool loadInfo = true, CancellationToken cancellationToken = default);
    }
    public abstract class BaseReadOnlyService<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto> : BaseInfrastructureService<TContext, TEntity>, IBaseReadOnlyService<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto>
    where TContext : DbContext
    where TEntity : class, IBaseEntity
    where TEntityDto : IEntityDto
    where TEntityListDto : IEntityDto
    where TSearchDto : ISearchAndPaginateDto
    {
        protected BaseReadOnlyService(TContext Context, IMapper Mapper) : base(Context, Mapper) { }
        protected abstract IQueryable<TEntity> ApplyFiltering(IQueryable<TEntity> query, TSearchDto searchDto);
        public virtual Task<TEntityDto?> GetBySearch(TSearchDto searchDto, CancellationToken cancellationToken = default) => this.ApplyFiltering(this.DbSet, searchDto).AsNoTracking().ProjectTo<TEntityDto>(this.Mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);
        public virtual async Task<TEntityListDto[]> GetAll(TSearchDto searchDto, CancellationToken cancellationToken = default) => (await this.GetAllPaginate(searchDto, false, cancellationToken)).items;
        public virtual Task<Paginate<TEntityListDto>> GetAllPaginate(TSearchDto searchDto, bool loadInfo = true, CancellationToken cancellationToken = default)
        {
            Guard.ThrowIfNull(searchDto, nameof(searchDto));
            return searchDto.ToPagedList(this.ApplyFiltering(this.DbSet, searchDto).AsNoTracking().ProjectTo<TEntityListDto>(this.Mapper.ConfigurationProvider), loadInfo, cancellationToken);
        }
    }
}