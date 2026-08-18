namespace UD.Core.Helper.Services
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;
    using System.Data;
    using System.Data.Common;
    using System.Dynamic;
    using UD.Core.Helper;
    using UD.Core.Helper.Configurations;
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
        Task<List<dynamic>> QueryRawDynamic(string query, CommandType commandType, CommandBehavior commandBehavior, int? commandTimeout, object parameters, CancellationToken cancellationToken = default);
    }
    public abstract class BaseServiceInfrastructure<TContext, TEntity> : IBaseServiceInfrastructure<TContext, TEntity>, IDisposable
        where TContext : DbContext
        where TEntity : class, IBaseEntity
    {
        protected BaseServiceInfrastructure(TContext Context)
        {
            this.Context = Context ?? throw new ArgumentNullException(nameof(Context));
        }
        public TContext Context { get; }
        public DbSet<TEntity> DbSet => this.Context.Set<TEntity>();
        public DbConnection GetDbConnection() => this.Context.Database.GetDbConnection();
        public IQueryable<T> SqlQueryRaw<T>(string sql, object parameters) => this.Context.Database.SqlQueryRaw<T>(sql, Converters.ToSqlParameterFromObject(parameters));
        public Task<int> ExecuteSqlRaw(string sql, object parameters, CancellationToken cancellationToken = default) => this.Context.Database.ExecuteSqlRawAsync(sql, Converters.ToSqlParameterFromObject(parameters), cancellationToken);
        public virtual Task<int> SaveChanges(CancellationToken cancellationToken = default) => this.Context.SaveChangesAsync(cancellationToken);
        public async Task<List<dynamic>> QueryRawDynamic(string query, CommandType commandType, CommandBehavior commandBehavior, int? commandTimeout, object parameters, CancellationToken cancellationToken = default)
        {
            var connection = this.GetDbConnection();
            try
            {
                var result = new List<dynamic>();
                if (connection.State != ConnectionState.Open) { await connection.OpenAsync(cancellationToken); }
                await using var command = connection.CreateCommand();
                command.CommandText = query;
                command.CommandType = commandType;
                if (commandTimeout.HasValue) { command.CommandTimeout = commandTimeout.Value; }
                var transaction = this.Context.Database.CurrentTransaction; // Not: using eklenmemelidir!
                if (transaction != null) { command.Transaction = transaction.GetDbTransaction(); }
                var sqlParameters = Converters.ToSqlParameterFromObject(parameters);
                if (sqlParameters.Length > 0) { command.Parameters.AddRange(sqlParameters); }
                int i, fieldCount;
                string columnName;
                IDictionary<string, object> row;
                await using var reader = await command.ExecuteReaderAsync(commandBehavior, cancellationToken);
                while (await reader.ReadAsync(cancellationToken))
                {
                    row = new ExpandoObject();
                    fieldCount = reader.FieldCount;
                    for (i = 0; i < fieldCount; i++)
                    {
                        columnName = reader.GetName(i);
                        row.Add(columnName, await reader.IsDBNullAsync(i, cancellationToken) ? null : reader.GetValue(i));
                    }
                    result.Add(row);
                }
                return result;
            }
            finally { if (connection.State != ConnectionState.Closed) { await connection.CloseAsync(); } }
        }
        public void Dispose()
        {
            this.Context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}