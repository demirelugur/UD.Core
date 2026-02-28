namespace UD.Core.Base
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using UD.Core.Extensions;
    using UD.Core.Helper;
    using UD.Core.Helper.Paging;
    public interface IBaseServiceComplexKey<TContext, TEntity, TEntityDto, TSearchDto, TInsertDto, TUpdateDto> : IBaseService<TContext, TEntity, TEntityDto, TSearchDto>
    where TContext : DbContext
    where TEntity : class
    where TEntityDto : IEntityDto
    where TSearchDto : ISearchAndPaginateDto
    where TInsertDto : IEntityDto
    where TUpdateDto : IEntityDto
    {
        Task<TEntityDto?> GetByIdAsync(object[] keyvalues, CancellationToken cancellationToken = default);
        Task InsertAsync(TInsertDto insertDto, bool autoSave = false, CancellationToken cancellationToken = default);
        Task UpdateAsync(object[] keyvalues, TUpdateDto updateDto, bool autoSave = false, CancellationToken cancellationToken = default);
        Task DeleteByIdAsync(object[] keyvalues, bool autoSave = false, CancellationToken cancellationToken = default);
    }
    public abstract class BaseServiceComplexKey<TContext, TEntity, TEntityDto, TSearchDto, TInsertDto, TUpdateDto> : BaseService<TContext, TEntity, TEntityDto, TSearchDto>, IBaseServiceComplexKey<TContext, TEntity, TEntityDto, TSearchDto, TInsertDto, TUpdateDto>
    where TContext : DbContext
    where TEntity : class
    where TEntityDto : IEntityDto
    where TSearchDto : ISearchAndPaginateDto
    where TInsertDto : IEntityDto
    where TUpdateDto : IEntityDto
    {
        protected BaseServiceComplexKey(TContext context, IMapper mapper) : base(context, mapper) { }
        public virtual async Task<TEntityDto?> GetByIdAsync(object[] keyvalues, CancellationToken cancellationToken = default)
        {
            if (keyvalues.IsNullOrCountZero()) { return default; }
            var entity = await this.DbSet.FindAsync(keyvalues, cancellationToken);
            return entity == null ? default : this.mapper.Map<TEntityDto>(entity);
        }
        public virtual async Task InsertAsync(TInsertDto insertDto, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(insertDto);
            var entity = this.mapper.Map<TEntity>(insertDto);
            await this.DbSet.AddAsync(entity, cancellationToken);
            if (autoSave) { await this.context.SaveChangesAsync(cancellationToken); }
        }
        public virtual async Task UpdateAsync(object[] keyvalues, TUpdateDto updateDto, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            if (!keyvalues.IsNullOrCountZero())
            {
                ArgumentNullException.ThrowIfNull(updateDto);
                var entity = await this.DbSet.FindAsync(keyvalues, cancellationToken);
                if (entity != null)
                {
                    this.mapper.Map(updateDto, entity);
                    if (autoSave) { await this.context.SaveChangesAsync(cancellationToken); }
                }
            }
        }
        public virtual async Task DeleteByIdAsync(object[] keyvalues, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            if (!keyvalues.IsNullOrCountZero())
            {
                var entity = await this.DbSet.FindAsync(keyvalues, cancellationToken);
                await base.DeleteAsync(entity, autoSave, cancellationToken);
            }
        }
    }
}