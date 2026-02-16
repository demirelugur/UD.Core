namespace UD.Core.Base
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using UD.Core.Extensions;
    using UD.Core.Helper.Paging;
    public interface IBaseServiceComposite<TContext, TEntity, TReturnDto, TSearchDto, TInsertDto, TUpdateDto> : IBaseService<TContext, TEntity, TReturnDto, TSearchDto, TInsertDto, TUpdateDto>
    where TContext : DbContext
    where TEntity : class
    where TReturnDto : class
    where TSearchDto : ISearchAndPaginateDto
    where TInsertDto : class
    where TUpdateDto : class
    {
        Task<TReturnDto?> GetByIdAsync(object[] keyvalues, CancellationToken cancellationToken = default);
        Task InsertAsync(TInsertDto insertDto, bool autoSave = false, CancellationToken cancellationToken = default);
        Task UpdateAsync(object[] keyvalues, TUpdateDto updateDto, bool autoSave = false, CancellationToken cancellationToken = default);
        Task DeleteByIdAsync(object[] keyvalues, bool autoSave = false, CancellationToken cancellationToken = default);
    }
    public abstract class BaseServiceComposite<TContext, TEntity, TReturnDto, TSearchDto, TInsertDto, TUpdateDto> : BaseService<TContext, TEntity, TReturnDto, TSearchDto, TInsertDto, TUpdateDto>, IBaseServiceComposite<TContext, TEntity, TReturnDto, TSearchDto, TInsertDto, TUpdateDto>
    where TContext : DbContext
    where TEntity : class
    where TReturnDto : class
    where TSearchDto : ISearchAndPaginateDto
    where TInsertDto : class
    where TUpdateDto : class
    {
        protected BaseServiceComposite(TContext context, IMapper mapper) : base(context, mapper) { }
        public virtual async Task<TReturnDto?> GetByIdAsync(object[] keyvalues, CancellationToken cancellationToken = default)
        {
            if (keyvalues.IsNullOrCountZero()) { return null; }
            var entity = await this.DbSet.FindAsync(keyvalues, cancellationToken);
            return entity == null ? null : this.mapper.Map<TReturnDto>(entity);
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
                await this.DeleteAsync(entity, autoSave, cancellationToken);
            }
        }
    }
}