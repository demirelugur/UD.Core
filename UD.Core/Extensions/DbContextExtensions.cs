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
        /// <param name="context">DbContext örneği.</param>
        /// <param name="entity">Değişiklik durumu kontrol edilecek varlık.</param>
        /// <param name="expressions">Kontrol edilecek özelliklerin ifadeleri.</param>
        /// <returns>Değiştirilmişse <see langword="true"/>, değilse <see langword="false"/> döner.</returns>
        public static bool IsModified<T>(this DbContext context, T entity, params Expression<Func<T, object>>[] expressions) where T : class
        {
            var entry = context.Entry(entity);
            var properties = typeof(T).GetProperties().Where(x => x.IsMapped() && entry.Property(x.Name).IsModified).ToArray();
            var columns = (expressions ?? []).Select(x => x.GetExpressionName()).ToArray();
            if (columns.Length == 0) { return properties.Length > 0; }
            return properties.Any(x => columns.Contains(x.Name));
        }
        /// <summary>
        /// Belirli bir bileşik anahtar(composite key) özelliği ile eski varlığın güncellenmesini sağlar.
        /// </summary>
        public static async Task<T> SetCompositeKey<T, CompositeKey>(this DbContext context, bool autoSave, T oldEntity, Expression<Func<T, CompositeKey>> compositeKey, CompositeKey compositeKeyValue, string dil, CancellationToken cancellationToken = default) where T : class, new()
        {
            var type = typeof(T);
            var tableName = type.GetTableName(false);
            var compositeKeyName = compositeKey.GetExpressionName();
            var properties = type.GetProperties().Where(x => x.IsMapped()).Select(x => new
            {
                name = x.Name,
                isSetCompositeKeyName = x.Name == compositeKeyName,
                isCompositeKey = x.IsPK() && x.GetDatabaseGeneratedOption() == DatabaseGeneratedOption.None
            }).ToArray();
            Guard.UnSupportLanguage(dil, nameof(dil));
            if (properties.Count(x => x.isCompositeKey) < 2)
            {
                if (dil == "en") { throw new KeyNotFoundException($"The \"{tableName}\" table must contain at least 2 properties with \"{typeof(KeyAttribute).FullName}\" and \"{typeof(DatabaseGeneratedAttribute).FullName}\" attributes to continue processing!"); }
                throw new KeyNotFoundException($"İşleme devam edebilmek için \"{tableName}\" tablosunda en az 2 özelliğin \"{typeof(KeyAttribute).FullName}\" ve \"{typeof(DatabaseGeneratedAttribute).FullName}\" içermesi gerekmektedir!");
            }
            if (properties.Any(x => x.isSetCompositeKeyName && x.isCompositeKey))
            {
                var newEntity = new T();
                var entry = context.Entry(oldEntity);
                var dbSet = context.Set<T>();
                dbSet.Attach(oldEntity);
                foreach (var item in properties.Select(x => new
                {
                    x.name,
                    x.isSetCompositeKeyName
                }).ToArray()) { Utilities.SetPropertyValue(newEntity, item.name, (item.isSetCompositeKeyName ? compositeKeyValue : entry.Property(item.name).OriginalValue), dil); }
                await dbSet.AddAsync(newEntity, cancellationToken);
                dbSet.Remove(oldEntity);
                if (autoSave) { await context.SaveChangesAsync(cancellationToken); }
                return newEntity;
            }
            if (dil == "en") { throw new Exception($"The property \"{compositeKeyName}\" in table \"{tableName}\" must have either \"{typeof(KeyAttribute).FullName}\" and \"{typeof(DatabaseGeneratedAttribute).FullName}\" specified!"); }
            throw new Exception($"\"{tableName}\" tablosundaki \"{compositeKeyName}\" özelliğinde \"{typeof(KeyAttribute).FullName}\" ve \"{typeof(DatabaseGeneratedAttribute).FullName}\" belirtilmelidir!");
        }
        /// <summary> Bağlı bulunulan <see cref="DbContext"/> üzerinden SQL Server sunucusuna ait sistem özelliklerini asenkron olarak sorgular ve <see cref="SqlServerProperties"/> nesnesi olarak döndürür. </summary>
        /// <param name="context"> Sorgunun çalıştırılacağı veritabanı bağlamı.</param>
        /// <param name="cancellationToken"> İşlemi iptal etmek için kullanılabilecek isteğe bağlı <see cref="CancellationToken"/>.</param>
        public static Task<SqlServerProperties> GetServerProperty(this DbContext context, CancellationToken cancellationToken = default) => context.Database.SqlQueryRaw<SqlServerProperties>(SqlServerProperties.query()).FirstOrDefaultAsync(cancellationToken);
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
        /// tablolar için işlem uygulanır. <paramref name="isDebug"/> parametresi <see langword="true"/> olduğunda herhangi bir SQL komutu çalıştırılmaz ve metot 0 döner. İşlem uygulanacak tablo bulunamazsa yine 0 döndürülür.
        /// </summary>
        /// <param name="context"> SQL komutunun çalıştırılacağı <see cref="DbContext"/> örneği.</param>
        /// <param name="isDebug"> Debug modunu belirtir. <see langword="true"/> ise reseed işlemi yapılmaz.</param>
        /// <param name="mappedTables"> Reseed işlemi uygulanacak entity türleri. </param>
        /// <param name="cancellationToken"> İşlemi iptal etmek için kullanılabilecek isteğe bağlı <see cref="CancellationToken"/>.</param>
        /// <returns>Çalıştırılan SQL komutundan etkilenen satır sayısını temsil eden <see cref="Task{Int32}"/>. </returns>
        public static Task<int> TableReseed(this DbContext context, bool isDebug, Type[] mappedTables, CancellationToken cancellationToken = default)
        {
            if (isDebug) { return Task.FromResult(0); }
            var sb = new StringBuilder();
            var index = 0;
            mappedTables = mappedTables ?? [];
            foreach (var type in mappedTables.Where(x => x.IsMappedTable()).ToArray())
            {
                var pkInfo = getprimarykeyinfo(type);
                if (pkInfo.columnName == "" || pkInfo.sqlDbTypeName == "") { continue; }
                var tableName = type.GetTableName(true);
                var variableName = $"@MAXID_{index}";
                sb.AppendLine($"DECLARE {variableName} {pkInfo.sqlDbTypeName}");
                sb.AppendLine($"SELECT {variableName} = MAX([{pkInfo.columnName}]) FROM {tableName}");
                sb.AppendLine($"SET {variableName} = ISNULL({variableName}, 0)");
                sb.AppendLine($"DBCC CHECKIDENT ('{tableName}', RESEED, {variableName})");
                index++;
            }
            if (sb.Length == 0) { return Task.FromResult(0); }
            return context.Database.ExecuteSqlRawAsync(sb.ToString(), Array.Empty<SqlParameter>(), cancellationToken);
        }
        private static (string columnName, string sqlDbTypeName) getprimarykeyinfo(Type mappedtabletype)
        {
            if (Validators.TryTableisKeyAttribute(mappedtabletype, out PropertyInfo[] _properties) && _properties.Length == 1 && _properties[0].IsPK() && _properties[0].GetDatabaseGeneratedOption() == DatabaseGeneratedOption.Identity)
            {
                var propertytype = _properties[0].PropertyType;
                if (propertytype.IsEnum) { propertytype = Enum.GetUnderlyingType(propertytype); }
                if (propertytype == typeof(byte)) { return (_properties[0].GetColumnName(), "TINYINT"); }
                if (propertytype == typeof(short)) { return (_properties[0].GetColumnName(), "SMALLINT"); }
                if (propertytype == typeof(int)) { return (_properties[0].GetColumnName(), "INT"); }
                if (propertytype == typeof(long)) { return (_properties[0].GetColumnName(), "BIGINT"); }
            }
            return ("", "");
        }
    }
}