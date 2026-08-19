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
        DbConnection GetDbConnection();
        IQueryable<T> SqlQueryRaw<T>(string sql, object parameters);
        Task<int> ExecuteSqlRaw(string sql, object parameters, CancellationToken cancellationToken = default);
        Task<int> SaveChanges(CancellationToken cancellationToken = default);
        Task<List<dynamic>> QueryRawDynamic(string query, object parameters, CommandBehavior commandBehavior = CommandBehavior.Default, int? commandTimeout = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default);
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
        public IQueryable<T> SqlQueryRaw<T>(string sql, object parameters) => this.Context.Database.SqlQueryRaw<T>(sql, toDbParameterFromObject(parameters, null));
        public Task<int> ExecuteSqlRaw(string sql, object parameters, CancellationToken cancellationToken = default) => this.Context.Database.ExecuteSqlRawAsync(sql, toDbParameterFromObject(parameters, null), cancellationToken);
        public virtual Task<int> SaveChanges(CancellationToken cancellationToken = default) => this.Context.SaveChangesAsync(cancellationToken);
        public async Task<List<dynamic>> QueryRawDynamic(string query, object parameters, CommandBehavior commandBehavior = CommandBehavior.Default, int? commandTimeout = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
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
                var dbParameters = toDbParameterFromObject(parameters, command);
                if (dbParameters.Length > 0) { command.Parameters.AddRange(dbParameters); }
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
        private static IDataParameter[] toDbParameterFromObject(object obj, DbCommand? command)
        {
            if (obj == null) { return []; }
            if (obj is IDataParameter parameter) { return [parameter]; }
            if (obj is IEnumerable<IDataParameter> parameters) { return parameters.ToArray(); }
            var dic = Converters.ToDictionaryFromObject(obj);
            if (dic.Count == 0) { return []; }
            if (command == null) { return dic.Select(x => new SqlParameter(x.Key, x.Value ?? DBNull.Value)).ToArray(); }
            return dic.Select(x =>
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = x.Key;
                parameter.Value = x.Value ?? DBNull.Value;
                return parameter;
            }).ToArray();
        }
    }
}