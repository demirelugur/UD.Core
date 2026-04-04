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
    using UD.Core.Auditing;
    using UD.Core.Helper;
    using UD.Core.Helper.Database;
    using UD.Core.Helper.Validation;
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
            Guard.ThrowIfNull(context, nameof(context));
            var entry = context.Entry(entity);
            var properties = typeof(T).GetProperties().Where(x => x.IsMapped() && entry.Property(x.Name).IsModified).ToArray();
            var columns = (expressions ?? []).Select(x => x.GetMemberName()).ToArray();
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
            var compositeKeyName = compositeKey.GetMemberName();
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
        /// <summary><paramref name="context"/> içerisindeki <see cref="DbContext.ChangeTracker"/> üzerinden eklenmiþ (Added) durumdaki entity&#39;leri tespit eder. Her entity için property bazýnda eski ve yeni deðerler karþýlaþtýrýlarak sadece deðeri deðiþmiþ olanlar filtrelenir. Sonuç olarak, eklenen kayýtlarýn detaylarýný içeren bir sözlük (Dictionary) yapýsý döndürülür. Bu yapý, eklenen kayýtlarýn kapsamlý bir þekilde izlenmesini saðlar.</summary>
        /// <param name="context">Ýþlem yapýlacak <see cref="DbContext"/> örneði.</param>
        /// <returns>Eklenen kayýtlarýn detaylarýný içeren bir <see cref="ChangeEntry"/> dizisi döndürür.</returns>
        public static ChangeEntry[] GetAdded(this DbContext context) => context.ChangeTracker.Entries().Where(x => x.State == EntityState.Added).Select(ChangeEntry.ToEntityFromObject).ToArray();
        /// <summary><paramref name="context"/> içerisindeki <see cref="DbContext.ChangeTracker"/> üzerinden güncellenmiþ (Modified) durumdaki entity&#39;leri tespit eder. Her entity için property bazýnda eski ve yeni deðerler karþýlaþtýrýlarak sadece deðeri deðiþmiþ olanlar filtrelenir. Sonuç olarak, güncellenen kayýtlarýn detaylarýný içeren bir sözlük (Dictionary) yapýsý döndürülür. Bu yapý, güncellenen kayýtlarýn kapsamlý bir þekilde izlenmesini saðlar.</summary>
        /// <param name="context">Ýþlem yapýlacak <see cref="DbContext"/> örneði.</param>
        /// <returns>Güncellenen kayýtlarýn detaylarýný içeren bir <see cref="ChangeEntry"/> dizisi döndürür.</returns>
        public static ChangeEntry[] GetModified(this DbContext context) => context.ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).Select(ChangeEntry.ToEntityFromObject).ToArray();
        /// <summary><paramref name="context"/> içerisindeki <see cref="DbContext.ChangeTracker"/> üzerinden silinmiþ (Deleted) durumdaki entity&#39;leri tespit eder. Her entity için property bazýnda eski ve yeni deðerler karþýlaþtýrýlarak sadece deðeri deðiþmiþ olanlar filtrelenir. Sonuç olarak, silinen kayýtlarýn detaylarýný içeren bir sözlük (Dictionary) yapýsý döndürülür. Bu yapý, silinen kayýtlarýn kapsamlý bir þekilde izlenmesini saðlar.</summary>
        /// <param name="context">Ýþlem yapýlacak <see cref="DbContext"/> örneði.</param>
        /// <returns>Silinen kayýtlarýn detaylarýný içeren bir <see cref="ChangeEntry"/> dizisi döndürür.</returns>
        public static ChangeEntry[] GetDeleted(this DbContext context) => context.ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted).Select(ChangeEntry.ToEntityFromObject).ToArray();
        #endregion
        #region ModelBuilder
        /// <summary>Modeldeki <see cref="ISoftDelete"/> arayüzünü uygulayan tüm entity tiplerine global sorgu filtresi ekleyerek, <c>IsDeleted = true</c> olan (soft delete edilmiþ) kayýtlarýn sorgularda varsayýlan olarak gelmesini engeller.</summary>
        /// <remarks>Bu filtre, yalnýzca <see cref="ISoftDelete"/> implement eden entity&#39;lere uygulanýr. Soft delete edilmiþ kayýtlarý da getirmek gerektiðinde EF Core tarafýnda <c>IgnoreQueryFilters()</c> kullanýlabilir.</remarks>
        /// <param name="modelBuilder">EF Core modelini yapýlandýrmak için kullanýlan <see cref="ModelBuilder"/>.</param>
        public static void ApplySoftDeleteQueryFilters(this ModelBuilder modelBuilder)
        {
            Guard.ThrowIfNull(modelBuilder, nameof(modelBuilder));
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
        private static SanitizeStringTruncateAccessor[] SanitizeCreateTruncateAccessor(IEntityType entityType) => entityType.GetProperties().Where(p => p.ClrType == typeof(string) && p.PropertyInfo != null && p.PropertyInfo.IsMapped()).Select(p => new SanitizeStringTruncateAccessor(p, p.GetMaxLength() ?? 0)).ToArray();
        private static NullableStructNullifyAccessor[] CreateNullifyAccessor(IEntityType entityType) => entityType.GetProperties().Where(p => p.ClrType.IsNullable() && p.PropertyInfo != null && p.PropertyInfo.IsMapped()).Select(p => new NullableStructNullifyAccessor(p, Nullable.GetUnderlyingType(p.ClrType))).ToArray();
        private static readonly ConcurrentDictionary<Type, SanitizeStringTruncateAccessor[]> _sanitizeTruncateCache = new();
        private static readonly ConcurrentDictionary<Type, NullableStructNullifyAccessor[]> _nullifyCache = new();
        /// <summary><paramref name="entry"/> nesnesine ait string türündeki özelliklerin deðerlerini, belirtilen <paramref name="sanitizer"/> aracýlýðýyla temizler. Bu iþlem, potansiyel olarak zararlý HTML içeriðinin etkisiz hale getirilmesini saðlar. Özellikle kullanýcý tarafýndan saðlanan verilerin güvenliðini artýrmak amacýyla kullanýlabilir. Temizleme iþlemi sýrasýnda, özelliklerin üzerinde <see cref="SkipSanitizeAttribute"/> özniteliði bulunuyorsa, bu özellikler atlanýr ve temizlenmez. Bu sayede, belirli özelliklerin temizlenmeden kalmasý saðlanabilir.</summary>
        public static void SanitizeHtmlStrings(this EntityEntry entry, IHtmlSanitizer sanitizer)
        {
            Guard.ThrowIfNull(entry, nameof(entry));
            Guard.ThrowIfNull(sanitizer, nameof(sanitizer));
            var accessor = _sanitizeTruncateCache.GetOrAdd(entry.Metadata.ClrType, _ => SanitizeCreateTruncateAccessor(entry.Metadata));
            foreach (var item in accessor)
            {
                if (item.property.PropertyInfo.IsSkipSanitize()) { continue; }
                var propEntry = entry.Property(item.property.Name);
                if (propEntry.CurrentValue is String _s) { propEntry.CurrentValue = sanitizer.Sanitize(_s).ParseOrDefault<string>(); }
            }
        }
        /// <summary><paramref name="entry"/> nesnesine ait string türündeki özelliklerin deðerlerini, ilgili özellikler için tanýmlanmýþ maksimum uzunluklara göre keser. Bu iþlem, veritabaný þemasýnda belirtilen maksimum uzunluk sýnýrlarýna uyum saðlamak ve olasý veri kaybýný önlemek amacýyla kullanýlabilir.</summary>
        public static void TruncateStringsToMaxLength(this EntityEntry entry)
        {
            Guard.ThrowIfNull(entry, nameof(entry));
            var accessor = _sanitizeTruncateCache.GetOrAdd(entry.Metadata.ClrType, _ => SanitizeCreateTruncateAccessor(entry.Metadata));
            foreach (var item in accessor)
            {
                if (item.maxLength <= 0) { continue; }
                var propEntry = entry.Property(item.property.Name);
                if (propEntry.CurrentValue is String _s) { propEntry.CurrentValue = _s.SubstringUpToLength(item.maxLength).ParseOrDefault<string>(); }
            }
        }
        /// <summary><paramref name="entry"/> nesnesine ait nullable struct türündeki özelliklerin deðerlerini, eðer mevcut deðerleri ilgili struct türünün varsayýlan deðeriyle eþitse, null olarak günceller. Bu iþlem, veritabanýnda gereksiz yere varsayýlan deðerlerin saklanmasýný önlemek ve veri bütünlüðünü artýrmak amacýyla kullanýlabilir. Özellikle, nullable struct türlerinin kullanýldýðý durumlarda, bu tür özelliklerin null olarak kalmasý tercih edilebilir ve bu metot bu durumu saðlamak için tasarlanmýþtýr.</summary>
        public static void NullifyDefaultStructs(this EntityEntry entry)
        {
            Guard.ThrowIfNull(entry, nameof(entry));
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