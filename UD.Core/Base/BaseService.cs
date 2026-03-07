namespace UD.Core.Base
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
    public interface IBaseService<TContext, TEntity, TEntityListDto, TSearchDto>
    where TContext : DbContext
    where TEntity : class, IEntity
    where TEntityListDto : IEntityDto
    where TSearchDto : ISearchAndPaginateDto
    {
        DbSet<TEntity> DbSet { get; }
        DbConnection GetDbConnection();
        IQueryable<T> SqlQueryRaw<T>(string sql, object parameters);
        Task<int> ExecuteSqlRawAsync(string sql, object parameters, CancellationToken cancellationToken = default);
        Task<TEntityListDto[]> GetAllAsync(TSearchDto searchDto, QueryTrackingBehavior track = QueryTrackingBehavior.NoTracking, CancellationToken cancellationToken = default);
        Task<Paginate<TEntityListDto>> GetAllPaginateAsync(TSearchDto searchDto, QueryTrackingBehavior track = QueryTrackingBehavior.NoTracking, bool loadinfo = true, CancellationToken cancellationToken = default);
        Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);
        Task DeleteByPredicateAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default);
        Task DeleteRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default);
    }
    public abstract class BaseService<TContext, TEntity, TEntityListDto, TSearchDto> : IBaseService<TContext, TEntity, TEntityListDto, TSearchDto>, IDisposable
    where TContext : DbContext
    where TEntity : class, IEntity
    where TEntityListDto : IEntityDto
    where TSearchDto : ISearchAndPaginateDto
    {
        protected readonly TContext context;
        protected readonly IMapper mapper;
        protected BaseService(TContext context, IMapper mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        protected abstract IQueryable<TEntity> ApplyFiltering(IQueryable<TEntity> query, TSearchDto searchDto);
        public DbSet<TEntity> DbSet => this.context.Set<TEntity>();
        public DbConnection GetDbConnection() => this.context.Database.GetDbConnection();
        public IQueryable<T> SqlQueryRaw<T>(string sql, object parameters) => this.context.Database.SqlQueryRaw<T>(sql, Converters.ToSqlParameterFromObject(parameters));
        public Task<int> ExecuteSqlRawAsync(string sql, object parameters, CancellationToken cancellationToken = default) => this.context.Database.ExecuteSqlRawAsync(sql, Converters.ToSqlParameterFromObject(parameters), cancellationToken);
        public virtual async Task<TEntityListDto[]> GetAllAsync(TSearchDto searchDto, QueryTrackingBehavior track = QueryTrackingBehavior.NoTracking, CancellationToken cancellationToken = default) => (await this.GetAllPaginateAsync(searchDto, track, false, cancellationToken)).items;
        public virtual Task<Paginate<TEntityListDto>> GetAllPaginateAsync(TSearchDto searchDto, QueryTrackingBehavior track = QueryTrackingBehavior.NoTracking, bool loadinfo = true, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(searchDto, nameof(searchDto));
            return searchDto.ToPagedListAsync(this.ApplyFiltering(this.DbSet, searchDto).AsTracking(track).ProjectTo<TEntityListDto>(this.mapper.ConfigurationProvider), loadinfo, cancellationToken);
        }
        public virtual async Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            if (entity != null)
            {
                if (this.context.Entry(entity).State == EntityState.Detached) { this.DbSet.Attach(entity); }
                this.DbSet.Remove(entity);
                if (autoSave) { await this.context.SaveChangesAsync(cancellationToken); }
            }
        }
        public virtual async Task DeleteByPredicateAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(predicate);
            var entities = await this.DbSet.Where(predicate).ToArrayAsync(cancellationToken);
            await this.DeleteRangeAsync(entities, autoSave, cancellationToken);
        }
        public virtual async Task DeleteRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            if (entities != null)
            {
                foreach (var entity in entities) { await this.DeleteAsync(entity, false, cancellationToken); }
                if (autoSave) { await this.context.SaveChangesAsync(cancellationToken); }
            }
        }
        public void Dispose() => this.context.Dispose();
    }
}