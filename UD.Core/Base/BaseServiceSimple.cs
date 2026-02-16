namespace UD.Core.Base
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using UD.Core.Extensions;
    using UD.Core.Helper.Paging;
    public interface IBaseServiceSimple<TContext, TEntity, TKey, TReturnDto, TSearchDto, TInsertDto, TUpdateDto> : IBaseService<TContext, TEntity, TReturnDto, TSearchDto, TInsertDto, TUpdateDto>
    where TContext : DbContext
    where TEntity : class
    where TReturnDto : class
    where TSearchDto : ISearchAndPaginateDto
    where TInsertDto : class
    where TUpdateDto : class
    {
        Task<TReturnDto?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
        Task<TKey> InsertAsync(TInsertDto insertDto, bool autoSave = false, CancellationToken cancellationToken = default);
        Task UpdateAsync(TKey id, TUpdateDto updateDto, bool autoSave = false, CancellationToken cancellationToken = default);
        Task DeleteByIdAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default);
    }
    public abstract class BaseServiceSimple<TContext, TEntity, TKey, TReturnDto, TSearchDto, TInsertDto, TUpdateDto> : BaseService<TContext, TEntity, TReturnDto, TSearchDto, TInsertDto, TUpdateDto>, IBaseServiceSimple<TContext, TEntity, TKey, TReturnDto, TSearchDto, TInsertDto, TUpdateDto>
    where TContext : DbContext
    where TEntity : class
    where TReturnDto : class
    where TSearchDto : ISearchAndPaginateDto
    where TInsertDto : class
    where TUpdateDto : class
    {
        protected BaseServiceSimple(TContext context, IMapper mapper) : base(context, mapper) { }
        public virtual async Task<TReturnDto?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var entity = await this.DbSet.FindAsync(new object[] { id }, cancellationToken);
            return entity == null ? null : this.mapper.Map<TReturnDto>(entity);
        }
        public virtual async Task<TKey> InsertAsync(TInsertDto insertDto, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(insertDto);
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
            ArgumentNullException.ThrowIfNull(updateDto);
            var entity = await this.DbSet.FindAsync(new object[] { id }, cancellationToken);
            if (entity != null)
            {
                this.mapper.Map(updateDto, entity);
                if (autoSave) { await this.context.SaveChangesAsync(cancellationToken); }
            }
        }
        public virtual async Task DeleteByIdAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var entity = await this.DbSet.FindAsync(new object[] { id }, cancellationToken);
            await this.DeleteAsync(entity, autoSave, cancellationToken);
        }
        protected virtual TKey GetKeyValue(TEntity entity)
        {
            var t = typeof(TEntity);
            var properties = this.context.Model.FindEntityType(t)?.FindPrimaryKey()?.Properties;
            var keyname = (properties != null && properties.Count > 0) ? properties[0].Name : "";
            if (keyname.IsNullOrEmpty()) { throw new InvalidOperationException("PK not found."); }
            var property = t.GetProperty(keyname);
            if (property == null) { throw new InvalidOperationException($"Property \"{keyname}\" not found on {t.Name}."); }
            return (TKey)property.GetValue(entity);
        }
    }
}