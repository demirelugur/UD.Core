namespace UD.Core.Helper.Services
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections;
    using UD.Core.Extensions;
    using UD.Core.Helper.Configuration;
    using UD.Core.Helper.Paging;
    using UD.Core.Helper.Validation;
    public interface IBaseServiceReadOnly<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto> : IBaseServiceInfrastructure<TContext, TEntity>
    where TContext : DbContext
    where TEntity : class, IBaseEntity
    where TEntityDto : class, IEntityDto
    where TEntityListDto : class, IEntityDto
    where TSearchDto : class, ISearchAndPaginateDto
    {
        Task<TEntityDto?> GetById(object id, CancellationToken cancellationToken = default);
        Task<TEntityDto?> GetBySearch(TSearchDto searchDto, CancellationToken cancellationToken = default);
        Task<TEntityListDto[]> GetAll(TSearchDto searchDto, CancellationToken cancellationToken = default);
        Task<Paginate<TEntityListDto>> GetAllPaginate(TSearchDto searchDto, bool loadInfo = true, CancellationToken cancellationToken = default);
    }
    public abstract class BaseServiceReadOnly<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto> : BaseServiceInfrastructure<TContext, TEntity>, IBaseServiceReadOnly<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto>
    where TContext : DbContext
    where TEntity : class, IBaseEntity
    where TEntityDto : class, IEntityDto
    where TEntityListDto : class, IEntityDto
    where TSearchDto : class, ISearchAndPaginateDto
    {
        protected readonly IMapper Mapper;
        protected BaseServiceReadOnly(TContext Context, IMapper Mapper) : base(Context)
        {
            this.Mapper = Mapper ?? throw new ArgumentNullException(nameof(Mapper));
        }
        internal bool TryGetKeyValues(object id, out object[] keyValues)
        {
            if (id == null) { keyValues = []; }
            else if (id is object[] _array) { keyValues = _array; }
            else if (id.GetType().IsArray) { keyValues = (object[])id; }
            else if (id is IEnumerable _enumerable) { keyValues = _enumerable.Cast<object>().ToArray(); }
            else { keyValues = [id]; }
            return !keyValues.IsNullOrEmptyOrAllNull();
        }
        protected abstract IQueryable<TEntity> ApplyFiltering(IQueryable<TEntity> query, TSearchDto searchDto);
        public virtual async Task<TEntityDto?> GetById(object id, CancellationToken cancellationToken = default)
        {
            if (this.TryGetKeyValues(id, out object[] _keyValues))
            {
                var entity = await base.DbSet.FindAsync(_keyValues, cancellationToken);
                if (entity != null) { return this.Mapper.Map<TEntityDto>(entity); }
            }
            return null;
        }
        public virtual Task<TEntityDto?> GetBySearch(TSearchDto searchDto, CancellationToken cancellationToken = default)
        {
            Guard.ThrowIfNull(searchDto, nameof(searchDto));
            return this.ApplyFiltering(base.DbSet, searchDto).AsNoTracking().ProjectTo<TEntityDto>(this.Mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);
        }
        public virtual async Task<TEntityListDto[]> GetAll(TSearchDto searchDto, CancellationToken cancellationToken = default) => (await this.GetAllPaginate(searchDto, false, cancellationToken)).items;
        public virtual Task<Paginate<TEntityListDto>> GetAllPaginate(TSearchDto searchDto, bool loadInfo = true, CancellationToken cancellationToken = default)
        {
            Guard.ThrowIfNull(searchDto, nameof(searchDto));
            var query = this.ApplyFiltering(base.DbSet, searchDto).AsNoTracking();
            return searchDto.ToPagedList(query.ProjectTo<TEntityListDto>(this.Mapper.ConfigurationProvider), loadInfo, cancellationToken);
        }
    }
}