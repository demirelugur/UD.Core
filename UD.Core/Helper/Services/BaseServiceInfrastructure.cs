namespace UD.Core.Helper.Services
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using System.Data.Common;
    using UD.Core.Helper;
    using UD.Core.Helper.Configuration;
    public interface IBaseServiceInfrastructure<TContext, TEntity>
        where TContext : DbContext
        where TEntity : class, IBaseEntity
    {
        TContext Context { get; }
        DbSet<TEntity> DbSet { get; }
        DbConnection GetDbConnection();
        IQueryable<T> SqlQueryRaw<T>(string sql, object parameters);
        Task<int> ExecuteSqlRaw(string sql, object parameters, CancellationToken cancellationToken = default);
        Task<int> SaveChanges(CancellationToken cancellationToken = default);
    }
    public abstract class BaseServiceInfrastructure<TContext, TEntity> : IBaseServiceInfrastructure<TContext, TEntity>, IDisposable
        where TContext : DbContext
        where TEntity : class, IBaseEntity
    {
        protected readonly IMapper Mapper;
        protected BaseServiceInfrastructure(TContext Context, IMapper Mapper)
        {
            this.Context = Context ?? throw new ArgumentNullException(nameof(Context));
            this.Mapper = Mapper ?? throw new ArgumentNullException(nameof(Mapper));
        }
        public TContext Context { get; }
        public DbSet<TEntity> DbSet => this.Context.Set<TEntity>();
        public DbConnection GetDbConnection() => this.Context.Database.GetDbConnection();
        public IQueryable<T> SqlQueryRaw<T>(string sql, object parameters) => this.Context.Database.SqlQueryRaw<T>(sql, Converters.ToSqlParameterFromObject(parameters));
        public Task<int> ExecuteSqlRaw(string sql, object parameters, CancellationToken cancellationToken = default) => this.Context.Database.ExecuteSqlRawAsync(sql, Converters.ToSqlParameterFromObject(parameters), cancellationToken);
        public virtual Task<int> SaveChanges(CancellationToken cancellationToken = default) => this.Context.SaveChangesAsync(cancellationToken);
        public void Dispose()
        {
            this.Context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}