namespace UD.Core.Helper.Services
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using UD.Core.Extensions;
    using UD.Core.Helper;
    using UD.Core.Helper.Configuration;
    using UD.Core.Helper.Paging;
    using UD.Core.Helper.Validation;
    public interface IBaseServicePrimary<TContext, TEntity, TKey, TEntityDto, TEntityListDto, TSearchDto, TInsertDto, TUpdateDto> : IBaseService<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto>
    where TContext : DbContext
    where TEntity : class, IBaseEntity
    where TKey : struct
    where TEntityDto : IEntityDto
    where TEntityListDto : IEntityDto
    where TSearchDto : ISearchAndPaginateDto
    where TInsertDto : IEntityDto
    where TUpdateDto : IEntityDto<TKey>
    {
        Task<TEntityDto?> GetById(TKey id, CancellationToken cancellationToken = default);
        Task<TKey> Insert(TInsertDto insertDto, bool autoSave = false, CancellationToken cancellationToken = default);
        Task<TKey[]> InsertRange(IEnumerable<TInsertDto> insertDtos, bool autoSave = false, CancellationToken cancellationToken = default);
        Task Update(TUpdateDto updateDto, bool autoSave = false, CancellationToken cancellationToken = default);
        Task UpdateRange(IEnumerable<TUpdateDto> updateDtos, bool autoSave = false, CancellationToken cancellationToken = default);
        Task DeleteByIds(IEnumerable<TKey> ids, bool autoSave = false, CancellationToken cancellationToken = default);
    }
    public abstract class BaseServicePrimary<TContext, TEntity, TKey, TEntityDto, TEntityListDto, TSearchDto, TInsertDto, TUpdateDto> : BaseService<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto>, IBaseServicePrimary<TContext, TEntity, TKey, TEntityDto, TEntityListDto, TSearchDto, TInsertDto, TUpdateDto>
    where TContext : DbContext
    where TEntity : class, IBaseEntity
    where TKey : struct
    where TEntityDto : IEntityDto
    where TEntityListDto : IEntityDto
    where TSearchDto : ISearchAndPaginateDto
    where TInsertDto : IEntityDto
    where TUpdateDto : IEntityDto<TKey>
    {
        protected BaseServicePrimary(TContext Context, IMapper Mapper) : base(Context, Mapper) { }
        public virtual async Task<TEntityDto?> GetById(TKey id, CancellationToken cancellationToken = default)
        {
            var entity = await this.DbSet.FindAsync([id], cancellationToken);
            return entity == null ? default : this.Mapper.Map<TEntityDto>(entity);
        }
        public virtual async Task<TKey> Insert(TInsertDto insertDto, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            Guard.ThrowIfNull(insertDto, nameof(insertDto));
            var entity = this.Mapper.Map<TEntity>(insertDto);
            await this.DbSet.AddAsync(entity, cancellationToken);
            if (autoSave)
            {
                await this.Context.SaveChangesAsync(cancellationToken);
                return this.GetKeyValue(entity);
            }
            return default;
        }
        public virtual async Task<TKey[]> InsertRange(IEnumerable<TInsertDto> insertDtos, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            Guard.ThrowIfEmpty(insertDtos, nameof(insertDtos));
            var entities = insertDtos.Select(dto => this.Mapper.Map<TEntity>(dto));
            await this.DbSet.AddRangeAsync(entities, cancellationToken);
            if (autoSave)
            {
                await this.Context.SaveChangesAsync(cancellationToken);
                return entities.Select(this.GetKeyValue).ToArray();
            }
            return [];
        }
        public virtual async Task Update(TUpdateDto updateDto, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            Guard.ThrowIfNull(updateDto, nameof(updateDto));
            var entity = await this.DbSet.FindAsync([updateDto.Id], cancellationToken);
            if (entity != null)
            {
                this.Mapper.Map(updateDto, entity);
                if (autoSave) { await this.Context.SaveChangesAsync(cancellationToken); }
            }
        }
        public virtual async Task UpdateRange(IEnumerable<TUpdateDto> updateDtos, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            Guard.ThrowIfEmpty(updateDtos, nameof(updateDtos));
            var tasks = new List<Task>();
            foreach (var updateDto in updateDtos) { tasks.Add(this.Update(updateDto, false, cancellationToken)); }
            await Task.WhenAll(tasks);
            if (autoSave) { await this.Context.SaveChangesAsync(cancellationToken); }
        }
        public virtual async Task DeleteByIds(IEnumerable<TKey> ids, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            if (!ids.IsNullOrEmptyOrAllNull())
            {
                foreach (var id in ids)
                {
                    var entity = await this.DbSet.FindAsync([id], cancellationToken);
                    await base.Delete(entity, false, cancellationToken);
                }
                if (autoSave) { await this.Context.SaveChangesAsync(cancellationToken); }
            }
        }
        protected virtual TKey GetKeyValue(TEntity entity)
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
            return (TKey)value;
        }
    }
}