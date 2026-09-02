namespace UD.Core.Helper.Services
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections;
    using UD.Core.Extensions;
    using UD.Core.Helper.Configurations;
    using UD.Core.Helper.Pages;
    public interface IBaseServiceReadOnly<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto> : IBaseServiceInfrastructure<TContext, TEntity>
    where TContext : DbContext
    where TEntity : class, IBaseEntity
    where TEntityDto : class, IEntityDto
    where TEntityListDto : class, IEntityDto
    where TSearchDto : class, ISearchAndPaginateDto
    {
        Task<TEntityDto?> GetByIdAsync(object id, CancellationToken cancellationToken = default);
        Task<TEntityDto?> GetBySearchAsync(TSearchDto searchDto, CancellationToken cancellationToken = default);
        Task<TEntityListDto[]> GetAllAsync(TSearchDto searchDto, CancellationToken cancellationToken = default);
        Task<Paginate<TEntityListDto>> GetAllPaginateAsync(TSearchDto searchDto, bool loadInfo = true, CancellationToken cancellationToken = default);
    }
    public abstract class BaseServiceReadOnly<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto> : BaseServiceInfrastructure<TContext, TEntity>, IBaseServiceReadOnly<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto>
    where TContext : DbContext
    where TEntity : class, IBaseEntity
    where TEntityDto : class, IEntityDto
    where TEntityListDto : class, IEntityDto
    where TSearchDto : class, ISearchAndPaginateDto
    {
        protected readonly IMapper Mapper;
        protected BaseServiceReadOnly(TContext context, IMapper mapper) : base(context)
        {
            this.Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        internal static bool TryGetKeyValues(object id, out object[] keyValues)
        {
            if (id == null) { keyValues = []; }
            else if (id is object[] _array) { keyValues = _array; }
            else if (id.GetType().IsArray) { keyValues = (object[])id; }
            else if (id is IEnumerable _enumerable) { keyValues = _enumerable.Cast<object>().ToArray(); }
            else { keyValues = [id]; }
            return !keyValues.IsNullOrEmptyOrAllNull();
        }
        protected abstract IQueryable<TEntity> ApplyFiltering(IQueryable<TEntity> query, TSearchDto searchDto);
        public virtual async Task<TEntityDto?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            if (TryGetKeyValues(id, out object[] _keyValues))
            {
                var entity = await base.DbSet.FindAsync(_keyValues, cancellationToken);
                if (entity != null) { return this.Mapper.Map<TEntityDto>(entity); }
            }
            return null;
        }
        public virtual Task<TEntityDto?> GetBySearchAsync(TSearchDto searchDto, CancellationToken cancellationToken = default) => this.ApplyFiltering(base.DbSet, searchDto).AsNoTracking().ProjectTo<TEntityDto>(this.Mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);
        public virtual async Task<TEntityListDto[]> GetAllAsync(TSearchDto searchDto, CancellationToken cancellationToken = default) => (await this.GetAllPaginateAsync(searchDto, false, cancellationToken)).Items;
        public virtual Task<Paginate<TEntityListDto>> GetAllPaginateAsync(TSearchDto searchDto, bool loadInfo = true, CancellationToken cancellationToken = default)
        {
            var query = this.ApplyFiltering(base.DbSet, searchDto).AsNoTracking();
            return searchDto.ToPagedListAsync(query.ProjectTo<TEntityListDto>(this.Mapper.ConfigurationProvider), loadInfo, cancellationToken);
        }
    }
}