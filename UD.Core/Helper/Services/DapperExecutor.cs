namespace UD.Core.Helper.Services
{
    using Dapper;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;
    using Microsoft.Extensions.DependencyInjection;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using static Dapper.SqlMapper;
    public interface IDapperExecutor
    {
        DbConnection Connection { get; }
        IDbTransaction? Transaction { get; set; }
        Task EnsureConnectionOpenAsync(CancellationToken cancellationToken);
        Task EnsureConnectionCloseAsync();
        Task<IEnumerable<T>> QueryAsync<T>(string commandText, object parameters, int? commandTimeout = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default);
        Task<IEnumerable<dynamic>> QueryDynamicAsync(string commandText, object parameters, int? commandTimeout = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default);
        Task<GridReader> QueryMultipleAsync(string commandText, object parameters, int? commandTimeout = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default);
        Task<int> ExecuteAsync(string commandText, object parameters, int? commandTimeout = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default);
        Task<DbDataReader> ExecuteReaderAsync(string commandText, object parameters, int? commandTimeout = null, CommandType commandType = CommandType.Text, CommandBehavior commandBehavior = CommandBehavior.Default, CancellationToken cancellationToken = default);
        Task<T> ExecuteScalarAsync<T>(string commandText, object parameters, int? commandTimeout = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default);
    }
    public sealed class DapperExecutor : IDapperExecutor, IDisposable, IAsyncDisposable
    {
        private bool _disposed = false;
        private readonly bool _ownsConnection;
        public void Dispose()
        {
            if (this._disposed) { return; }
            if (this.Connection.State != ConnectionState.Closed) { this.Connection.Close(); }
            if (this._ownsConnection) { this.Connection.Dispose(); }
            this._disposed = true;
        }
        public async ValueTask DisposeAsync()
        {
            if (this._disposed) { return; }
            await this.EnsureConnectionCloseAsync();
            if (this._ownsConnection) { await this.Connection.DisposeAsync(); }
            this._disposed = true;
        }
        [ActivatorUtilitiesConstructor]
        public DapperExecutor(DbContext context) : this(context.Database.GetDbConnection(), context.Database.CurrentTransaction?.GetDbTransaction())
        {
            this._ownsConnection = false;
        }
        public DapperExecutor(DbConnection connection, IDbTransaction? transaction)
        {
            this.Connection = connection;
            this.Transaction = transaction;
            this._ownsConnection = true;
        }
        public DbConnection Connection { get; }
        public IDbTransaction? Transaction { get; set; }
        public Task EnsureConnectionOpenAsync(CancellationToken cancellationToken)
        {
            if (this.Connection.State != ConnectionState.Open) { return this.Connection.OpenAsync(cancellationToken); }
            return Task.CompletedTask;
        }
        public Task EnsureConnectionCloseAsync()
        {
            if (this.Connection.State != ConnectionState.Closed) { return this.Connection.CloseAsync(); }
            return Task.CompletedTask;
        }
        public async Task<IEnumerable<T>> QueryAsync<T>(string commandText, object parameters, int? commandTimeout = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            await this.EnsureConnectionOpenAsync(cancellationToken);
            return await this.Connection.QueryAsync<T>(new(commandText, parameters, this.Transaction, commandTimeout, commandType, CommandFlags.Buffered, cancellationToken));
        }
        public async Task<IEnumerable<dynamic>> QueryDynamicAsync(string commandText, object parameters, int? commandTimeout = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            await this.EnsureConnectionOpenAsync(cancellationToken);
            return await this.Connection.QueryAsync(new(commandText, parameters, this.Transaction, commandTimeout, commandType, CommandFlags.Buffered, cancellationToken));
        }
        public async Task<GridReader> QueryMultipleAsync(string commandText, object parameters, int? commandTimeout = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            await this.EnsureConnectionOpenAsync(cancellationToken);
            return await this.Connection.QueryMultipleAsync(new(commandText, parameters, this.Transaction, commandTimeout, commandType, CommandFlags.Buffered, cancellationToken));
        }
        public async Task<int> ExecuteAsync(string commandText, object parameters, int? commandTimeout = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            await this.EnsureConnectionOpenAsync(cancellationToken);
            return await this.Connection.ExecuteAsync(new(commandText, parameters, this.Transaction, commandTimeout, commandType, CommandFlags.Buffered, cancellationToken));
        }
        public async Task<DbDataReader> ExecuteReaderAsync(string commandText, object parameters, int? commandTimeout = null, CommandType commandType = CommandType.Text, CommandBehavior commandbehavior = CommandBehavior.Default, CancellationToken cancellationToken = default)
        {
            await this.EnsureConnectionOpenAsync(cancellationToken);
            return await this.Connection.ExecuteReaderAsync(new(commandText, parameters, this.Transaction, commandTimeout, commandType, CommandFlags.Buffered, cancellationToken), commandbehavior);
        }
        public async Task<T> ExecuteScalarAsync<T>(string commandText, object parameters, int? commandTimeout = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            await this.EnsureConnectionOpenAsync(cancellationToken);
            return await this.Connection.ExecuteScalarAsync<T>(new(commandText, parameters, this.Transaction, commandTimeout, commandType, CommandFlags.Buffered, cancellationToken));
        }
    }
}