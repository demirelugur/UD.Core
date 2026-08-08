namespace UD.Core.Extensions
{
    using Ganss.Xss;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.EntityFrameworkCore.Metadata;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;
    using UD.Core.Attributes;
    using UD.Core.Auditings;
    using UD.Core.Helper;
    using UD.Core.Helper.Databases;
    public static class EntityFrameworkCoreExtensions
    {
        #region DbContext
        /// <summary>Belirtilen varlýðýn (entity) bir veya daha fazla özelliðinin deðiþtirilip deðiþtirilmediðini kontrol eder.</summary>
        /// <typeparam name="T">Kontrol edilecek varlýk türü.</typeparam>
        /// <param name="context">DbContext örneði.</param>
        /// <param name="entity">Deðiþiklik durumu kontrol edilecek varlýk.</param>
        /// <param name="expressions">Kontrol edilecek özelliklerin ifadeleri.</param>
        /// <returns>Deðiþtirilmiþse <see langword="true"/>, deðilse <see langword="false"/> döner.</returns>
        public static bool IsModified<T>(this DbContext context, T entity, params Expression<Func<T, object>>[] expressions) where T : class
        {
            var entry = context.Entry(entity);
            var properties = typeof(T).GetProperties().Where(x => x.IsMapped() && entry.Property(x.Name).IsModified).ToArray();
            var columns = (expressions ?? []).Select(x => x.GetMemberName()).ToArray();
            if (columns.Length == 0) { return properties.Length > 0; }
            return properties.Any(x => columns.Contains(x.Name));
        }
        /// <summary>Belirli bir bileþik anahtar(composite key) özelliði ile eski varlýðýn güncellenmesini saðlar.</summary>
        public static async Task<T> SetCompositeKey<T, CompositeKey>(this DbContext context, bool autoSave, T oldEntity, Expression<Func<T, CompositeKey>> compositeKey, CompositeKey compositeKeyNewValue, CancellationToken cancellationToken = default) where T : class, new()
        {
            var type = typeof(T);
            var tableName = type.GetTableName(false);
            var compositeKeyName = compositeKey.GetMemberName();
            var properties = type.GetProperties().Where(x => x.IsMapped()).Select(x => new
            {
                name = x.Name,
                isSetCompositeKeyName = x.Name == compositeKeyName,
                isCompositeKey = x.IsPK() && x.GetDatabaseGeneratedOption() == DatabaseGeneratedOption.None
            }).ToArray();
            if (!properties.Any(x => x.isSetCompositeKeyName && x.isCompositeKey))
            {
                if (Checks.IsEnglishCurrentUICulture) { throw new Exception($"The property \"{compositeKeyName}\" in table \"{tableName}\" must have either \"{typeof(KeyAttribute).FullName}\" and \"{typeof(DatabaseGeneratedAttribute).FullName}\" specified!"); }
                throw new Exception($"\"{tableName}\" tablosundaki \"{compositeKeyName}\" özelliðinde \"{typeof(KeyAttribute).FullName}\" ve \"{typeof(DatabaseGeneratedAttribute).FullName}\" belirtilmelidir!");
            }
            if (properties.Count(x => x.isCompositeKey) < 2)
            {
                if (Checks.IsEnglishCurrentUICulture) { throw new KeyNotFoundException($"The \"{tableName}\" table must contain at least 2 properties with \"{typeof(KeyAttribute).FullName}\" and \"{typeof(DatabaseGeneratedAttribute).FullName}\" attributes to continue processing!"); }
                throw new KeyNotFoundException($"Ýþleme devam edebilmek için \"{tableName}\" tablosunda en az 2 özelliðin \"{typeof(KeyAttribute).FullName}\" ve \"{typeof(DatabaseGeneratedAttribute).FullName}\" içermesi gerekmektedir!");
            }
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
        /// <summary> Baðlý bulunulan <see cref="DbContext"/> üzerinden SQL Server sunucusuna ait sistem özelliklerini asenkron olarak sorgular ve <see cref="SqlServerProperties"/> nesnesi olarak döndürür. </summary>
        /// <param name="context"> Sorgunun çalýþtýrýlacaðý veritabaný baðlamý.</param>
        /// <param name="cancellationToken"> Ýþlemi iptal etmek için kullanýlabilecek isteðe baðlý <see cref="CancellationToken"/>.</param>
        public static Task<SqlServerProperties> GetServerProperty(this DbContext context, CancellationToken cancellationToken = default) => context.Database.SqlQueryRaw<SqlServerProperties>(SqlServerProperties.query()).FirstOrDefaultAsync(cancellationToken);
        /// <summary>
        /// Belirtilen entity türlerine karþýlýk gelen tablolar için, Identity özelliðine sahip birincil anahtar alanlarýnýn mevcut maksimum deðerini baz alarak yeniden numaralandýrma (RESEED) iþlemini asenkron olarak gerçekleþtirir. Metot, her tablo için ilgili birincil anahtar kolonunun mevcut en büyük deðerini (MAX) hesaplar ve <c>DBCC CHECKIDENT</c> komutu ile Identity deðerini bu deðere eþitler. Böylece manuel veri ekleme, toplu veri taþýma veya seed iþlemleri sonrasý oluþabilecek kimlik (Identity) kaymalarýnýn önüne geçilmiþ olur.
        /// <para>Sadece</para>
        /// <list type="number">
        /// <item><description>Tek kolonlu birincil anahtara sahip</description></item>
        /// <item><description><see cref="DatabaseGeneratedOption.Identity"/> olarak iþaretlenmiþ</description></item>
        /// <item><description>Veri tipi TINYINT, SMALLINT, INT veya BIGINT olan</description></item>
        /// </list>
        /// tablolar için iþlem uygulanýr. <paramref name="isDebug"/> parametresi <see langword="true"/> olduðunda herhangi bir SQL komutu çalýþtýrýlmaz ve metot 0 döner. Ýþlem uygulanacak tablo bulunamazsa yine 0 döndürülür.
        /// </summary>
        /// <param name="context"> SQL komutunun çalýþtýrýlacaðý <see cref="DbContext"/> örneði.</param>
        /// <param name="isDebug"> Debug modunu belirtir. <see langword="true"/> ise reseed iþlemi yapýlmaz.</param>
        /// <param name="mappedTables"> Reseed iþlemi uygulanacak entity türleri. <see cref="TableAttribute"/> özelliðine sahip olmalýdýr.</param>
        /// <param name="cancellationToken"> Ýþlemi iptal etmek için kullanýlabilecek isteðe baðlý <see cref="CancellationToken"/>.</param>
        /// <returns>Çalýþtýrýlan SQL komutundan etkilenen satýr sayýsýný temsil eden <see cref="Task{Int32}"/>. </returns>
        public static Task<int> TableReseed(this DbContext context, bool isDebug, Type[] mappedTables, CancellationToken cancellationToken = default)
        {
            if (isDebug) { return Task.FromResult(0); }
            var sb = new StringBuilder();
            var index = 0;
            if (!mappedTables.All(x => x.IsMappedTable()))
            {
                if (Checks.IsEnglishCurrentUICulture) { throw new Exception($"All provided types must be mapped to database tables. Ensure that each type is decorated with the \"{nameof(TableAttribute)}\"."); }
                throw new Exception($"Tüm türler veritabaný tablolarýna eþlenmiþ olmalýdýr. Her bir türün \"{nameof(TableAttribute)}\" ile iþaretlendiðinden emin olun.");
            }
            foreach (var type in mappedTables)
            {
                var (columnName, sqlDbTypeName) = getPrimaryKeyInfo(type);
                if (columnName == "" || sqlDbTypeName == "") { continue; }
                var tableName = type.GetTableName(true);
                var variableName = String.Concat("@MAXID_", index);
                sb.AppendLine($"DECLARE {variableName} {sqlDbTypeName}");
                sb.AppendLine($"SELECT {variableName} = MAX([{columnName}]) FROM {tableName}");
                sb.AppendLine($"SET {variableName} = ISNULL({variableName}, 0)");
                sb.AppendLine($"DBCC CHECKIDENT ('{tableName}', RESEED, {variableName})");
                index++;
            }
            if (sb.Length == 0) { return Task.FromResult(0); }
            return context.Database.ExecuteSqlRawAsync(sb.ToString(), [], cancellationToken);
        }
        private static (string columnName, string sqlDbTypeName) getPrimaryKeyInfo(Type tableType)
        {
            if (TryValidators.TryTableisKeyAttribute(tableType, out PropertyInfo[] _properties) && _properties.Length == 1 && _properties[0].IsPK() && _properties[0].GetDatabaseGeneratedOption() == DatabaseGeneratedOption.Identity)
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
        /// <summary><paramref name="context"/> içerisindeki <see cref="DbContext.ChangeTracker"/> üzerinden eklenmiþ (Added) durumdaki entity&#39;leri tespit eder. Her entity için property bazýnda eski ve yeni deðerler karþýlaþtýrýlarak sadece deðeri deðiþmiþ olanlar filtrelenir. Sonuç olarak, eklenen kayýtlarýn detaylarýný içeren bir sözlük (Dictionary) yapýsý döndürülür. Bu yapý, eklenen kayýtlarýn kapsamlý bir þekilde izlenmesini saðlar.</summary>
        /// <param name="context">Ýþlem yapýlacak <see cref="DbContext"/> örneði.</param>
        /// <returns>Eklenen kayýtlarýn detaylarýný içeren bir <see cref="ChangeEntry"/> dizisi döndürür.</returns>
        public static ChangeEntry[] GetAdded(this DbContext context) => context.ChangeTracker.Entries().Where(x => x.State == EntityState.Added && x.Entity != null && !x.Entity.GetType().IsSkipAuditLog()).Select(ChangeEntry.ToEntityFromObject).ToArray();
        /// <summary><paramref name="context"/> içerisindeki <see cref="DbContext.ChangeTracker"/> üzerinden güncellenmiþ (Modified) durumdaki entity&#39;leri tespit eder. Her entity için property bazýnda eski ve yeni deðerler karþýlaþtýrýlarak sadece deðeri deðiþmiþ olanlar filtrelenir. Sonuç olarak, güncellenen kayýtlarýn detaylarýný içeren bir sözlük (Dictionary) yapýsý döndürülür. Bu yapý, güncellenen kayýtlarýn kapsamlý bir þekilde izlenmesini saðlar.</summary>
        /// <param name="context">Ýþlem yapýlacak <see cref="DbContext"/> örneði.</param>
        /// <returns>Güncellenen kayýtlarýn detaylarýný içeren bir <see cref="ChangeEntry"/> dizisi döndürür.</returns>
        public static ChangeEntry[] GetModified(this DbContext context) => context.ChangeTracker.Entries().Where(e => e.State == EntityState.Modified && e.Entity != null && !e.Entity.GetType().IsSkipAuditLog()).Select(ChangeEntry.ToEntityFromObject).ToArray();
        /// <summary><paramref name="context"/> içerisindeki <see cref="DbContext.ChangeTracker"/> üzerinden silinmiþ (Deleted) durumdaki entity&#39;leri tespit eder. Her entity için property bazýnda eski ve yeni deðerler karþýlaþtýrýlarak sadece deðeri deðiþmiþ olanlar filtrelenir. Sonuç olarak, silinen kayýtlarýn detaylarýný içeren bir sözlük (Dictionary) yapýsý döndürülür. Bu yapý, silinen kayýtlarýn kapsamlý bir þekilde izlenmesini saðlar.</summary>
        /// <param name="context">Ýþlem yapýlacak <see cref="DbContext"/> örneði.</param>
        /// <returns>Silinen kayýtlarýn detaylarýný içeren bir <see cref="ChangeEntry"/> dizisi döndürür.</returns>
        public static ChangeEntry[] GetDeleted(this DbContext context) => context.ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted && e.Entity != null && !e.Entity.GetType().IsSkipAuditLog()).Select(ChangeEntry.ToEntityFromObject).ToArray();
        #endregion
        #region ModelBuilder
        /// <summary>Modeldeki <see cref="ISoftDelete"/> arayüzünü uygulayan tüm entity tiplerine global sorgu filtresi ekleyerek, <c>IsDeleted = true</c> olan (soft delete edilmiþ) kayýtlarýn sorgularda varsayýlan olarak gelmesini engeller.</summary>
        /// <remarks>Bu filtre, yalnýzca <see cref="ISoftDelete"/> implement eden entity&#39;lere uygulanýr. Soft delete edilmiþ kayýtlarý da getirmek gerektiðinde EF Core tarafýnda <c>IgnoreQueryFilters()</c> kullanýlabilir.</remarks>
        /// <param name="modelBuilder">EF Core modelini yapýlandýrmak için kullanýlan <see cref="ModelBuilder"/>.</param>
        public static void ApplySoftDeleteQueryFilters(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (!typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType)) { continue; }
                var parameter = Expression.Parameter(entityType.ClrType, "x");
                var isDeletedProperty = Expression.Call(typeof(EF), nameof(EF.Property), [typeof(bool)], parameter, Expression.Constant(nameof(ISoftDelete.IsDeleted)));
                var filter = Expression.Lambda(Expression.Equal(isDeletedProperty, Expression.Constant(false)), parameter);
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
            }
        }
        #endregion
        #region EntityEntry
        private record SanitizeStringTruncateAccessor(IProperty property, int maxLength);
        private record NullableStructNullifyAccessor(IProperty property, Type underlyingType);
        private static SanitizeStringTruncateAccessor[] SanitizeCreateTruncateAccessor(IEntityType entityType) => entityType.GetProperties().Where(p => p.ClrType == typeof(string) && p.PropertyInfo.IsMapped()).Select(p => new SanitizeStringTruncateAccessor(p, p.GetMaxLength() ?? 0)).ToArray();
        private static NullableStructNullifyAccessor[] CreateNullifyAccessor(IEntityType entityType) => entityType.GetProperties().Where(p => p.PropertyInfo.IsMapped() && p.ClrType.IsNullable()).Select(p => new NullableStructNullifyAccessor(p, Nullable.GetUnderlyingType(p.ClrType))).ToArray();
        private static readonly ConcurrentDictionary<Type, SanitizeStringTruncateAccessor[]> _sanitizeTruncateCache = new();
        private static readonly ConcurrentDictionary<Type, NullableStructNullifyAccessor[]> _nullifyCache = new();
        /// <summary>
        /// <see cref="EntityEntry"/> içerisindeki string tipindeki property deðerlerini düzenler.
        /// <para>Ýþlem adýmlarý:</para>
        /// <list type="bullet">
        /// <item><description>String deðerler varsa, tanýmlý maksimum uzunluða (<c>maxLength</c>) göre kýsaltýlýr.</description></item>
        /// <item><description><paramref name="sanitizer"/> saðlanmýþsa ve property için sanitize atlanmamýþsa (<see cref="SkipSanitizeAttribute"/>), HTML/zararlý içerik temizliði uygulanýr.</description></item>
        /// <item><description>Sonuç deðer normalize edilerek (örn: null/boþ kontrolü) tekrar property&#39;e atanýr.</description></item>
        /// </list>
        /// <para>Bu method, özellikle veritabanýna yazýlmadan önce string alanlarýn güvenli ve uzunluk kýsýtlarýna uygun hale getirilmesi amacýyla kullanýlýr.</para>
        /// </summary>
        /// <param name="entry">Ýþlem yapýlacak entity&#39;nin <see cref="EntityEntry"/> nesnesi.</param>
        /// <param name="sanitizer">HTML içerik temizleme iþlemi için kullanýlacak <see cref="IHtmlSanitizer"/> instance&#39;ý. Null verilirse sanitize iþlemi uygulanmaz.</param>
        public static void SanitizeAndTruncate(this EntityEntry entry, IHtmlSanitizer? sanitizer)
        {
            var accessor = _sanitizeTruncateCache.GetOrAdd(entry.Metadata.ClrType, _ => SanitizeCreateTruncateAccessor(entry.Metadata));
            foreach (var item in accessor)
            {
                var propEntry = entry.Property(item.property.Name);
                if (propEntry.CurrentValue is String _s)
                {
                    if (item.maxLength > 0) { _s = _s.SubstringUpToLength(item.maxLength); }
                    if (sanitizer != null && !item.property.PropertyInfo.IsSkipSanitize()) { _s = sanitizer.Sanitize(_s); }
                    propEntry.CurrentValue = _s.ParseOrDefault<string>();
                }
            }
        }
        /// <summary><paramref name="entry"/> nesnesine ait nullable struct türündeki özelliklerin deðerlerini, eðer mevcut deðerleri ilgili struct türünün varsayýlan deðeriyle eþitse, null olarak günceller. Bu iþlem, veritabanýnda gereksiz yere varsayýlan deðerlerin saklanmasýný önlemek ve veri bütünlüðünü artýrmak amacýyla kullanýlabilir. Özellikle, nullable struct türlerinin kullanýldýðý durumlarda, bu tür özelliklerin null olarak kalmasý tercih edilebilir ve bu metot bu durumu saðlamak için tasarlanmýþtýr.</summary>
        public static void NullifyDefaultStructs(this EntityEntry entry)
        {
            var accessor = _nullifyCache.GetOrAdd(entry.Metadata.ClrType, _ => CreateNullifyAccessor(entry.Metadata));
            foreach (var item in accessor)
            {
                var propEntry = entry.Property(item.property.Name);
                if (propEntry.CurrentValue == null) { continue; }
                var defaultValue = Activator.CreateInstance(item.underlyingType);
                if (Equals(propEntry.CurrentValue, defaultValue)) { propEntry.CurrentValue = null; }
            }
        }
        #endregion
    }
}