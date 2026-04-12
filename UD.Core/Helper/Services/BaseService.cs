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
    public interface IBaseService<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto, TInsertDto, TUpdateDto> : IBaseServiceReadOnly<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto>
    where TContext : DbContext
    where TEntity : class, IBaseEntity
    where TEntityDto : class, IEntityDto
    where TEntityListDto : class, IEntityDto
    where TSearchDto : class, ISearchAndPaginateDto
    where TInsertDto : class, IEntityDto
    where TUpdateDto : class, IEntityDto
    {
        Task Delete(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);
        Task DeleteByPredicate(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default);
        Task DeleteRange(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default);
        Task DeleteById(object id, bool autoSave = false, CancellationToken cancellationToken = default);
        Task Insert(TInsertDto insertDto, bool autoSave = false, CancellationToken cancellationToken = default);
        Task InsertRange(IEnumerable<TInsertDto> insertDtos, bool autoSave = false, CancellationToken cancellationToken = default);
        Task<TKey> InsertReturningId<TKey>(TInsertDto insertDto, bool autoSave = false, CancellationToken cancellationToken = default) where TKey : struct;
        Task<TKey[]> InsertRangeReturningIds<TKey>(IEnumerable<TInsertDto> insertDtos, bool autoSave = false, CancellationToken cancellationToken = default) where TKey : struct;
        Task Update(object id, TUpdateDto updateDto, bool autoSave = false, CancellationToken cancellationToken = default);
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
        protected BaseService(TContext Context, IMapper Mapper) : base(Context, Mapper) { }
        public virtual async Task Delete(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            if (entity != null)
            {
                if (base.Context.Entry(entity).State == EntityState.Detached) { base.DbSet.Attach(entity); }
                base.DbSet.Remove(entity);
                if (autoSave) { await base.Context.SaveChangesAsync(cancellationToken); }
            }
        }
        public virtual async Task DeleteByPredicate(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            Guard.ThrowIfNull(predicate, nameof(predicate));
            var entities = await base.DbSet.Where(predicate).ToArrayAsync(cancellationToken);
            await this.DeleteRange(entities, autoSave, cancellationToken);
        }
        public virtual async Task DeleteRange(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            if (!entities.IsNullOrEmptyOrAllNull())
            {
                foreach (var entity in entities) { await this.Delete(entity, false, cancellationToken); }
                if (autoSave) { await base.Context.SaveChangesAsync(cancellationToken); }
            }
        }
        public virtual async Task DeleteById(object id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            if (base.TryGetKeyValues(id, out object[] _keyValues))
            {
                var entity = await base.DbSet.FindAsync(_keyValues, cancellationToken);
                await this.Delete(entity, autoSave, cancellationToken);
            }
        }
        public virtual async Task Insert(TInsertDto insertDto, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            Guard.ThrowIfNull(insertDto, nameof(insertDto));
            var entity = base.Mapper.Map<TEntity>(insertDto);
            await base.DbSet.AddAsync(entity, cancellationToken);
            if (autoSave) { await base.Context.SaveChangesAsync(cancellationToken); }
        }
        public virtual async Task InsertRange(IEnumerable<TInsertDto> insertDtos, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            Guard.ThrowIfEmpty(insertDtos, nameof(insertDtos));
            var entities = insertDtos.Select(base.Mapper.Map<TEntity>);
            await base.DbSet.AddRangeAsync(entities, cancellationToken);
            if (autoSave) { await base.Context.SaveChangesAsync(cancellationToken); }
        }
        public virtual async Task<TKey> InsertReturningId<TKey>(TInsertDto insertDto, bool autoSave = false, CancellationToken cancellationToken = default) where TKey : struct
        {
            Guard.ThrowIfNull(insertDto, nameof(insertDto));
            var entity = base.Mapper.Map<TEntity>(insertDto);
            await base.DbSet.AddAsync(entity, cancellationToken);
            if (autoSave)
            {
                await base.Context.SaveChangesAsync(cancellationToken);
                return this.GetKeyValue<TKey>(entity);
            }
            return default;
        }
        public virtual async Task<TKey[]> InsertRangeReturningIds<TKey>(IEnumerable<TInsertDto> insertDtos, bool autoSave = false, CancellationToken cancellationToken = default) where TKey : struct
        {
            Guard.ThrowIfEmpty(insertDtos, nameof(insertDtos));
            var entities = insertDtos.Select(base.Mapper.Map<TEntity>);
            await base.DbSet.AddRangeAsync(entities, cancellationToken);
            if (autoSave)
            {
                await base.Context.SaveChangesAsync(cancellationToken);
                return entities.Select(this.GetKeyValue<TKey>).ToArray();
            }
            return [];
        }
        public virtual async Task Update(object id, TUpdateDto updateDto, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            if (base.TryGetKeyValues(id, out object[] _keyValues))
            {
                Guard.ThrowIfNull(updateDto, nameof(updateDto));
                var entity = await base.DbSet.FindAsync(_keyValues, cancellationToken);
                if (entity != null)
                {
                    base.Mapper.Map(updateDto, entity);
                    if (autoSave) { await base.Context.SaveChangesAsync(cancellationToken); }
                }
            }
        }
        protected virtual TKey GetKeyValue<TKey>(TEntity entity)
        {
            var type = typeof(TEntity);
            var properties = base.Context.Model.FindEntityType(type)?.FindPrimaryKey()?.Properties;
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
            if (value is TKey _tKeyValue) { return _tKeyValue; }
            if (Checks.IsEnglishCurrentUICulture) { throw new InvalidOperationException($"The value of the primary key property \"{keyName}\" on entity \"{type.Name}\". Cannot be converted to type \"{typeof(TKey).FullName}\"."); }
            throw new InvalidOperationException($"Birincil Anahtar(PK) özelliği \"{keyName}\" değeri, varlık \"{type.Name}\" üzerinde istenen türe (\"{typeof(TKey).FullName}\") dönüştürülemiyor.");
        }
    }
}