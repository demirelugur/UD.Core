namespace UD.Core.Helper.Services
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using UD.Core.Extensions;
    using UD.Core.Helper.Configuration;
    using UD.Core.Helper.Paging;
    public interface IBaseServiceComplexKey<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto, TInsertDto, TUpdateDto> : IBaseService<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto>
    where TContext : DbContext
    where TEntity : class, IEntity
    where TEntityDto : IEntityDto
    where TEntityListDto : IEntityDto
    where TSearchDto : ISearchAndPaginateDto
    where TInsertDto : IEntityDto
    where TUpdateDto : IEntityDto
    {
        Task<TEntityDto?> GetById(object[] keyValues, CancellationToken cancellationToken = default);
        Task Insert(TInsertDto insertDto, bool autoSave = false, CancellationToken cancellationToken = default);
        Task Update(object[] keyValues, TUpdateDto updateDto, bool autoSave = false, CancellationToken cancellationToken = default);
        Task DeleteById(object[] keyValues, bool autoSave = false, CancellationToken cancellationToken = default);
    }
    public abstract class BaseServiceComplexKey<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto, TInsertDto, TUpdateDto> : BaseService<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto>, IBaseServiceComplexKey<TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto, TInsertDto, TUpdateDto>
    where TContext : DbContext
    where TEntity : class, IEntity
    where TEntityDto : IEntityDto
    where TEntityListDto : IEntityDto
    where TSearchDto : ISearchAndPaginateDto
    where TInsertDto : IEntityDto
    where TUpdateDto : IEntityDto
    {
        protected BaseServiceComplexKey(TContext Context, IMapper Mapper) : base(Context, Mapper) { }
        public virtual async Task<TEntityDto?> GetById(object[] keyValues, CancellationToken cancellationToken = default)
        {
            if (keyValues.IsNullOrCountZero()) { return default; }
            var entity = await this.DbSet.FindAsync(keyValues, cancellationToken);
            return entity == null ? default : this.Mapper.Map<TEntityDto>(entity);
        }
        public virtual async Task Insert(TInsertDto insertDto, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(insertDto);
            var entity = this.Mapper.Map<TEntity>(insertDto);
            await this.DbSet.AddAsync(entity, cancellationToken);
            if (autoSave) { await this.Context.SaveChangesAsync(cancellationToken); }
        }
        public virtual async Task Update(object[] keyValues, TUpdateDto updateDto, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            if (!keyValues.IsNullOrCountZero())
            {
                ArgumentNullException.ThrowIfNull(updateDto);
                var entity = await this.DbSet.FindAsync(keyValues, cancellationToken);
                if (entity != null)
                {
                    this.Mapper.Map(updateDto, entity);
                    if (autoSave) { await this.Context.SaveChangesAsync(cancellationToken); }
                }
            }
        }
        public virtual async Task DeleteById(object[] keyValues, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            if (!keyValues.IsNullOrCountZero())
            {
                var entity = await this.DbSet.FindAsync(keyValues, cancellationToken);
                await base.Delete(entity, autoSave, cancellationToken);
            }
        }
    }
}