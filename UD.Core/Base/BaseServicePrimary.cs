namespace UD.Core.Base
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using UD.Core.Extensions;
    using UD.Core.Helper;
    using UD.Core.Helper.Paging;
    public interface IBaseServicePrimary<TContext, TEntity, TKey, TEntityDto, TSearchDto, TInsertDto, TUpdateDto> : IBaseService<TContext, TEntity, TEntityDto, TSearchDto>
    where TContext : DbContext
    where TEntity : class
    where TKey : struct
    where TEntityDto : IEntityDto
    where TSearchDto : ISearchAndPaginateDto
    where TInsertDto : IEntityDto
    where TUpdateDto : IEntityDto
    {
        Task<TEntityDto?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
        Task<TKey> InsertAsync(TInsertDto insertDto, bool autoSave = false, CancellationToken cancellationToken = default);
        Task UpdateAsync(TKey id, TUpdateDto updateDto, bool autoSave = false, CancellationToken cancellationToken = default);
        Task DeleteByIdAsync(IEnumerable<TKey> ids, bool autoSave = false, CancellationToken cancellationToken = default);
    }
    public abstract class BaseServicePrimary<TContext, TEntity, TKey, TEntityDto, TSearchDto, TInsertDto, TUpdateDto> : BaseService<TContext, TEntity, TEntityDto, TSearchDto>, IBaseServicePrimary<TContext, TEntity, TKey, TEntityDto, TSearchDto, TInsertDto, TUpdateDto>
    where TContext : DbContext
    where TEntity : class
    where TKey : struct
    where TEntityDto : IEntityDto
    where TSearchDto : ISearchAndPaginateDto
    where TInsertDto : IEntityDto
    where TUpdateDto : IEntityDto
    {
        protected BaseServicePrimary(TContext context, IMapper mapper) : base(context, mapper) { }
        public virtual async Task<TEntityDto?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var entity = await this.DbSet.FindAsync(new object[] { id }, cancellationToken);
            return entity == null ? default : this.mapper.Map<TEntityDto>(entity);
        }
        public virtual async Task<TKey> InsertAsync(TInsertDto insertDto, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(insertDto, nameof(insertDto));
            var entity = this.mapper.Map<TEntity>(insertDto);
            await this.DbSet.AddAsync(entity, cancellationToken);
            if (autoSave)
            {
                await this.context.SaveChangesAsync(cancellationToken);
                return this.GetKeyValue(entity);
            }
            return default;
        }
        public virtual async Task UpdateAsync(TKey id, TUpdateDto updateDto, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(updateDto, nameof(updateDto));
            var entity = await this.DbSet.FindAsync(new object[] { id }, cancellationToken);
            if (entity != null)
            {
                this.mapper.Map(updateDto, entity);
                if (autoSave) { await this.context.SaveChangesAsync(cancellationToken); }
            }
        }
        public virtual async Task DeleteByIdAsync(IEnumerable<TKey> ids, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            if (ids != null)
            {
                foreach (var id in ids)
                {
                    var entity = await this.DbSet.FindAsync(new object[] { id }, cancellationToken);
                    await base.DeleteAsync(entity, false, cancellationToken);
                }
                if (autoSave) { await this.context.SaveChangesAsync(cancellationToken); }
            }
        }
        protected virtual TKey GetKeyValue(TEntity entity)
        {
            var t = typeof(TEntity);
            var properties = this.context.Model.FindEntityType(t)?.FindPrimaryKey()?.Properties;
            var keyname = (properties != null && properties.Count > 0) ? properties[0].Name : "";
            if (keyname.IsNullOrEmpty()) { throw new InvalidOperationException("PK not found"); }
            var property = t.GetProperty(keyname);
            if (property == null) { throw new InvalidOperationException($"Property \"{keyname}\" not found on {t.Name}"); }
            var value = property.GetValue(entity);
            if (value == null) { throw new InvalidOperationException($"Key value is null"); }
            return (TKey)value;
        }
    }
}