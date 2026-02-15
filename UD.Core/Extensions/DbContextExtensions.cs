namespace UD.Core.Extensions
{
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;
    using UD.Core.Helper;
    using static UD.Core.Helper.OrtakTools;
    public static class DbContextExtensions
    {
        /// <summary>
        /// Belirtilen varlığın (entity) bir veya daha fazla özelliğinin değiştirilip değiştirilmediğini kontrol eder.
        /// </summary>
        /// <typeparam name="T">Kontrol edilecek varlık türü.</typeparam>
        /// <param name="dbcontext">DbContext örneği.</param>
        /// <param name="entity">Değişiklik durumu kontrol edilecek varlık.</param>
        /// <param name="expressions">Kontrol edilecek özelliklerin ifadeleri.</param>
        /// <returns>Değiştirilmişse <see langword="true"/>, değilse <see langword="false"/> döner.</returns>
        public static bool IsModified<T>(this DbContext dbcontext, T entity, params Expression<Func<T, object>>[] expressions) where T : class
        {
            var _entry = dbcontext.Entry(entity);
            var _ie = typeof(T).GetProperties().Where(x => x.IsMapped() && _entry.Property(x.Name).IsModified).ToArray();
            var _columns = (expressions ?? Array.Empty<Expression<Func<T, object>>>()).Select(x => x.GetExpressionName()).ToArray();
            if (_columns.Length == 0) { return _ie.Length > 0; }
            return _ie.Any(x => _columns.Contains(x.Name));
        }
        /// <summary>
        /// Belirli bir bileşik anahtar(composite key) özelliği ile eski varlığın güncellenmesini sağlar.
        /// </summary>
        public static async Task<T> SetCompositeKeyAsync<T, CompositeKey>(this DbContext dbcontext, bool autosave, T oldentity, Expression<Func<T, CompositeKey>> compositekey, CompositeKey compositekeynewvalue, string dil, CancellationToken cancellationtoken = default) where T : class, new()
        {
            var _t = typeof(T);
            var _tablename = _t.GetTableName(false);
            var _c = compositekey.GetExpressionName();
            var _props = _t.GetProperties().Where(x => x.IsMapped()).Select(x => new
            {
                name = x.Name,
                setcolumn = x.Name == _c,
                iscompositekey = x.IsPK() && x.GetDatabaseGeneratedOption() == DatabaseGeneratedOption.None
            }).ToArray();
            Guard.UnSupportLanguage(dil, nameof(dil));
            if (_props.Count(x => x.iscompositekey) < 2)
            {
                if (dil == "en") { throw new KeyNotFoundException($"The \"{_tablename}\" table must contain at least 2 properties with \"{typeof(KeyAttribute).FullName}\" and \"{typeof(DatabaseGeneratedAttribute).FullName}\" attributes to continue processing!"); }
                throw new KeyNotFoundException($"İşleme devam edebilmek için \"{_tablename}\" tablosunda en az 2 özelliğin \"{typeof(KeyAttribute).FullName}\" ve \"{typeof(DatabaseGeneratedAttribute).FullName}\" içermesi gerekmektedir!");
            }
            if (_props.Any(x => x.setcolumn && x.iscompositekey))
            {
                var _newentity = new T();
                var _entry = dbcontext.Entry(oldentity);
                var _dbset = dbcontext.Set<T>();
                _dbset.Attach(oldentity);
                foreach (var item in _props.Select(x => new
                {
                    x.name,
                    x.setcolumn
                }).ToArray()) { _other.SetPropertyValue(_newentity, item.name, (item.setcolumn ? compositekeynewvalue : _entry.Property(item.name).OriginalValue), dil); }
                await _dbset.AddAsync(_newentity, cancellationtoken);
                _dbset.Remove(oldentity);
                if (autosave) { await dbcontext.SaveChangesAsync(cancellationtoken); }
                return _newentity;
            }
            if (dil == "en") { throw new Exception($"The property \"{_c}\" in table \"{_tablename}\" must have either \"{typeof(KeyAttribute).FullName}\" and \"{typeof(DatabaseGeneratedAttribute).FullName}\" specified!"); }
            throw new Exception($"\"{_tablename}\" tablosundaki \"{_c}\" özelliğinde \"{typeof(KeyAttribute).FullName}\" ve \"{typeof(DatabaseGeneratedAttribute).FullName}\" belirtilmelidir!");
        }
        /// <summary> Bağlı bulunulan <see cref="DbContext"/> üzerinden SQL Server sunucusuna ait sistem özelliklerini asenkron olarak sorgular ve <see cref="SqlServerProperties"/> nesnesi olarak döndürür. </summary>
        /// <param name="dbcontext"> Sorgunun çalıştırılacağı veritabanı bağlamı.</param>
        /// <param name="cancellationtoken"> İşlemi iptal etmek için kullanılabilecek isteğe bağlı <see cref="CancellationToken"/>.</param>
        public static Task<SqlServerProperties> GetServerPropertyAsync(this DbContext dbcontext, CancellationToken cancellationtoken = default) => dbcontext.Database.SqlQueryRaw<SqlServerProperties>(SqlServerProperties.query()).FirstOrDefaultAsync(cancellationtoken);
        /// <summary>
        /// Belirtilen entity türlerine karşılık gelen tablolar için, Identity özelliğine sahip birincil anahtar alanlarının mevcut maksimum değerini baz alarak yeniden numaralandırma (RESEED) işlemini asenkron olarak gerçekleştirir. Metot, her tablo için ilgili birincil anahtar kolonunun mevcut en büyük değerini (MAX) hesaplar ve <c>DBCC CHECKIDENT</c> komutu ile Identity değerini bu değere eşitler. Böylece manuel veri ekleme, toplu veri taşıma veya seed işlemleri sonrası oluşabilecek kimlik (Identity) kaymalarının önüne geçilmiş olur.
        /// <br />
        /// <br />
        /// Sadece
        /// <list type="number">
        /// <item><description>Tek kolonlu birincil anahtara sahip</description></item>
        /// <item><description><see cref="DatabaseGeneratedOption.Identity"/> olarak işaretlenmiş</description></item>
        /// <item><description>Veri tipi TINYINT, SMALLINT, INT veya BIGINT olan</description></item>
        /// </list>
        /// tablolar için işlem uygulanır. <paramref name="isdebug"/> parametresi <see langword="true"/> olduğunda herhangi bir SQL komutu çalıştırılmaz ve metot 0 döner. İşlem uygulanacak tablo bulunamazsa yine 0 döndürülür.
        /// </summary>
        /// <param name="dbcontext"> SQL komutunun çalıştırılacağı <see cref="DbContext"/> örneği.</param>
        /// <param name="isdebug"> Debug modunu belirtir. <see langword="true"/> ise reseed işlemi yapılmaz.</param>
        /// <param name="mappedtables"> Reseed işlemi uygulanacak entity türleri. </param>
        /// <param name="cancellationtoken"> İşlemi iptal etmek için kullanılabilecek isteğe bağlı <see cref="CancellationToken"/>.</param>
        /// <returns>Çalıştırılan SQL komutundan etkilenen satır sayısını temsil eden <see cref="Task{Int32}"/>. </returns>
        public static Task<int> TableReseedAsync(this DbContext dbcontext, bool isdebug, Type[] mappedtables, CancellationToken cancellationtoken = default)
        {
            if (isdebug) { return Task.FromResult(0); }
            var _sb = new StringBuilder();
            var _index = 0;
            mappedtables = mappedtables ?? Array.Empty<Type>();
            foreach (var type in mappedtables.Where(x => x.IsMappedTable()).ToArray())
            {
                var _pkinfo = getprimarykeyinfo(type);
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
            return dbcontext.Database.ExecuteSqlRawAsync(_sb.ToString(), Array.Empty<SqlParameter>(), cancellationtoken);
        }
        private static (string columnname, string sqldbtypename) getprimarykeyinfo(Type mappedtabletype)
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