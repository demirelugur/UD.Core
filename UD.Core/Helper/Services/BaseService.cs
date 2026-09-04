namespace UD.Core.Helper.Services
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using System.Linq.Dynamic.Core;
    using System.Linq.Expressions;
    using UD.Core.Extensions;
    using UD.Core.Helper.Configurations;
    using UD.Core.Helper.Pages;
    public interface IBaseService<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto, TInsertDto, TUpdateDto> : IBaseServiceReadOnly<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto>
    where TContext : DbContext
    where TEntity : class, IBaseEntity
    where TEntityDto : class, IEntityDto
    where TEntityListDto : class, IEntityDto
    where TSearchDto : class, ISearchAndPaginateDto
    where TInsertDto : class, IEntityDto
    where TUpdateDto : class, IEntityDto
    {
        Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);
        Task DeleteByPredicateAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default);
        Task DeleteRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default);
        Task DeleteByIdAsync(object id, bool autoSave = false, CancellationToken cancellationToken = default);
        Task InsertAsync(TInsertDto insertDto, bool autoSave = false, CancellationToken cancellationToken = default);
        Task InsertRangeAsync(IEnumerable<TInsertDto> insertDtos, bool autoSave = false, CancellationToken cancellationToken = default);
        Task<TKey> InsertReturningIdAsync<TKey>(TInsertDto insertDto, bool autoSave = false, CancellationToken cancellationToken = default) where TKey : struct;
        Task<TKey[]> InsertRangeReturningIdsAsync<TKey>(IEnumerable<TInsertDto> insertDtos, bool autoSave = false, CancellationToken cancellationToken = default) where TKey : struct;
        Task UpdateAsync(object id, TUpdateDto updateDto, bool autoSave = false, CancellationToken cancellationToken = default);
    }
    public abstract class BaseService<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto, TInsertDto, TUpdateDto> : BaseServiceReadOnly<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto>, IBaseService<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto, TInsertDto, TUpdateDto>
    where TContext : DbContext
    where TEntity : class, IBaseEntity
    where TEntityDto : class, IEntityDto
    where TEntityListDto : class, IEntityDto
    where TSearchDto : class, ISearchAndPaginateDto
    where TInsertDto : class, IEntityDto
    where TUpdateDto : class, IEntityDto
    {
        protected BaseService(TContext context, IMapper mapper) : base(context, mapper) { }
        public virtual async Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            if (entity != null)
            {
                if (base.Context.Entry(entity).State == EntityState.Detached) { base.DbSet.Attach(entity); }
                base.DbSet.Remove(entity);
                if (autoSave) { await base.Context.SaveChangesAsync(cancellationToken); }
            }
        }
        public virtual async Task DeleteByPredicateAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var entities = await base.DbSet.Where(predicate).ToArrayAsync(cancellationToken);
            await this.DeleteRangeAsync(entities, autoSave, cancellationToken);
        }
        public virtual async Task DeleteRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            if (!entities.IsNullOrEmptyOrAllNull())
            {
                foreach (var entity in entities) { await this.DeleteAsync(entity, false, cancellationToken); }
                if (autoSave) { await base.Context.SaveChangesAsync(cancellationToken); }
            }
        }
        public virtual async Task DeleteByIdAsync(object id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            if (TryGetKeyValues(id, out var _keyValues))
            {
                var entity = await base.DbSet.FindAsync(_keyValues, cancellationToken);
                await this.DeleteAsync(entity, autoSave, cancellationToken);
            }
        }
        public virtual async Task InsertAsync(TInsertDto insertDto, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var entity = base.Mapper.Map<TEntity>(insertDto);
            await base.DbSet.AddAsync(entity, cancellationToken);
            if (autoSave) { await base.Context.SaveChangesAsync(cancellationToken); }
        }
        public virtual async Task InsertRangeAsync(IEnumerable<TInsertDto> insertDtos, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var entities = insertDtos.Select(base.Mapper.Map<TEntity>);
            await base.DbSet.AddRangeAsync(entities, cancellationToken);
            if (autoSave) { await base.Context.SaveChangesAsync(cancellationToken); }
        }
        public virtual async Task<TKey> InsertReturningIdAsync<TKey>(TInsertDto insertDto, bool autoSave = false, CancellationToken cancellationToken = default) where TKey : struct
        {
            var entity = base.Mapper.Map<TEntity>(insertDto);
            await base.DbSet.AddAsync(entity, cancellationToken);
            if (autoSave)
            {
                await base.Context.SaveChangesAsync(cancellationToken);
                return (TKey)this.GetKeyValue(entity);
            }
            return default;
        }
        public virtual async Task<TKey[]> InsertRangeReturningIdsAsync<TKey>(IEnumerable<TInsertDto> insertDtos, bool autoSave = false, CancellationToken cancellationToken = default) where TKey : struct
        {
            var entities = insertDtos.Select(base.Mapper.Map<TEntity>);
            await base.DbSet.AddRangeAsync(entities, cancellationToken);
            if (autoSave)
            {
                await base.Context.SaveChangesAsync(cancellationToken);
                return entities.Select(x => (TKey)this.GetKeyValue(x)).ToArray();
            }
            return [];
        }
        public virtual async Task UpdateAsync(object id, TUpdateDto updateDto, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            if (TryGetKeyValues(id, out var _keyValues))
            {
                var entity = await base.DbSet.FindAsync(_keyValues, cancellationToken);
                if (entity != null)
                {
                    base.Mapper.Map(updateDto, entity);
                    if (autoSave) { await base.Context.SaveChangesAsync(cancellationToken); }
                }
            }
        }
        protected virtual object GetKeyValue(TEntity entity)
        {
            var type = typeof(TEntity);
            var properties = this.Context.Model.FindEntityType(type)?.FindPrimaryKey()?.Properties;
            var keyName = (properties.IsNullOrEmptyOrAllNull() ? "" : properties[0].Name);
            if (keyName.IsNullOrEmpty())
            {
                if (Checks.IsEnglishCurrentUICulture) { throw new InvalidOperationException("PK not found"); }
                throw new InvalidOperationException("Birincil Anahtar(PK) bulunamadı!");
            }
            var property = type.GetProperty(keyName);
            if (property == null)
            {
                if (Checks.IsEnglishCurrentUICulture) { throw new InvalidOperationException($"Property \"{keyName}\" not found on {type.Name}"); }
                throw new InvalidOperationException($"\"{keyName}\" özelliği \"{type.Name}\" üzerinde bulunamadı!");
            }
            var value = property.GetValue(entity);
            if (value == null)
            {
                if (Checks.IsEnglishCurrentUICulture) { throw new InvalidOperationException($"Key value is null"); }
                throw new InvalidOperationException($"Anahtar(Key) değeri boş.");
            }
            return value;
        }
    }
}