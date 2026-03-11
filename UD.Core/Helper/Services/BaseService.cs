namespace UD.Core.Helper.Services
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using System.Data.Common;
    using System.Linq.Dynamic.Core;
    using System.Linq.Expressions;
    using UD.Core.Helper.Configuration;
    using UD.Core.Helper.Paging;
    using static UD.Core.Helper.OrtakTools;
    public interface IBaseService<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto>
    where TContext : DbContext
    where TEntity : class, IBaseEntity
    where TEntityDto : IEntityDto
    where TEntityListDto : IEntityDto
    where TSearchDto : ISearchAndPaginateDto
    {
        TContext Context { get; }
        DbSet<TEntity> DbSet { get; }
        DbConnection GetDbConnection();
        IQueryable<T> SqlQueryRaw<T>(string sql, object parameters);
        Task<int> ExecuteSqlRaw(string sql, object parameters, CancellationToken cancellationToken = default);
        Task<int> SaveChanges(CancellationToken cancellationToken = default);
        Task<TEntityDto?> GetBySearch(TSearchDto searchDto, CancellationToken cancellationToken = default);
        Task<TEntityListDto[]> GetAll(TSearchDto searchDto, CancellationToken cancellationToken = default);
        Task<Paginate<TEntityListDto>> GetAllPaginate(TSearchDto searchDto, bool loadInfo = true, CancellationToken cancellationToken = default);
        Task Delete(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);
        Task DeleteByPredicate(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default);
        Task DeleteRange(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default);
    }
    public abstract class BaseService<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto> : IBaseService<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto>, IDisposable
    where TContext : DbContext
    where TEntity : class, IBaseEntity
    where TEntityDto : IEntityDto
    where TEntityListDto : IEntityDto
    where TSearchDto : ISearchAndPaginateDto
    {
        protected readonly IMapper Mapper;
        protected BaseService(TContext Context, IMapper Mapper)
        {
            this.Context = Context ?? throw new ArgumentNullException(nameof(Context));
            this.Mapper = Mapper ?? throw new ArgumentNullException(nameof(Mapper));
        }
        protected abstract IQueryable<TEntity> ApplyFiltering(IQueryable<TEntity> query, TSearchDto searchDto);
        public TContext Context { get; }
        public DbSet<TEntity> DbSet => this.Context.Set<TEntity>();
        public DbConnection GetDbConnection() => this.Context.Database.GetDbConnection();
        public IQueryable<T> SqlQueryRaw<T>(string sql, object parameters) => this.Context.Database.SqlQueryRaw<T>(sql, Converters.ToSqlParameterFromObject(parameters));
        public Task<int> ExecuteSqlRaw(string sql, object parameters, CancellationToken cancellationToken = default) => this.Context.Database.ExecuteSqlRawAsync(sql, Converters.ToSqlParameterFromObject(parameters), cancellationToken);
        public virtual Task<int> SaveChanges(CancellationToken cancellationToken = default) => this.Context.SaveChangesAsync(cancellationToken);
        public virtual Task<TEntityDto?> GetBySearch(TSearchDto searchDto, CancellationToken cancellationToken = default) => this.ApplyFiltering(this.DbSet, searchDto).AsNoTracking().ProjectTo<TEntityDto>(this.Mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);
        public virtual async Task<TEntityListDto[]> GetAll(TSearchDto searchDto, CancellationToken cancellationToken = default) => (await this.GetAllPaginate(searchDto, false, cancellationToken)).items;
        public virtual Task<Paginate<TEntityListDto>> GetAllPaginate(TSearchDto searchDto, bool loadInfo = true, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(searchDto, nameof(searchDto));
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
            ArgumentNullException.ThrowIfNull(predicate);
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
        public void Dispose() => this.Context.Dispose();
    }
}