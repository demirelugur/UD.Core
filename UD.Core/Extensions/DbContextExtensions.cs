namespace UD.Core.Extensions
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;
    using UD.Core.Helper;
    using UD.Core.Helper.Database;
    using UD.Core.Helper.Validation;
    public static class DbContextExtensions
    {
        /// <summary>Belirtilen varlýðýn (entity) bir veya daha fazla özelliðinin deðiþtirilip deðiþtirilmediðini kontrol eder.</summary>
        /// <typeparam name="T">Kontrol edilecek varlýk türü.</typeparam>
        /// <param name="context">DbContext örneði.</param>
        /// <param name="entity">Deðiþiklik durumu kontrol edilecek varlýk.</param>
        /// <param name="expressions">Kontrol edilecek özelliklerin ifadeleri.</param>
        /// <returns>Deðiþtirilmiþse <see langword="true"/>, deðilse <see langword="false"/> döner.</returns>
        public static bool IsModified<T>(this DbContext context, T entity, params Expression<Func<T, object>>[] expressions) where T : class
        {
            Guard.ThrowIfNull(context, nameof(context));
            var entry = context.Entry(entity);
            var properties = typeof(T).GetProperties().Where(x => x.IsMapped() && entry.Property(x.Name).IsModified).ToArray();
            var columns = (expressions ?? []).Select(x => x.GetExpressionName()).ToArray();
            if (columns.Length == 0) { return properties.Length > 0; }
            return properties.Any(x => columns.Contains(x.Name));
        }
        /// <summary>Belirli bir bileþik anahtar(composite key) özelliði ile eski varlýðýn güncellenmesini saðlar.</summary>
        public static async Task<T> SetCompositeKey<T, CompositeKey>(this DbContext context, bool autoSave, T oldEntity, Expression<Func<T, CompositeKey>> compositeKey, CompositeKey compositeKeyNewValue, CancellationToken cancellationToken = default) where T : class, new()
        {
            Guard.ThrowIfNull(context, nameof(context));
            Guard.ThrowIfNull(oldEntity, nameof(oldEntity));
            var type = typeof(T);
            var tableName = type.GetTableName(false);
            var compositeKeyName = compositeKey.GetExpressionName();
            var properties = type.GetProperties().Where(x => x.IsMapped()).Select(x => new
            {
                name = x.Name,
                isSetCompositeKeyName = x.Name == compositeKeyName,
                isCompositeKey = x.IsPK() && x.GetDatabaseGeneratedOption() == DatabaseGeneratedOption.None
            }).ToArray();
            if (properties.Count(x => x.isCompositeKey) < 2)
            {
                if (ValidationChecks.IsEnglishDefaultThreadCurrentUICulture) { throw new KeyNotFoundException($"The \"{tableName}\" table must contain at least 2 properties with \"{typeof(KeyAttribute).FullName}\" and \"{typeof(DatabaseGeneratedAttribute).FullName}\" attributes to continue processing!"); }
                throw new KeyNotFoundException($"Ýþleme devam edebilmek için \"{tableName}\" tablosunda en az 2 özelliðin \"{typeof(KeyAttribute).FullName}\" ve \"{typeof(DatabaseGeneratedAttribute).FullName}\" içermesi gerekmektedir!");
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
                }).ToArray()) { Utilities.SetPropertyValue(newEntity, item.name, (item.isSetCompositeKeyName ? compositeKeyNewValue : entry.Property(item.name).OriginalValue)); }
                await dbSet.AddAsync(newEntity, cancellationToken);
                dbSet.Remove(oldEntity);
                if (autoSave) { await context.SaveChangesAsync(cancellationToken); }
                return newEntity;
            }
            if (ValidationChecks.IsEnglishDefaultThreadCurrentUICulture) { throw new Exception($"The property \"{compositeKeyName}\" in table \"{tableName}\" must have either \"{typeof(KeyAttribute).FullName}\" and \"{typeof(DatabaseGeneratedAttribute).FullName}\" specified!"); }
            throw new Exception($"\"{tableName}\" tablosundaki \"{compositeKeyName}\" özelliðinde \"{typeof(KeyAttribute).FullName}\" ve \"{typeof(DatabaseGeneratedAttribute).FullName}\" belirtilmelidir!");
        }
        /// <summary> Baðlý bulunulan <see cref="DbContext"/> üzerinden SQL Server sunucusuna ait sistem özelliklerini asenkron olarak sorgular ve <see cref="SqlServerProperties"/> nesnesi olarak döndürür. </summary>
        /// <param name="context"> Sorgunun çalýþtýrýlacaðý veritabaný baðlamý.</param>
        /// <param name="cancellationToken"> Ýþlemi iptal etmek için kullanýlabilecek isteðe baðlý <see cref="CancellationToken"/>.</param>
        public static Task<SqlServerProperties> GetServerProperty(this DbContext context, CancellationToken cancellationToken = default) => context.Database.SqlQueryRaw<SqlServerProperties>(SqlServerProperties.query()).FirstOrDefaultAsync(cancellationToken);
        /// <summary>
        /// Belirtilen entity türlerine karþýlýk gelen tablolar için, Identity özelliðine sahip birincil anahtar alanlarýnýn mevcut maksimum deðerini baz alarak yeniden numaralandýrma (RESEED) iþlemini asenkron olarak gerçekleþtirir. Metot, her tablo için ilgili birincil anahtar kolonunun mevcut en büyük deðerini (MAX) hesaplar ve <c>DBCC CHECKIDENT</c> komutu ile Identity deðerini bu deðere eþitler. Böylece manuel veri ekleme, toplu veri taþýma veya seed iþlemleri sonrasý oluþabilecek kimlik (Identity) kaymalarýnýn önüne geçilmiþ olur.
        /// <br />
        /// <br />
        /// Sadece
        /// <list type="number">
        /// <item><description>Tek kolonlu birincil anahtara sahip</description></item>
        /// <item><description><see cref="DatabaseGeneratedOption.Identity"/> olarak iþaretlenmiþ</description></item>
        /// <item><description>Veri tipi TINYINT, SMALLINT, INT veya BIGINT olan</description></item>
        /// </list>
        /// tablolar için iþlem uygulanýr. <paramref name="isDebug"/> parametresi <see langword="true"/> olduðunda herhangi bir SQL komutu çalýþtýrýlmaz ve metot 0 döner. Ýþlem uygulanacak tablo bulunamazsa yine 0 döndürülür.
        /// </summary>
        /// <param name="context"> SQL komutunun çalýþtýrýlacaðý <see cref="DbContext"/> örneði.</param>
        /// <param name="isDebug"> Debug modunu belirtir. <see langword="true"/> ise reseed iþlemi yapýlmaz.</param>
        /// <param name="mappedTables"> Reseed iþlemi uygulanacak entity türleri. </param>
        /// <param name="cancellationToken"> Ýþlemi iptal etmek için kullanýlabilecek isteðe baðlý <see cref="CancellationToken"/>.</param>
        /// <returns>Çalýþtýrýlan SQL komutundan etkilenen satýr sayýsýný temsil eden <see cref="Task{Int32}"/>. </returns>
        public static Task<int> TableReseed(this DbContext context, bool isDebug, Type[] mappedTables, CancellationToken cancellationToken = default)
        {
            if (isDebug) { return Task.FromResult(0); }
            Guard.ThrowIfNull(context, nameof(context));
            Guard.ThrowIfEmpty(mappedTables, nameof(mappedTables));
            var sb = new StringBuilder();
            var index = 0;
            foreach (var type in mappedTables.Where(x => x.IsMappedTable()).ToArray())
            {
                var (columnName, sqlDbTypeName) = getPrimaryKeyInfo(type);
                if (columnName == "" || sqlDbTypeName == "") { continue; }
                var tableName = type.GetTableName(true);
                var variableName = $"@MAXID_{index}";
                sb.AppendLine($"DECLARE {variableName} {sqlDbTypeName}");
                sb.AppendLine($"SELECT {variableName} = MAX([{columnName}]) FROM {tableName}");
                sb.AppendLine($"SET {variableName} = ISNULL({variableName}, 0)");
                sb.AppendLine($"DBCC CHECKIDENT ('{tableName}', RESEED, {variableName})");
                index++;
            }
            if (sb.Length == 0) { return Task.FromResult(0); }
            return context.Database.ExecuteSqlRawAsync(sb.ToString(), [], cancellationToken);
        }
        private static (string columnName, string sqlDbTypeName) getPrimaryKeyInfo(Type mappedtabletype)
        {
            if (TryValidators.TryTableisKeyAttribute(mappedtabletype, out PropertyInfo[] _properties) && _properties.Length == 1 && _properties[0].IsPK() && _properties[0].GetDatabaseGeneratedOption() == DatabaseGeneratedOption.Identity)
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
        /// <summary><paramref name="context"/> içerisindeki <see cref="DbContext.ChangeTracker"/> üzerinden eklenmiþ (Added), güncellenmiþ (Modified) ve silinmiþ (Deleted) durumdaki entity&#39;leri tespit eder. Her entity için original ve current deðerler karþýlaþtýrýlarak deðiþim bilgileri içeren bir ChangeEntry listesi oluþturulur. Bu metot, veri deðiþikliklerini izlemek ve kaydetmek için kullanýlabilir. Deðiþiklik türüne göre property bazýnda eski ve yeni deðerler birlikte tutulur.</summary>
        public static ChangeEntry[] GetAllChanges(this DbContext context) => context.GetAdded().Union(context.GetModified()).Union(context.GetDeleted()).ToArray();
        /// <summary><paramref name="context"/> içerisindeki <see cref="DbContext.ChangeTracker"/> üzerinden eklenmiþ (Added) durumdaki entity&#39;leri tespit eder. Her entity için mevcut (current) deðerler alýnarak ChangeEntry listesi oluþturulur. Eklenen kayýtlar için original deðerler bulunmadýðýndan null olarak atanýr. Property bazýnda bir sözlük (Dictionary) ile deðiþim bilgileri döndürülür.</summary>
        public static ChangeEntry[] GetAdded(this DbContext context) => context.ChangeTracker
        .Entries()
        .Where(e => e.State == EntityState.Added)
        .Select(entry =>
        {
            var changes = entry.CurrentValues.Properties.Where(prop => prop.PropertyInfo.IsMapped())
            .ToDictionary(
               prop => prop.PropertyInfo.GetColumnName(),
               prop => new ChangePropertyInfo(null, entry.CurrentValues[prop])
            );
            return new ChangeEntry(entry, changes);
        }).ToArray();
        /// <summary><paramref name="context"/> içerisindeki <see cref="DbContext.ChangeTracker"/> üzerinden güncellenmiþ (Modified) durumdaki entity&#39;leri tespit eder. Her entity için hem original hem current deðerler karþýlaþtýrýlýr. Sadece deðeri deðiþmiþ olan property&#39;ler filtrelenerek ChangeEntry listesi oluþturulur. Property bazýnda eski ve yeni deðerler birlikte tutulur.</summary>
        public static ChangeEntry[] GetModified(this DbContext context) => context.ChangeTracker
        .Entries()
        .Where(e => e.State == EntityState.Modified)
        .Select(entry =>
        {
            var changes = entry.OriginalValues.Properties.Where(prop => prop.PropertyInfo.IsMapped())
                .Select(prop => new
                {
                    Property = prop,
                    Original = entry.OriginalValues[prop],
                    Current = entry.CurrentValues[prop]
                })
                .Where(x => !Equals(x.Original, x.Current))
                .ToDictionary(
                    prop => prop.Property.PropertyInfo.GetColumnName(),
                    prop => new ChangePropertyInfo(prop.Original, prop.Current)
                );
            return new ChangeEntry(entry, changes);
        }).ToArray();
        /// <summary><paramref name="context"/> içerisindeki <see cref="DbContext.ChangeTracker"/> üzerinden silinmiþ (Deleted) durumdaki entity&#39;leri tespit eder. Silinen kayýtlar için sadece original deðerler alýnýr, current deðerler null olarak atanýr. Property bazýnda bir sözlük (Dictionary) ile silinmeden önceki deðerler döndürülür.</summary>
        public static ChangeEntry[] GetDeleted(this DbContext context) => context.ChangeTracker
        .Entries()
        .Where(e => e.State == EntityState.Deleted)
        .Select(entry =>
        {
            var changes = entry.OriginalValues.Properties.Where(prop => prop.PropertyInfo.IsMapped())
            .ToDictionary(
                prop => prop.PropertyInfo.GetColumnName(),
                prop => new ChangePropertyInfo(entry.OriginalValues[prop], null)
            );
            return new ChangeEntry(entry, changes);
        }).ToArray();
    }
}