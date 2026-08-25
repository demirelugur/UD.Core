namespace UD.Core.Helper.Databases
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
        Task<IEnumerable<T>> Query<T>(string commandText, object parameters, int? commandTimeout = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default);
        Task<IEnumerable<dynamic>> QueryDynamic(string commandText, object parameters, int? commandTimeout = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default);
        Task<GridReader> QueryMultiple(string commandText, object parameters, int? commandTimeout = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default);
        Task<int> Execute(string commandText, object parameters, int? commandTimeout = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default);
        Task<DbDataReader> ExecuteReader(string commandText, object parameters, int? commandTimeout = null, CommandType commandType = CommandType.Text, CommandBehavior commandBehavior = CommandBehavior.Default, CancellationToken cancellationToken = default);
        Task<T> ExecuteScalar<T>(string commandText, object parameters, int? commandTimeout = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default);
    }
    public sealed class DapperExecutor : IDapperExecutor, IDisposable, IAsyncDisposable
    {
        private bool _disposed = false;
        private readonly bool _ownsConnection;
        public void Dispose()
        {
            if (this._disposed) { return; }
            if (this._ownsConnection)
            {
                if (this.Connection.State != ConnectionState.Closed) { this.Connection.Close(); }
                this.Connection.Dispose();
            }
            this._disposed = true;
        }
        public async ValueTask DisposeAsync()
        {
            if (this._disposed) { return; }
            if (this._ownsConnection)
            {
                await this.EnsureConnectionClose();
                await this.Connection.DisposeAsync();
            }
            this._disposed = true;
        }
        [ActivatorUtilitiesConstructor]
        public DapperExecutor(DbContext dbContext) : this(dbContext.Database.GetDbConnection(), dbContext.Database.CurrentTransaction?.GetDbTransaction())
        {
            this._ownsConnection = false;
        }
        public DapperExecutor(DbConnection Connection, IDbTransaction? Transaction)
        {
            this.Connection = Connection;
            this.Transaction = Transaction;
            this._ownsConnection = true;
        }
        public DbConnection Connection { get; }
        public IDbTransaction? Transaction { get; set; }
        private Task EnsureConnectionOpen(CancellationToken cancellationToken)
        {
            if (this.Connection.State != ConnectionState.Open) { return this.Connection.OpenAsync(cancellationToken); }
            return Task.CompletedTask;
        }
        private Task EnsureConnectionClose()
        {
            if (this.Connection.State != ConnectionState.Closed) { return this.Connection.CloseAsync(); }
            return Task.CompletedTask;
        }
        public async Task<IEnumerable<T>> Query<T>(string commandText, object parameters, int? commandTimeout = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            await this.EnsureConnectionOpen(cancellationToken);
            return await this.Connection.QueryAsync<T>(new(commandText, parameters, this.Transaction, commandTimeout, commandType, CommandFlags.Buffered, cancellationToken));
        }
        public async Task<IEnumerable<dynamic>> QueryDynamic(string commandText, object parameters, int? commandTimeout = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            await this.EnsureConnectionOpen(cancellationToken);
            return await this.Connection.QueryAsync(new(commandText, parameters, this.Transaction, commandTimeout, commandType, CommandFlags.Buffered, cancellationToken));
        }
        public async Task<GridReader> QueryMultiple(string commandText, object parameters, int? commandTimeout = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            await this.EnsureConnectionOpen(cancellationToken);
            return await this.Connection.QueryMultipleAsync(new(commandText, parameters, this.Transaction, commandTimeout, commandType, CommandFlags.Buffered, cancellationToken));
        }
        public async Task<int> Execute(string commandText, object parameters, int? commandTimeout = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            await this.EnsureConnectionOpen(cancellationToken);
            return await this.Connection.ExecuteAsync(new(commandText, parameters, this.Transaction, commandTimeout, commandType, CommandFlags.Buffered, cancellationToken));
        }
        public async Task<DbDataReader> ExecuteReader(string commandText, object parameters, int? commandTimeout = null, CommandType commandType = CommandType.Text, CommandBehavior commandbehavior = CommandBehavior.Default, CancellationToken cancellationToken = default)
        {
            await this.EnsureConnectionOpen(cancellationToken);
            return await this.Connection.ExecuteReaderAsync(new(commandText, parameters, this.Transaction, commandTimeout, commandType, CommandFlags.Buffered, cancellationToken), commandbehavior);
        }
        public async Task<T> ExecuteScalar<T>(string commandText, object parameters, int? commandTimeout = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            await this.EnsureConnectionOpen(cancellationToken);
            return await this.Connection.ExecuteScalarAsync<T>(new(commandText, parameters, this.Transaction, commandTimeout, commandType, CommandFlags.Buffered, cancellationToken));
        }
    }
}