namespace UD.Core.Helper
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
        DbConnection con { get; }
        IDbTransaction? dbtransaction { get; set; }
        Task EnsureConnectionOpenAsync(CancellationToken cancellationtoken = default);
        Task EnsureConnectionCloseAsync();
        Task<IEnumerable<T>> QueryAsync<T>(string commandtext, object parameters, int? commandtimeout, CommandType commandtype, CancellationToken cancellationtoken = default);
        Task<IEnumerable<dynamic>> QueryDynamicAsync(string commandtext, object parameters, int? commandtimeout, CommandType commandtype, CancellationToken cancellationtoken = default);
        Task<GridReader> QueryMultipleAsync(string commandtext, object parameters, int? commandtimeout, CommandType commandtype, CancellationToken cancellationtoken = default);
        Task<int> ExecuteAsync(string commandtext, object parameters, int? commandtimeout, CommandType commandtype, CancellationToken cancellationtoken = default);
        Task<DbDataReader> ExecuteReaderAsync(string commandtext, object parameters, int? commandtimeout, CommandType commandtype, CommandBehavior commandBehavior, CancellationToken cancellationtoken = default);
        Task<T> ExecuteScalarAsync<T>(string commandtext, object parameters, int? commandtimeout, CommandType commandtype, CancellationToken cancellationtoken = default);
    }
    public sealed class DapperHelper : IDapperHelper
    {
        private bool disposed = false;
        public void Dispose()
        {
            if (!this.disposed)
            {
                if (this.con.State != ConnectionState.Closed) { this.con.Close(); }
                this.con.Dispose();
                this.disposed = true;
            }
        }
        public async ValueTask DisposeAsync()
        {
            if (!this.disposed)
            {
                await this.EnsureConnectionCloseAsync();
                await this.con.DisposeAsync();
                this.disposed = true;
            }
        }
        public DapperHelper(DbContext dbcontext) : this(dbcontext.Database.GetDbConnection(), dbcontext.Database.CurrentTransaction?.GetDbTransaction()) { }
        public DapperHelper(DbConnection con, IDbTransaction? dbtransaction)
        {
            this.con = con;
            this.dbtransaction = dbtransaction;
        }
        public DbConnection con { get; }
        public IDbTransaction? dbtransaction { get; set; }
        public async Task EnsureConnectionOpenAsync(CancellationToken cancellationtoken = default)
        {
            if (this.con.State != ConnectionState.Open)
            {
                await this.con.OpenAsync(cancellationtoken);
            }
        }
        public async Task EnsureConnectionCloseAsync()
        {
            if (this.con.State != ConnectionState.Closed)
            {
                await this.con.CloseAsync();
            }
        }
        public async Task<IEnumerable<T>> QueryAsync<T>(string commandtext, object parameters, int? commandtimeout, CommandType commandtype, CancellationToken cancellationtoken = default)
        {
            await this.EnsureConnectionOpenAsync(cancellationtoken);
            return await this.con.QueryAsync<T>(new(commandtext, parameters, this.dbtransaction, commandtimeout, commandtype, CommandFlags.Buffered, cancellationtoken));
        }
        public async Task<IEnumerable<dynamic>> QueryDynamicAsync(string commandtext, object parameters, int? commandtimeout, CommandType commandtype, CancellationToken cancellationtoken = default)
        {
            await this.EnsureConnectionOpenAsync(cancellationtoken);
            return await this.con.QueryAsync(new(commandtext, parameters, this.dbtransaction, commandtimeout, commandtype, CommandFlags.Buffered, cancellationtoken));
        }
        public async Task<GridReader> QueryMultipleAsync(string commandtext, object parameters, int? commandtimeout, CommandType commandtype, CancellationToken cancellationtoken = default)
        {
            await this.EnsureConnectionOpenAsync(cancellationtoken);
            return await this.con.QueryMultipleAsync(new(commandtext, parameters, this.dbtransaction, commandtimeout, commandtype, CommandFlags.Buffered, cancellationtoken));
        }
        public async Task<int> ExecuteAsync(string commandtext, object parameters, int? commandtimeout, CommandType commandtype, CancellationToken cancellationtoken = default)
        {
            await this.EnsureConnectionOpenAsync(cancellationtoken);
            return await this.con.ExecuteAsync(new(commandtext, parameters, this.dbtransaction, commandtimeout, commandtype, CommandFlags.Buffered, cancellationtoken));
        }
        public async Task<DbDataReader> ExecuteReaderAsync(string commandtext, object parameters, int? commandtimeout, CommandType commandtype, CommandBehavior commandbehavior, CancellationToken cancellationtoken = default)
        {
            await this.EnsureConnectionOpenAsync(cancellationtoken);
            return await this.con.ExecuteReaderAsync(new(commandtext, parameters, this.dbtransaction, commandtimeout, commandtype, CommandFlags.Buffered, cancellationtoken), commandbehavior);
        }
        public async Task<T> ExecuteScalarAsync<T>(string commandtext, object parameters, int? commandtimeout, CommandType commandtype, CancellationToken cancellationtoken = default)
        {
            await this.EnsureConnectionOpenAsync(cancellationtoken);
            return await this.con.ExecuteScalarAsync<T>(new(commandtext, parameters, this.dbtransaction, commandtimeout, commandtype, CommandFlags.Buffered, cancellationtoken));
        }
    }
}