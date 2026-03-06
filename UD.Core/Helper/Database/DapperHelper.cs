namespace UD.Core.Helper.Database
{
    using Dapper;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using static Dapper.SqlMapper;
    public interface IDapperHelper : IDisposable, IAsyncDisposable
    {
        DbConnection connection { get; }
        IDbTransaction? dbTransaction { get; set; }
        Task EnsureConnectionOpenAsync(CancellationToken cancellationToken = default);
        Task EnsureConnectionCloseAsync();
        Task<IEnumerable<T>> QueryAsync<T>(string commandText, object parameters, int? commandTimeout, CommandType commandType, CancellationToken cancellationToken = default);
        Task<IEnumerable<dynamic>> QueryDynamicAsync(string commandText, object parameters, int? commandTimeout, CommandType commandType, CancellationToken cancellationToken = default);
        Task<GridReader> QueryMultipleAsync(string commandText, object parameters, int? commandTimeout, CommandType commandType, CancellationToken cancellationToken = default);
        Task<int> ExecuteAsync(string commandText, object parameters, int? commandTimeout, CommandType commandType, CancellationToken cancellationToken = default);
        Task<DbDataReader> ExecuteReaderAsync(string commandText, object parameters, int? commandTimeout, CommandType commandType, CommandBehavior commandBehavior, CancellationToken cancellationToken = default);
        Task<T> ExecuteScalarAsync<T>(string commandText, object parameters, int? commandTimeout, CommandType commandType, CancellationToken cancellationToken = default);
    }
    public sealed class DapperHelper : IDapperHelper
    {
        private bool disposed = false;
        public void Dispose()
        {
            if (!this.disposed)
            {
                if (this.connection.State != ConnectionState.Closed) { this.connection.Close(); }
                this.connection.Dispose();
                this.disposed = true;
            }
        }
        public async ValueTask DisposeAsync()
        {
            if (!this.disposed)
            {
                await this.EnsureConnectionCloseAsync();
                await this.connection.DisposeAsync();
                this.disposed = true;
            }
        }
        public DapperHelper(DbContext dbContext) : this(dbContext.Database.GetDbConnection(), dbContext.Database.CurrentTransaction?.GetDbTransaction()) { }
        public DapperHelper(DbConnection connection, IDbTransaction? dbTransaction)
        {
            this.connection = connection;
            this.dbTransaction = dbTransaction;
        }
        public DbConnection connection { get; }
        public IDbTransaction? dbTransaction { get; set; }
        public async Task EnsureConnectionOpenAsync(CancellationToken cancellationToken = default)
        {
            if (this.connection.State != ConnectionState.Open)
            {
                await this.connection.OpenAsync(cancellationToken);
            }
        }
        public async Task EnsureConnectionCloseAsync()
        {
            if (this.connection.State != ConnectionState.Closed)
            {
                await this.connection.CloseAsync();
            }
        }
        public async Task<IEnumerable<T>> QueryAsync<T>(string commandText, object parameters, int? commandTimeout, CommandType commandType, CancellationToken cancellationToken = default)
        {
            await this.EnsureConnectionOpenAsync(cancellationToken);
            return await this.connection.QueryAsync<T>(new(commandText, parameters, this.dbTransaction, commandTimeout, commandType, CommandFlags.Buffered, cancellationToken));
        }
        public async Task<IEnumerable<dynamic>> QueryDynamicAsync(string commandText, object parameters, int? commandTimeout, CommandType commandType, CancellationToken cancellationToken = default)
        {
            await this.EnsureConnectionOpenAsync(cancellationToken);
            return await this.connection.QueryAsync(new(commandText, parameters, this.dbTransaction, commandTimeout, commandType, CommandFlags.Buffered, cancellationToken));
        }
        public async Task<GridReader> QueryMultipleAsync(string commandText, object parameters, int? commandTimeout, CommandType commandType, CancellationToken cancellationToken = default)
        {
            await this.EnsureConnectionOpenAsync(cancellationToken);
            return await this.connection.QueryMultipleAsync(new(commandText, parameters, this.dbTransaction, commandTimeout, commandType, CommandFlags.Buffered, cancellationToken));
        }
        public async Task<int> ExecuteAsync(string commandText, object parameters, int? commandTimeout, CommandType commandType, CancellationToken cancellationToken = default)
        {
            await this.EnsureConnectionOpenAsync(cancellationToken);
            return await this.connection.ExecuteAsync(new(commandText, parameters, this.dbTransaction, commandTimeout, commandType, CommandFlags.Buffered, cancellationToken));
        }
        public async Task<DbDataReader> ExecuteReaderAsync(string commandText, object parameters, int? commandTimeout, CommandType commandType, CommandBehavior commandbehavior, CancellationToken cancellationToken = default)
        {
            await this.EnsureConnectionOpenAsync(cancellationToken);
            return await this.connection.ExecuteReaderAsync(new(commandText, parameters, this.dbTransaction, commandTimeout, commandType, CommandFlags.Buffered, cancellationToken), commandbehavior);
        }
        public async Task<T> ExecuteScalarAsync<T>(string commandText, object parameters, int? commandTimeout, CommandType commandType, CancellationToken cancellationToken = default)
        {
            await this.EnsureConnectionOpenAsync(cancellationToken);
            return await this.connection.ExecuteScalarAsync<T>(new(commandText, parameters, this.dbTransaction, commandTimeout, commandType, CommandFlags.Buffered, cancellationToken));
        }
    }
}