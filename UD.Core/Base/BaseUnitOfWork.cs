namespace UD.Core.Base
{
    using UD.Core.Extensions;
    using UD.Core.Helper;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Common;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using static UD.Core.Helper.OrtakTools;
    public interface IUnitOfWork<TContext> where TContext : DbContext
    {
        TContext Context { get; }
        IDapperHelper Dapper { get; }
        DbConnection GetDbConnection();
        Task<SqlServerProperties> ServerPropertyAsync(CancellationToken cancellationtoken = default);
        Task<int> ExecuteRawAsync(string query, object parameters, CancellationToken cancellationtoken = default);
        Task<int> TableReseedAsync(bool isdebug, CancellationToken cancellationtoken = default, params Type[] mappedtables);
    }
    public class BaseUnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
    {
        /*
        IDisposable
        #region Dispose
        private bool _disposed;
        ~BaseUnitOfWork()
        {
            this.Dispose(false);
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing) { this.Context.Dispose(); }
                _disposed = true;
            }
        }
        #endregion
        */
        public BaseUnitOfWork(TContext context)
        {
            this.Context = context;
        }
        public TContext Context { get; }
        public IDapperHelper Dapper => new DapperHelper(this.Context);
        public DbConnection GetDbConnection() => this.Context.Database.GetDbConnection();
        public Task<SqlServerProperties> ServerPropertyAsync(CancellationToken cancellationtoken = default) => this.Context.Database.SqlQueryRaw<SqlServerProperties>(SqlServerProperties.query()).FirstOrDefaultAsync(cancellationtoken);
        public Task<int> ExecuteRawAsync(string sql, object parameters, CancellationToken cancellationtoken = default) => this.Context.Database.ExecuteSqlRawAsync(sql, _to.ToSqlParameterFromObject(parameters), cancellationtoken);
        /// <summary>
        /// Identity (sıralı artan) sayısal PK değerine sahip tablolarda kimlik değerini resetlemek için kullanılan metot.
        /// <code>DBCC CHECKIDENT (&#39;[dbo].[LoremIpsum]&#39;, RESEED, 0)</code>
        /// </summary>
        public Task<int> TableReseedAsync(bool isdebug, CancellationToken cancellationtoken = default, params Type[] mappedtables)
        {
            mappedtables = mappedtables ?? Array.Empty<Type>();
            if (isdebug || mappedtables.Length == 0 || !mappedtables.All(x => x.IsMappedTable())) { return Task.FromResult(0); }
            var _sb = new StringBuilder();
            var _index = 0;
            foreach (var type in mappedtables)
            {
                var _pkinfo = this.getprimarykeyinfo(type);
                if (_pkinfo.columnname == "" || _pkinfo.sqldbtypename == "") { continue; }
                var _tablename = type.GetTableName(true);
                var _variablename = $"@MAXID_{_index}";
                _sb.AppendLine($"DECLARE {_variablename} {_pkinfo.sqldbtypename}");
                _sb.AppendLine($"SELECT {_variablename} = MAX([{_pkinfo.columnname}]) FROM {_tablename}");
                _sb.AppendLine($"SET {_variablename} = ISNULL({_variablename}, 0)");
                _sb.AppendLine($"DBCC CHECKIDENT ('{_tablename}', RESEED, {_variablename})");
                _index++;
            }
            if (_sb.Length == 0) { return Task.FromResult(0); }
            return this.ExecuteRawAsync(_sb.ToString(), null, cancellationtoken);
        }
        private (string columnname, string sqldbtypename) getprimarykeyinfo(Type mappedtabletype)
        {
            if (_try.TryTableisKeyAttribute(mappedtabletype, out PropertyInfo[] _pis) && _pis.Length == 1 && _pis[0].IsPK() && _pis[0].GetDatabaseGeneratedOption() == DatabaseGeneratedOption.Identity)
            {
                var _propertytype = _pis[0].PropertyType;
                if (_propertytype.IsEnum) { _propertytype = Enum.GetUnderlyingType(_propertytype); }
                if (_propertytype == typeof(byte)) { return (_pis[0].GetColumnName(), "TINYINT"); }
                if (_propertytype == typeof(short)) { return (_pis[0].GetColumnName(), "SMALLINT"); }
                if (_propertytype == typeof(int)) { return (_pis[0].GetColumnName(), "INT"); }
                if (_propertytype == typeof(long)) { return (_pis[0].GetColumnName(), "BIGINT"); }
            }
            return ("", "");
        }
    }
}