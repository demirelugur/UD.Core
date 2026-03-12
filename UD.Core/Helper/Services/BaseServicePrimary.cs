namespace UD.Core.Helper.Services
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using UD.Core.Extensions;
    using UD.Core.Helper.Configuration;
    using UD.Core.Helper.Paging;
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
        Task Update(TUpdateDto updateDto, bool autoSave = false, CancellationToken cancellationToken = default);
        Task DeleteById(IEnumerable<TKey> ids, bool autoSave = false, CancellationToken cancellationToken = default);
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
            ArgumentNullException.ThrowIfNull(insertDto, nameof(insertDto));
            var entity = this.Mapper.Map<TEntity>(insertDto);
            await this.DbSet.AddAsync(entity, cancellationToken);
            if (autoSave)
            {
                await this.Context.SaveChangesAsync(cancellationToken);
                return this.GetKeyValue(entity);
            }
            return default;
        }
        public virtual async Task Update(TUpdateDto updateDto, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(updateDto, nameof(updateDto));
            var entity = await this.DbSet.FindAsync([updateDto.Id], cancellationToken);
            if (entity != null)
            {
                this.Mapper.Map(updateDto, entity);
                if (autoSave) { await this.Context.SaveChangesAsync(cancellationToken); }
            }
        }
        public virtual async Task DeleteById(IEnumerable<TKey> ids, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            if (ids != null)
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
            var keyName = (properties != null && properties.Count > 0) ? properties[0].Name : "";
            if (keyName.IsNullOrEmpty()) { throw new InvalidOperationException("PK not found"); }
            var property = type.GetProperty(keyName);
            if (property == null) { throw new InvalidOperationException($"Property \"{keyName}\" not found on {type.Name}"); }
            var value = property.GetValue(entity);
            if (value == null) { throw new InvalidOperationException($"Key value is null"); }
            return (TKey)value;
        }
    }
}