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
    using UD.Core.Helper.Database;
    using UD.Core.Helper.Validation;
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
            var entry = dbcontext.Entry(entity);
            var ie = typeof(T).GetProperties().Where(x => x.IsMapped() && entry.Property(x.Name).IsModified).ToArray();
            var columns = (expressions ?? []).Select(x => x.GetExpressionName()).ToArray();
            if (columns.Length == 0) { return ie.Length > 0; }
            return ie.Any(x => columns.Contains(x.Name));
        }
        /// <summary>
        /// Belirli bir bileşik anahtar(composite key) özelliği ile eski varlığın güncellenmesini sağlar.
        /// </summary>
        public static async Task<T> SetCompositeKeyAsync<T, CompositeKey>(this DbContext dbcontext, bool autosave, T oldentity, Expression<Func<T, CompositeKey>> compositekey, CompositeKey compositekeynewvalue, string dil, CancellationToken cancellationtoken = default) where T : class, new()
        {
            var t = typeof(T);
            var tablename = t.GetTableName(false);
            var c = compositekey.GetExpressionName();
            var props = t.GetProperties().Where(x => x.IsMapped()).Select(x => new
            {
                name = x.Name,
                setcolumn = x.Name == c,
                iscompositekey = x.IsPK() && x.GetDatabaseGeneratedOption() == DatabaseGeneratedOption.None
            }).ToArray();
            Guard.UnSupportLanguage(dil, nameof(dil));
            if (props.Count(x => x.iscompositekey) < 2)
            {
                if (dil == "en") { throw new KeyNotFoundException($"The \"{tablename}\" table must contain at least 2 properties with \"{typeof(KeyAttribute).FullName}\" and \"{typeof(DatabaseGeneratedAttribute).FullName}\" attributes to continue processing!"); }
                throw new KeyNotFoundException($"İşleme devam edebilmek için \"{tablename}\" tablosunda en az 2 özelliğin \"{typeof(KeyAttribute).FullName}\" ve \"{typeof(DatabaseGeneratedAttribute).FullName}\" içermesi gerekmektedir!");
            }
            if (props.Any(x => x.setcolumn && x.iscompositekey))
            {
                var newentity = new T();
                var entry = dbcontext.Entry(oldentity);
                var dbset = dbcontext.Set<T>();
                dbset.Attach(oldentity);
                foreach (var item in props.Select(x => new
                {
                    x.name,
                    x.setcolumn
                }).ToArray()) { _other.SetPropertyValue(newentity, item.name, (item.setcolumn ? compositekeynewvalue : entry.Property(item.name).OriginalValue), dil); }
                await dbset.AddAsync(newentity, cancellationtoken);
                dbset.Remove(oldentity);
                if (autosave) { await dbcontext.SaveChangesAsync(cancellationtoken); }
                return newentity;
            }
            if (dil == "en") { throw new Exception($"The property \"{c}\" in table \"{tablename}\" must have either \"{typeof(KeyAttribute).FullName}\" and \"{typeof(DatabaseGeneratedAttribute).FullName}\" specified!"); }
            throw new Exception($"\"{tablename}\" tablosundaki \"{c}\" özelliğinde \"{typeof(KeyAttribute).FullName}\" ve \"{typeof(DatabaseGeneratedAttribute).FullName}\" belirtilmelidir!");
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
            var sb = new StringBuilder();
            var index = 0;
            mappedtables = mappedtables ?? [];
            foreach (var type in mappedtables.Where(x => x.IsMappedTable()).ToArray())
            {
                var pkinfo = getprimarykeyinfo(type);
                if (pkinfo.columnname == "" || pkinfo.sqldbtypename == "") { continue; }
                var tablename = type.GetTableName(true);
                var variablename = $"@MAXID_{index}";
                sb.AppendLine($"DECLARE {variablename} {pkinfo.sqldbtypename}");
                sb.AppendLine($"SELECT {variablename} = MAX([{pkinfo.columnname}]) FROM {tablename}");
                sb.AppendLine($"SET {variablename} = ISNULL({variablename}, 0)");
                sb.AppendLine($"DBCC CHECKIDENT ('{tablename}', RESEED, {variablename})");
                index++;
            }
            if (sb.Length == 0) { return Task.FromResult(0); }
            return dbcontext.Database.ExecuteSqlRawAsync(sb.ToString(), Array.Empty<SqlParameter>(), cancellationtoken);
        }
        private static (string columnname, string sqldbtypename) getprimarykeyinfo(Type mappedtabletype)
        {
            if (_try.TryTableisKeyAttribute(mappedtabletype, out PropertyInfo[] _pis) && _pis.Length == 1 && _pis[0].IsPK() && _pis[0].GetDatabaseGeneratedOption() == DatabaseGeneratedOption.Identity)
            {
                var propertytype = _pis[0].PropertyType;
                if (propertytype.IsEnum) { propertytype = Enum.GetUnderlyingType(propertytype); }
                if (propertytype == typeof(byte)) { return (_pis[0].GetColumnName(), "TINYINT"); }
                if (propertytype == typeof(short)) { return (_pis[0].GetColumnName(), "SMALLINT"); }
                if (propertytype == typeof(int)) { return (_pis[0].GetColumnName(), "INT"); }
                if (propertytype == typeof(long)) { return (_pis[0].GetColumnName(), "BIGINT"); }
            }
            return ("", "");
        }
    }
}