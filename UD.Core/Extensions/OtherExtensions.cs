namespace UD.Core.Extensions
{
    using Ganss.Xss;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Concurrent;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using System.Net.Mail;
    using System.Reflection;
    using System.Web;
    using UD.Core.Attributes;
    using UD.Core.Auditing;
    using UD.Core.Helper;
    using UD.Core.Helper.Services;
    using UD.Core.Helper.Validation;
    public static class OtherExtensions
    {
        /// <summary>Verilen e-Posta adresindeki &quot;@&quot; karakterini &quot;[at]&quot; ile değiştirir.</summary>
        /// <param name="mailAddress">MailAddress nesnesi.</param>
        /// <returns>Dönüştürülmüş e-Posta adresi.</returns>
        /// <exception cref="ArgumentNullException">Verilen e-Posta adresi null ise fırlatılır.</exception>
        public static string ReplaceAT(this MailAddress mailAddress)
        {
            Guard.ThrowIfNull(mailAddress, nameof(mailAddress));
            return mailAddress.Address.Replace("@", "[at]");
        }
        /// <summary>Verilen validation bağlamında bir özelliğin gerekli olup olmadığını kontrol eder.</summary>
        /// <param name="validationContext">ValidationContext nesnesi.</param>
        /// <returns>Gerekli ise <see langword="true"/>, değilse <see langword="false"/> döner.</returns>
        public static bool IsRequiredAttribute(this ValidationContext validationContext)
        {
            Guard.ThrowIfNull(validationContext, nameof(validationContext));
            var property = validationContext.ObjectInstance.GetType().GetProperty(validationContext.MemberName);
            return (property != null && TryValidators.TryCustomAttribute(property, out RequiredAttribute _));
        }
        /// <summary>Verilen doğrulama bağlamına göre, belirtilen özelliğin değerini günceller. Eğer özellik yazılabilir durumdaysa, yeni değer atanır.</summary>
        /// <param name="validationContext">Doğrulama işlemi sırasında bağlam bilgilerini içeren nesne.</param>
        /// <param name="value">Güncellenmek istenen yeni değer.</param>
        /// <exception cref="ArgumentNullException">Eğer <paramref name="validationContext"/> null ise tetiklenir.</exception>
        public static void SetValidatePropertyValue(this ValidationContext validationContext, object value)
        {
            Guard.ThrowIfNull(validationContext, nameof(validationContext));
            var property = validationContext.ObjectType.GetProperty(validationContext.MemberName);
            if (property != null && property.CanWrite) { property.SetValue(validationContext.ObjectInstance, value); }
        }
        /// <summary>Verilen ifadenin adını alır.</summary>
        /// <param name="expression">İfade.</param>
        /// <returns>İfadenin adı.</returns>
        /// <exception cref="ArgumentException">İfade geçersiz ise fırlatılır.</exception>
        public static string GetExpressionName(this Expression expression)
        {
            Guard.ThrowIfNull(expression, nameof(expression));
            if (expression is LambdaExpression _lambda) { expression = _lambda.Body; }
            var result = "";
            if (expression is MemberExpression _me) { result = _me.Member.Name; }
            else if (expression is UnaryExpression _ue)
            {
                if (_ue.Operand is MemberExpression _ume) { result = _ume.Member.Name; }
                else if (_ue.Operand is MethodCallExpression _umce && _umce.Object is ConstantExpression _uce && _uce.Value != null)
                {
                    if (_uce.Value is MemberInfo _mi) { result = _mi.Name; }
                    else if (TryValidators.TryGetProperty(_uce.Value, nameof(MemberInfo.Name), out string _name)) { result = _name; }
                }
            }
            if (result.IsNullOrEmpty())
            {
                if (ValidationChecks.IsEnglishDefaultThreadCurrentUICulture) { throw new ArgumentException($"The value of \"{expression}\" is incompatible!", nameof(expression)); }
                throw new ArgumentException($"\"{expression}\" değeri uyumsuzdur!", nameof(expression));
            }
            return result;
        }
        /// <summary>Verilen <see cref="SqlDbType"/> enum değerini, SQL Server sistem tür kimliğine (<c>[system_type_id]</c>) dönüştürür. Bu kimlikler, SQL Server&#39;ın [sys].[types] sistem tablosunda bulunan ve her veri türü için benzersiz olan sayısal değerlerdir.</summary>
        /// <param name="type">Dönüştürülecek <see cref="SqlDbType"/> enum değeri.</param>
        /// <returns>SQL Server sistem tür kimliği (<c>[system_type_id]</c>) değeri</returns>
        /// <exception cref="NotSupportedException">Desteklenmeyen bir <see cref="SqlDbType"/> değeri verildiğinde fırlatılır.</exception>
        /// <remarks>SELECT [name], [system_type_id] FROM [sys].[types]</remarks>
        public static int ToSystemTypeId(this SqlDbType type)
        {
            return type switch
            {
                SqlDbType.Image => 34,
                SqlDbType.Text => 35,
                SqlDbType.UniqueIdentifier => 36,
                SqlDbType.Date => 40,
                SqlDbType.Time => 41,
                SqlDbType.DateTime2 => 42,
                SqlDbType.DateTimeOffset => 43,
                SqlDbType.TinyInt => 48,
                SqlDbType.SmallInt => 52,
                SqlDbType.Int => 56,
                SqlDbType.SmallDateTime => 58,
                SqlDbType.Real => 59,
                SqlDbType.Money => 60,
                SqlDbType.DateTime => 61,
                SqlDbType.Float => 62,
                SqlDbType.NText => 99,
                SqlDbType.Bit => 104,
                SqlDbType.Decimal => 106,
                SqlDbType.SmallMoney => 122,
                SqlDbType.BigInt => 127,
                SqlDbType.VarBinary => 165,
                SqlDbType.VarChar => 167,
                SqlDbType.Binary => 173,
                SqlDbType.Char => 175,
                SqlDbType.Timestamp => 189,
                SqlDbType.NVarChar => 231,
                SqlDbType.NChar => 239,
                SqlDbType.Xml => 241,
                _ => throw Utilities.ThrowNotSupportedForEnum<SqlDbType>()
            };
        }
        /// <summary>Verilen <see cref="SqlDbType"/> enum değerini, ADO.NET&#39;in genel veri türü olan <see cref="DbType"/>&#39;a dönüştürür. SQL Server&#39;a özgü veri türlerini platform bağımsız <see cref="DbType"/> türlerine çevirir.</summary>
        /// <param name="type">Dönüştürülecek <see cref="SqlDbType"/> enum değeri.</param>
        /// <returns>Eşdeğer <see cref="DbType"/> enum değeri.</returns>
        /// <exception cref="NotSupportedException">Desteklenmeyen bir <see cref="SqlDbType"/> değeri verildiğinde fırlatılır.</exception>
        public static DbType ToDbType(this SqlDbType type)
        {
            return type switch
            {
                SqlDbType.UniqueIdentifier => DbType.Guid,
                SqlDbType.Date => DbType.Date,
                SqlDbType.Time => DbType.Time,
                SqlDbType.DateTime2 => DbType.DateTime2,
                SqlDbType.DateTimeOffset => DbType.DateTimeOffset,
                SqlDbType.TinyInt => DbType.Byte,
                SqlDbType.SmallInt => DbType.Int16,
                SqlDbType.Int => DbType.Int32,
                SqlDbType.Real => DbType.Single,
                SqlDbType.Money => DbType.Currency,
                SqlDbType.DateTime => DbType.DateTime,
                SqlDbType.Float => DbType.Double,
                SqlDbType.Bit => DbType.Boolean,
                SqlDbType.Decimal => DbType.Decimal,
                SqlDbType.BigInt => DbType.Int64,
                SqlDbType.VarChar => DbType.AnsiString,
                SqlDbType.Binary => DbType.Binary,
                SqlDbType.VarBinary => DbType.Binary,
                SqlDbType.Char => DbType.AnsiStringFixedLength,
                SqlDbType.NVarChar => DbType.String,
                SqlDbType.NChar => DbType.StringFixedLength,
                SqlDbType.Xml => DbType.Xml,
                _ => throw Utilities.ThrowNotSupportedForEnum<SqlDbType>()
            };
        }
        /// <summary>Verilen <see cref="DbType"/> enum değerini, SQL Server&#39;a özgü <see cref="SqlDbType"/> enum değerine dönüştürür. ADO.NET&#39;in genel veri türleri (<see cref="DbType"/>) ile SQL Server&#39;ın özel veri türleri (<see cref="SqlDbType"/>) arasında eşleme yapar.</summary>
        /// <param name="type">Dönüştürülecek <see cref="DbType"/> enum değeri.</param>
        /// <returns>Eşdeğer <see cref="SqlDbType"/> enum değeri.</returns>
        /// <exception cref="NotSupportedException">Desteklenmeyen bir <see cref="DbType"/> değeri verildiğinde fırlatılır.</exception>
        public static SqlDbType ToSqlDbType(this DbType type)
        {
            return type switch
            {
                DbType.Guid => SqlDbType.UniqueIdentifier,
                DbType.Date => SqlDbType.Date,
                DbType.Time => SqlDbType.Time,
                DbType.DateTime2 => SqlDbType.DateTime2,
                DbType.DateTimeOffset => SqlDbType.DateTimeOffset,
                DbType.Byte => SqlDbType.TinyInt,
                DbType.Int16 => SqlDbType.SmallInt,
                DbType.Int32 => SqlDbType.Int,
                DbType.Single => SqlDbType.Real,
                DbType.Currency => SqlDbType.Money,
                DbType.DateTime => SqlDbType.DateTime,
                DbType.Double => SqlDbType.Float,
                DbType.Boolean => SqlDbType.Bit,
                DbType.Decimal => SqlDbType.Decimal,
                DbType.Int64 => SqlDbType.BigInt,
                DbType.AnsiString => SqlDbType.VarChar,
                DbType.Binary => SqlDbType.VarBinary,
                DbType.AnsiStringFixedLength => SqlDbType.Char,
                DbType.String => SqlDbType.NVarChar,
                DbType.StringFixedLength => SqlDbType.NChar,
                DbType.Xml => SqlDbType.Xml,
                _ => throw Utilities.ThrowNotSupportedForEnum<DbType>()
            };
        }
        /// <summary><paramref name="jToken"/> değeri null, <see cref="JTokenType.None"/>, <see cref="JTokenType.Null"/> veya <see cref="JTokenType.Undefined"/> ise <see langword="true"/> döner; aksi takdirde <see langword="false"/> döner. Bu metot, bir <see cref="JToken"/> nesnesinin geçerli bir değere sahip olup olmadığını kontrol etmek için kullanılır.</summary>
        public static bool IsNullOrUndefined(this JToken jToken) => (jToken == null || jToken.Type.Includes(JTokenType.None, JTokenType.Null, JTokenType.Undefined));
        /// <summary>Bir <see cref="JToken"/> nesnesini belirtilen <typeparamref name="TKey"/> türündeki bir diziye dönüştürür. Eğer <see cref="JToken"/> null ise boş bir dizi döner, array türünde ise içindeki değerleri <typeparamref name="TKey"/> türüne çevirip dizi olarak döner. Diğer durumlarda bir istisna fırlatır.</summary>
        /// <typeparam name="TKey">Dönüştürülecek hedef veri türü.</typeparam>
        /// <param name="jToken">Dönüştürülecek <see cref="JToken"/> nesnesi.</param>
        /// <returns><typeparamref name="TKey"/> türünden bir dizi.</returns>
        /// <exception cref="NotSupportedException"><see cref="JToken"/> türü null veya array değilse fırlatılır.</exception>
        public static TKey[] ToArrayFromJToken<TKey>(this JToken jToken)
        {
            if (jToken.IsNullOrUndefined()) { return []; }
            if (jToken.Type == JTokenType.Array) { return jToken.Select(x => x.Value<TKey>()).ToArray(); }
            if (ValidationChecks.IsEnglishDefaultThreadCurrentUICulture) { throw new NotSupportedException($"The type of \"{nameof(jToken)}\" is incompatible!"); }
            throw new NotSupportedException($"\"{nameof(jToken)}\" türü uyumsuzdur!");
        }
        /// <summary><see cref="QueryString"/> içindeki belirtilen anahtarı alır ve uygun türde bir değere dönüştürür. Eğer anahtar bulunamazsa veya dönüştürme başarısız olursa, varsayılan değeri döner.</summary>
        /// <typeparam name="TKey">Dönüştürülecek hedef tür.</typeparam>
        /// <param name="queryString">İçinde sorgu parametrelerini barındıran <see cref="QueryString"/> nesnesi.</param>
        /// <param name="key">Alınacak sorgu parametresinin adı (anahtar).</param>
        /// <returns>Başarılıysa sorgu parametresi uygun türe dönüştürülür, aksi halde varsayılan değer döner.</returns>
        public static TKey ParseOrDefault<TKey>(this QueryString queryString, string key)
        {
            Guard.ThrowIfEmpty(key, nameof(key));
            var querydic = (queryString.HasValue ? HttpUtility.ParseQueryString(queryString.Value) : []);
            if (querydic.AllKeys.Contains(key)) { return querydic[key].ParseOrDefault<TKey>(); }
            return default;
        }
        /// <summary>Belirtilen sütun adını kullanarak <see cref="IDataReader"/> içinden değeri okur ve verilen türde (<typeparamref name="TKey"/>) döndürür. Eğer sütun değeri <c>null</c>, <see cref="DBNull"/> ya da dönüştürülemeyen bir değer içeriyorsa, türün varsayılan değerini (<c>default</c>) geri döner.</summary>
        /// <typeparam name="TKey">Dönüştürülmek istenen hedef tür.</typeparam>
        /// <param name="reader">Veri kaynağından okuma yapan <see cref="IDataReader"/> nesnesi.</param>
        /// <param name="key">Okunacak sütunun adı.</param>
        /// <returns>Sütun değeri başarıyla dönüştürülebilirse <typeparamref name="TKey"/> tipinde değer, aksi durumda varsayılan değer döner.</returns>
        public static TKey ParseOrDefault<TKey>(this IDataReader reader, string key)
        {
            Guard.ThrowIfNull(reader, nameof(reader));
            Guard.ThrowIfEmpty(key, nameof(key));
            try
            {
                var value = reader[key];
                if (value == null || value == DBNull.Value) { return default; }
                return value.ToString().ParseOrDefault<TKey>();
            }
            catch { return default; }
        }
        /// <summary>Stopwatch&#39;ı durdurur ve geçen süreyi döner.</summary>
        /// <param name="stopWatch">Zamanlayıcı nesnesi.</param>
        /// <returns>Durdurulduktan sonra geçen süre.</returns>
        public static TimeSpan StopThenGetElapsed(this Stopwatch stopWatch)
        {
            Guard.ThrowIfNull(stopWatch, nameof(stopWatch));
            stopWatch.Stop();
            return stopWatch.Elapsed;
        }
        /// <summary>Verilen <see cref="MemberInfo"/> nesnesine tanımlanmış olan <see cref="DescriptionAttribute"/> bilgisini döndürür. Eğer attribute yoksa veya hata oluşursa boş string (&quot;&quot;) döner.</summary>
        public static string GetDescription(this MemberInfo memberInfo)
        {
            Guard.ThrowIfNull(memberInfo, nameof(memberInfo));
            try
            {
                var attr = memberInfo.GetCustomAttribute<DescriptionAttribute>();
                return attr == null ? "" : attr.Description.ToStringOrEmpty();
            }
            catch { return ""; }
        }
        /// <summary>Verilen <see cref="MemberInfo"/> nesnesine tanımlanmış olan <see cref="DisplayAttribute"/> bilgisini döndürür. Eğer attribute yoksa veya hata oluşursa boş string (&quot;&quot;) döner.</summary>
        public static string GetDisplayName(this MemberInfo memberInfo)
        {
            Guard.ThrowIfNull(memberInfo, nameof(memberInfo));
            try
            {
                var attr = memberInfo.GetCustomAttribute<DisplayAttribute>();
                return attr == null ? "" : attr.GetName().ToStringOrEmpty();
            }
            catch { return ""; }
        }
        /// <summary>Verilen assembly içerisinde bulunan ve <paramref name="typeInterface"/> arayüzünü uygulayan veya <paramref name="typeBaseclass"/> sınıfından türeyen tüm repository sınıflarını otomatik olarak tarar ve bağımlılık enjeksiyonuna Scoped yaşam süresi ile ekler. Bu sayede her repository için manuel olarak AddScoped tanımı yapmaya gerek kalmaz.</summary>
        /// <param name="services">Bağımlılık enjeksiyon konteyneri</param>
        /// <param name="assembly">Repository sınıflarının bulunduğu assembly</param>
        /// <param name="typeInterface">Repository arayüzünün tipi (örneğin, typeof(IRepository&lt;&gt;))</param>
        /// <param name="typeBaseclass">Repository> sınıflarının türediği temel sınıfın tipi (örneğin, typeof(BaseRepository&lt;&gt;))</param>
        /// <returns>Güncellenmiş IServiceCollection nesnesi</returns>
        public static IServiceCollection AddScopedRange(this IServiceCollection services, Assembly assembly, Type typeInterface, Type typeBaseclass)
        {
            Guard.ThrowIfNull(services, nameof(services));
            Guard.ThrowIfNull(assembly, nameof(assembly));
            Guard.ThrowIfNull(typeInterface, nameof(typeInterface));
            Guard.ThrowIfNull(typeBaseclass, nameof(typeBaseclass));
            var types = assembly.GetTypes().Where(x => !x.IsAbstract && !x.IsInterface && x.IsSubclassOfOpenGeneric(typeBaseclass)).ToArray();
            foreach (var implementation in types)
            {
                var interfaces = implementation.GetInterfaces().Where(x => x.IsImplementsOpenGenericInterface(typeInterface)).ToArray();
                foreach (var service in interfaces) { services.AddScoped(service, implementation); }
            }
            return services;
        }
        /// <summary>Verilen assembly içerisinde bulunan ve <see cref="IBaseService{TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto}"/> arayüzünü uygulayan veya <see cref="BaseService{TContext, TEntity, TEntityDto, TEntityListDto, TSearchDto}"/> sınıfından türeyen tüm servis sınıflarını otomatik olarak tarar ve bağımlılık enjeksiyonuna Scoped yaşam süresi ile ekler. Bu sayede her servis için manuel olarak AddScoped tanımı yapmaya gerek kalmaz.</summary>
        public static IServiceCollection AddScopedRangeBaseService(this IServiceCollection services, Assembly assembly)
        {
            services.AddScopedRange(assembly, typeof(IBaseService<,,,,>), typeof(BaseService<,,,,>));
            return services;
        }
        /// <summary>Modeldeki <see cref="ISoftDelete"/> arayüzünü uygulayan tüm entity tiplerine global sorgu filtresi ekleyerek, <c>IsDeleted = true</c> olan (soft delete edilmiş) kayıtların sorgularda varsayılan olarak gelmesini engeller.</summary>
        /// <remarks>Bu filtre, yalnızca <see cref="ISoftDelete"/> implement eden entity&#39;lere uygulanır. Soft delete edilmiş kayıtları da getirmek gerektiğinde EF Core tarafında <c>IgnoreQueryFilters()</c> kullanılabilir.</remarks>
        /// <param name="modelBuilder">EF Core modelini yapılandırmak için kullanılan <see cref="ModelBuilder"/>.</param>
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
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> cacheSanitize = new();
        /// <summary><paramref name="sanitizer"/> değeri kullanılarak, <paramref name="entity"/> nesnesinin tüm string türündeki özelliklerini temizler (sanitize eder). Temizleme işlemi sırasında, <see cref="SkipSanitizeAttribute"/> ile işaretlenmiş özellikler atlanır. Bu metod, özellikle kullanıcı tarafından sağlanan verilerin güvenliğini sağlamak ve potansiyel XSS saldırılarını önlemek için kullanılabilir.</summary>
        public static void SanitizeEntity(this IHtmlSanitizer sanitizer, object entity)
        {
            Guard.ThrowIfNull(sanitizer, nameof(sanitizer));
            Guard.ThrowIfNull(entity, nameof(entity));
            var props = cacheSanitize.GetOrAdd(entity.GetType(), x => x.GetProperties().Where(y => y.PropertyType == typeof(string) && !y.IsSkipSanitize() && y.IsMapped()).ToArray());
            foreach (var prop in props)
            {
                var value = prop.GetValue(entity).ToStringOrEmpty();
                if (value == "") { prop.SetValue(entity, default(string)); }
                else { prop.SetValue(entity, sanitizer.Sanitize(value).ParseOrDefault<string>()); }
            }
        }
    }
}