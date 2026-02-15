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
    public interface IBaseService<TContext, TEntity, TKey, TReturnDto, TSearchDto, TInsertDto, TUpdateDto>
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
        Task<TReturnDto?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
        Task<TReturnDto[]> GetAllAsync(TSearchDto searchDto, CancellationToken cancellationToken = default);
        Task<Paginate<TReturnDto>> GetAllPaginateAsync(TSearchDto searchDto, bool loadinfo = true, CancellationToken cancellationToken = default);
        Task<TKey> InsertAsync(TInsertDto insertDto, bool autoSave = false, CancellationToken cancellationToken = default);
        Task UpdateAsync(TKey id, TUpdateDto updateDto, bool autoSave = false, CancellationToken cancellationToken = default);
        Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);
        Task DeleteByIdAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default);
        Task DeleteByPredicateAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationtoken = default);
        Task DeleteRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationtoken = default);
    }
    /// <summary> Entity Framework Core tabanlı uygulamalarda temel CRUD (Ekle, Getir, Güncelle, Sil) işlemlerini ve listeleme/sayfalama süreçlerini yöneten generic bir servis sınıfıdır. </summary>
    public abstract class BaseService<TContext, TEntity, TKey, TReturnDto, TSearchDto, TInsertDto, TUpdateDto> : IBaseService<TContext, TEntity, TKey, TReturnDto, TSearchDto, TInsertDto, TUpdateDto>, IDisposable
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
        public virtual async Task<TReturnDto?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var entity = await this.DbSet.FindAsync(new object[] { id }, cancellationToken);
            return entity == null ? null : this.mapper.Map<TReturnDto>(entity);
        }
        public virtual async Task<TReturnDto[]> GetAllAsync(TSearchDto searchDto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(searchDto);
            var query = ApplyFiltering(this.DbSet, searchDto);
            var entities = await searchDto.Paginate(query).ToArrayAsync(cancellationToken);
            return this.mapper.Map<TReturnDto[]>(entities);
        }
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
        public virtual async Task<TKey> InsertAsync(TInsertDto insertDto, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(insertDto);
            var entity = this.mapper.Map<TEntity>(insertDto);
            await this.DbSet.AddAsync(entity, cancellationToken);
            if (autoSave) { await this.context.SaveChangesAsync(cancellationToken); }
            return this.GetKeyValue(entity);
        }
        public virtual async Task UpdateAsync(TKey id, TUpdateDto updateDto, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(updateDto);
            var entity = await this.DbSet.FindAsync(new object[] { id }, cancellationToken);
            if (entity != null)
            {
                this.mapper.Map(updateDto, entity);
                if (autoSave) { await this.context.SaveChangesAsync(cancellationToken); }
            }
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
        public virtual async Task DeleteByIdAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var entity = await this.DbSet.FindAsync(new object[] { id }, cancellationToken);
            await this.DeleteAsync(entity, autoSave, cancellationToken);
        }
        public virtual async Task DeleteByPredicateAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationtoken = default)
        {
            ArgumentNullException.ThrowIfNull(predicate);
            var entities = await this.DbSet.Where(predicate).ToArrayAsync(cancellationtoken);
            await this.DeleteRangeAsync(entities, autoSave, cancellationtoken);
        }
        public virtual async Task DeleteRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationtoken = default)
        {
            if (entities != null) { foreach (var entity in entities) { await this.DeleteAsync(entity, autoSave, cancellationtoken); } }
        }
        protected virtual TKey GetKeyValue(TEntity entity)
        {
            var properties = this.context.Model.FindEntityType(typeof(TEntity))?.FindPrimaryKey()?.Properties;
            var keyname = (properties != null && properties.Count > 0) ? properties[0].Name : "";
            if (keyname.IsNullOrEmpty()) { throw new InvalidOperationException("PK not found."); }
            var property = typeof(TEntity).GetProperty(keyname);
            if (property == null) { throw new InvalidOperationException($"Property \"{keyname}\" not found on {typeof(TEntity).Name}."); }
            return (TKey)property.GetValue(entity)!;
        }
        public void Dispose() => this.context.Dispose();
    }
}