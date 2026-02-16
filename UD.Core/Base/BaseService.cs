namespace UD.Core.Base
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using System.Data.Common;
    using System.Linq.Dynamic.Core;
    using System.Linq.Expressions;
    using UD.Core.Extensions;
    using UD.Core.Helper.Paging;
    using static UD.Core.Helper.OrtakTools;
    public interface IBaseService<TContext, TEntity, TReturnDto, TSearchDto, TInsertDto, TUpdateDto>
    where TContext : DbContext
    where TEntity : class
    where TReturnDto : class
    where TSearchDto : ISearchAndPaginateDto
    where TInsertDto : class
    where TUpdateDto : class
    {
        DbSet<TEntity> DbSet { get; }
        DbConnection GetDbConnection();
        IQueryable<T> SqlQueryRaw<T>(string sql, object parameters);
        Task<int> ExecuteSqlRawAsync(string sql, object parameters, CancellationToken cancellationtoken = default);
        Task<TReturnDto[]> GetAllAsync(TSearchDto searchDto, CancellationToken cancellationToken = default);
        Task<Paginate<TReturnDto>> GetAllPaginateAsync(TSearchDto searchDto, bool loadinfo = true, CancellationToken cancellationToken = default);
        Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);
        Task DeleteByPredicateAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationtoken = default);
        Task DeleteRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationtoken = default);
    }
    public abstract class BaseService<TContext, TEntity, TReturnDto, TSearchDto, TInsertDto, TUpdateDto> : IBaseService<TContext, TEntity, TReturnDto, TSearchDto, TInsertDto, TUpdateDto>, IDisposable
    where TContext : DbContext
    where TEntity : class
    where TReturnDto : class
    where TSearchDto : ISearchAndPaginateDto
    where TInsertDto : class
    where TUpdateDto : class
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
        public IQueryable<T> SqlQueryRaw<T>(string sql, object parameters) => this.context.Database.SqlQueryRaw<T>(sql, _to.ToSqlParameterFromObject(parameters));
        public Task<int> ExecuteSqlRawAsync(string sql, object parameters, CancellationToken cancellationtoken = default) => this.context.Database.ExecuteSqlRawAsync(sql, _to.ToSqlParameterFromObject(parameters), cancellationtoken);
        public virtual async Task<TReturnDto[]> GetAllAsync(TSearchDto searchDto, CancellationToken cancellationToken = default) => (await this.GetAllPaginateAsync(searchDto, false, cancellationToken)).items;
        public virtual Task<Paginate<TReturnDto>> GetAllPaginateAsync(TSearchDto searchDto, bool loadinfo = true, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(searchDto);
            var query = ApplyFiltering(this.DbSet, searchDto);
            if (!searchDto.sorting.IsNullOrEmpty())
            {
                try { query = query.OrderBy(searchDto.sorting); }
                catch (Exception ex) { throw new InvalidOperationException($"Sorting failed: {searchDto.sorting}", ex); }
            }
            return query.ProjectTo<TReturnDto>(this.mapper.ConfigurationProvider).ToPagedListAsync(searchDto.pagenumber, searchDto.size, loadinfo, cancellationToken);
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
        public virtual async Task DeleteByPredicateAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationtoken = default)
        {
            ArgumentNullException.ThrowIfNull(predicate);
            var entities = await this.DbSet.Where(predicate).ToArrayAsync(cancellationtoken);
            await this.DeleteRangeAsync(entities, autoSave, cancellationtoken);
        }
        public virtual async Task DeleteRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationtoken = default)
        {
            if (entities != null)
            {
                foreach (var entity in entities) { await this.DeleteAsync(entity, false, cancellationtoken); }
                if (autoSave) { await this.context.SaveChangesAsync(cancellationtoken); }
            }
        }
        public void Dispose() => this.context.Dispose();
    }
}