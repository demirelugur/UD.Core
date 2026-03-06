namespace UD.Core.Base
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using UD.Core.Extensions;
    using UD.Core.Helper.Configuration;
    using UD.Core.Helper.Paging;
    public interface IBaseServiceComplexKey<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto, TInsertDto, TUpdateDto> : IBaseService<TContext, TEntity, TEntityListDto, TSearchDto>
    where TContext : DbContext
    where TEntity : class, IEntity
    where TEntityDto : IEntityDto
    where TEntityListDto : IEntityDto
    where TSearchDto : ISearchAndPaginateDto
    where TInsertDto : IEntityDto
    where TUpdateDto : IEntityDto
    {
        Task<TEntityDto?> GetByIdAsync(object[] keyValues, CancellationToken cancellationToken = default);
        Task InsertAsync(TInsertDto insertDto, bool autoSave = false, CancellationToken cancellationToken = default);
        Task UpdateAsync(object[] keyValues, TUpdateDto updateDto, bool autoSave = false, CancellationToken cancellationToken = default);
        Task DeleteByIdAsync(object[] keyValues, bool autoSave = false, CancellationToken cancellationToken = default);
    }
    public abstract class BaseServiceComplexKey<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto, TInsertDto, TUpdateDto> : BaseService<TContext, TEntity, TEntityListDto, TSearchDto>, IBaseServiceComplexKey<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto, TInsertDto, TUpdateDto>
    where TContext : DbContext
    where TEntity : class, IEntity
    where TEntityDto : IEntityDto
    where TEntityListDto : IEntityDto
    where TSearchDto : ISearchAndPaginateDto
    where TInsertDto : IEntityDto
    where TUpdateDto : IEntityDto
    {
        protected BaseServiceComplexKey(TContext context, IMapper mapper) : base(context, mapper) { }
        public virtual async Task<TEntityDto?> GetByIdAsync(object[] keyValues, CancellationToken cancellationToken = default)
        {
            if (keyValues.IsNullOrCountZero()) { return default; }
            var entity = await this.DbSet.FindAsync(keyValues, cancellationToken);
            return entity == null ? default : this.mapper.Map<TEntityDto>(entity);
        }
        public virtual async Task InsertAsync(TInsertDto insertDto, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(insertDto);
            var entity = this.mapper.Map<TEntity>(insertDto);
            await this.DbSet.AddAsync(entity, cancellationToken);
            if (autoSave) { await this.context.SaveChangesAsync(cancellationToken); }
        }
        public virtual async Task UpdateAsync(object[] keyValues, TUpdateDto updateDto, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            if (!keyValues.IsNullOrCountZero())
            {
                ArgumentNullException.ThrowIfNull(updateDto);
                var entity = await this.DbSet.FindAsync(keyValues, cancellationToken);
                if (entity != null)
                {
                    this.mapper.Map(updateDto, entity);
                    if (autoSave) { await this.context.SaveChangesAsync(cancellationToken); }
                }
            }
        }
        public virtual async Task DeleteByIdAsync(object[] keyValues, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            if (!keyValues.IsNullOrCountZero())
            {
                var entity = await this.DbSet.FindAsync(keyValues, cancellationToken);
                await base.DeleteAsync(entity, autoSave, cancellationToken);
            }
        }
    }
}