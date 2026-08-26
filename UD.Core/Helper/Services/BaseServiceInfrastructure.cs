namespace UD.Core.Helper.Services
{
    using Microsoft.Data.SqlClient;
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
        DbConnection GetConnection();
        IQueryable<T> SqlQueryRaw<T>(string sql, object parameters);
        Task<int> ExecuteSqlRawAsync(string sql, object parameters, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<List<dynamic>> QueryRawDynamicAsync(string query, object parameters, CommandBehavior commandBehavior = CommandBehavior.Default, int? commandTimeout = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default);
    }
    public abstract class BaseServiceInfrastructure<TContext, TEntity> : IBaseServiceInfrastructure<TContext, TEntity>
        where TContext : DbContext
        where TEntity : class, IBaseEntity
    {
        protected BaseServiceInfrastructure(TContext Context)
        {
            this.Context = Context ?? throw new ArgumentNullException(nameof(Context));
        }
        public TContext Context { get; }
        public DbSet<TEntity> DbSet => this.Context.Set<TEntity>();
        public DbConnection GetConnection() => this.Context.Database.GetDbConnection();
        public IQueryable<T> SqlQueryRaw<T>(string sql, object parameters) => this.Context.Database.SqlQueryRaw<T>(sql, ToSqlParameters(parameters));
        public Task<int> ExecuteSqlRawAsync(string sql, object parameters, CancellationToken cancellationToken = default) => this.Context.Database.ExecuteSqlRawAsync(sql, ToSqlParameters(parameters), cancellationToken);
        public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => this.Context.SaveChangesAsync(cancellationToken);
        public async Task<List<dynamic>> QueryRawDynamicAsync(string query, object parameters, CommandBehavior commandBehavior = CommandBehavior.Default, int? commandTimeout = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            var connection = this.GetConnection();
            var shouldCloseConnection = connection.State != ConnectionState.Open;
            if (shouldCloseConnection) { await connection.OpenAsync(cancellationToken); }
            try
            {
                var result = new List<dynamic>();
                await using var command = connection.CreateCommand();
                command.CommandText = query;
                command.CommandType = commandType;
                if (commandTimeout.HasValue) { command.CommandTimeout = commandTimeout.Value; }
                var transaction = this.Context.Database.CurrentTransaction; // Not: using eklenmemelidir!
                if (transaction != null) { command.Transaction = transaction.GetDbTransaction(); }
                var dbParameters = ToDbParameters(parameters, command);
                if (dbParameters.Length > 0) { command.Parameters.AddRange(dbParameters); }
                int i, fieldCount;
                IDictionary<string, object> row;
                await using var reader = await command.ExecuteReaderAsync(commandBehavior, cancellationToken);
                while (await reader.ReadAsync(cancellationToken))
                {
                    row = new ExpandoObject();
                    fieldCount = reader.FieldCount;
                    for (i = 0; i < fieldCount; i++) { row.Add(reader.GetName(i), await reader.IsDBNullAsync(i, cancellationToken) ? null : reader.GetValue(i)); }
                    result.Add(row);
                }
                return result;
            }
            finally { if (shouldCloseConnection) { await connection.CloseAsync(); } }
        }
        private static SqlParameter[] ToSqlParameters(object value)
        {
            if (value == null) { return []; }
            if (value is SqlParameter parameter) { return [parameter]; }
            if (value is IEnumerable<SqlParameter> parameters) { return parameters.ToArray(); }
            return Converters.ToDictionaryFromObject(value).Select(x => new SqlParameter(x.Key, x.Value ?? DBNull.Value)).ToArray();
        }
        private static DbParameter[] ToDbParameters(object value, DbCommand command)
        {
            if (value == null) { return []; }
            if (value is DbParameter parameter) { return [parameter]; }
            if (value is IEnumerable<DbParameter> parameters) { return parameters.ToArray(); }
            return Converters.ToDictionaryFromObject(value).Select(x =>
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = x.Key;
                parameter.Value = x.Value ?? DBNull.Value;
                return parameter;
            }).ToArray();
        }
    }
}