namespace UD.Core.Extensions
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using System.Net.Mail;
    using System.Reflection;
    using System.Web;
    using UD.Core.Helper;
    using UD.Core.Helper.Results;
    using static UD.Core.Helper.OrtakTools;
    public static class OtherExtensions
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
        public static async Task<T> SetCompositeKeyAsync<T, CompositeKey>(this DbContext dbcontext, bool issavechanges, T oldentity, Expression<Func<T, CompositeKey>> compositekey, CompositeKey compositekeynewvalue, string dil, CancellationToken cancellationtoken = default) where T : class, new()
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
            if (_props.Where(x => x.iscompositekey).Count() < 2)
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
                if (issavechanges) { await dbcontext.SaveChangesAsync(cancellationtoken); }
                return _newentity;
            }
            if (dil == "en") { throw new Exception($"The property \"{_c}\" in table \"{_tablename}\" must have either \"{typeof(KeyAttribute).FullName}\" and \"{typeof(DatabaseGeneratedAttribute).FullName}\" specified!"); }
            throw new Exception($"\"{_tablename}\" tablosundaki \"{_c}\" özelliğinde \"{typeof(KeyAttribute).FullName}\" ve \"{typeof(DatabaseGeneratedAttribute).FullName}\" belirtilmelidir!");
        }
        /// <summary>
        /// Verilen e-Posta adresindeki &quot;@&quot; karakterini &quot;[at]&quot; ile değiştirir.
        /// </summary>
        /// <param name="mailaddress">MailAddress nesnesi.</param>
        /// <returns>Dönüştürülmüş e-Posta adresi.</returns>
        /// <exception cref="ArgumentNullException">Verilen e-Posta adresi null ise fırlatılır.</exception>
        public static string ReplaceAT(this MailAddress mailaddress)
        {
            Guard.CheckNull(mailaddress, nameof(mailaddress));
            return mailaddress.Address.Replace("@", "[at]");
        }
        /// <summary>
        /// Verilen validation bağlamında bir özelliğin gerekli olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="validationcontext">ValidationContext nesnesi.</param>
        /// <returns>Gerekli ise <see langword="true"/>, değilse <see langword="false"/> döner.</returns>
        public static bool IsRequiredAttribute(this ValidationContext validationcontext)
        {
            if (validationcontext == null) { return false; }
            var _pi = validationcontext.ObjectInstance.GetType().GetProperty(validationcontext.MemberName);
            if (_pi == null) { return false; }
            return _try.TryCustomAttribute(_pi, out RequiredAttribute _);
        }
        /// <summary>
        /// Verilen doğrulama bağlamına göre, belirtilen özelliğin değerini günceller. Eğer özellik yazılabilir durumdaysa, yeni değer atanır.
        /// </summary>
        /// <param name="validationcontext">Doğrulama işlemi sırasında bağlam bilgilerini içeren nesne.</param>
        /// <param name="value">Güncellenmek istenen yeni değer.</param>
        /// <exception cref="ArgumentNullException">Eğer <paramref name="validationcontext"/> null ise tetiklenir.</exception>
        public static void SetValidatePropertyValue(this ValidationContext validationcontext, object value)
        {
            if (validationcontext == null) { return; }
            var _propertyinfo = validationcontext.ObjectType.GetProperty(validationcontext.MemberName);
            if (_propertyinfo != null && _propertyinfo.CanWrite) { _propertyinfo.SetValue(validationcontext.ObjectInstance, value); }
        }
        /// <summary>
        /// Verilen ifadenin adını alır.
        /// </summary>
        /// <param name="expression">İfade.</param>
        /// <returns>İfadenin adı.</returns>
        /// <exception cref="ArgumentException">İfade geçersiz ise fırlatılır.</exception>
        public static string GetExpressionName(this Expression expression)
        {
            if (expression is LambdaExpression _lambda) { expression = _lambda.Body; }
            var _result = "";
            if (expression is MemberExpression _me) { _result = _me.Member.Name; }
            else if (expression is UnaryExpression _ue)
            {
                if (_ue.Operand is MemberExpression _ume) { _result = _ume.Member.Name; }
                else if (_ue.Operand is MethodCallExpression _umce && _umce.Object is ConstantExpression _uce && _uce.Value != null)
                {
                    if (_uce.Value is MemberInfo _mi) { _result = _mi.Name; }
                    else if (_try.TryGetProperty(_uce.Value, nameof(MemberInfo.Name), out string _name)) { _result = _name; }
                }
            }
            if (_result.IsNullOrEmpty()) { throw new ArgumentException($"\"{expression}\" değeri uyumsuzdur!", nameof(expression)); }
            return _result;
        }
        /// <summary>
        /// Bir Guid&#39;i SQL UNIQUEIDENTIFIER biçimine dönüştürür.
        /// </summary>
        /// <param name="guid">Dönüştürülecek Guid.</param>
        /// <returns>Guid&#39;in SQL biçimindeki string temsilini döner.</returns>
        public static string ToSqlExpressionUniqueIdentifier(this Guid guid) => $"CONVERT(UNIQUEIDENTIFIER, '{guid.ToString().ToUpper()}')";
        /// <summary>
        /// Verilen <see cref="SqlDbType"/> enum değerini, SQL Server sistem tür kimliğine (<c>[system_type_id]</c>) dönüştürür. Bu kimlikler, SQL Server&#39;ın [sys].[types] sistem tablosunda bulunan ve her veri türü için benzersiz olan sayısal değerlerdir.
        /// </summary>
        /// <param name="type">Dönüştürülecek <see cref="SqlDbType"/> enum değeri.</param>
        /// <returns>SQL Server sistem tür kimliği (<c>[system_type_id]</c>) değeri</returns>
        /// <exception cref="NotSupportedException">Desteklenmeyen bir <see cref="SqlDbType"/> değeri verildiğinde fırlatılır.</exception>
        /// <remarks>SELECT [name], [system_type_id] FROM [sys].[types]</remarks>
        public static int ToSystemTypeId(this SqlDbType type)
        {
            switch (type)
            {
                case SqlDbType.Image: return 34;
                case SqlDbType.Text: return 35;
                case SqlDbType.UniqueIdentifier: return 36;
                case SqlDbType.Date: return 40;
                case SqlDbType.Time: return 41;
                case SqlDbType.DateTime2: return 42;
                case SqlDbType.DateTimeOffset: return 43;
                case SqlDbType.TinyInt: return 48;
                case SqlDbType.SmallInt: return 52;
                case SqlDbType.Int: return 56;
                case SqlDbType.SmallDateTime: return 58;
                case SqlDbType.Real: return 59;
                case SqlDbType.Money: return 60;
                case SqlDbType.DateTime: return 61;
                case SqlDbType.Float: return 62;
                case SqlDbType.NText: return 99;
                case SqlDbType.Bit: return 104;
                case SqlDbType.Decimal: return 106;
                case SqlDbType.SmallMoney: return 122;
                case SqlDbType.BigInt: return 127;
                case SqlDbType.VarBinary: return 165;
                case SqlDbType.VarChar: return 167;
                case SqlDbType.Binary: return 173;
                case SqlDbType.Char: return 175;
                case SqlDbType.Timestamp: return 189;
                case SqlDbType.NVarChar: return 231;
                case SqlDbType.NChar: return 239;
                case SqlDbType.Xml: return 241;
                default: throw _other.ThrowNotSupportedForEnum<SqlDbType>();
            }
        }
        /// <summary>
        /// Verilen <see cref="SqlDbType"/> enum değerini, ADO.NET&#39;in genel veri türü olan <see cref="DbType"/>&#39;a dönüştürür. SQL Server&#39;a özgü veri türlerini platform bağımsız <see cref="DbType"/> türlerine çevirir.
        /// </summary>
        /// <param name="type">Dönüştürülecek <see cref="SqlDbType"/> enum değeri.</param>
        /// <returns>Eşdeğer <see cref="DbType"/> enum değeri.</returns>
        /// <exception cref="NotSupportedException">Desteklenmeyen bir <see cref="SqlDbType"/> değeri verildiğinde fırlatılır.</exception>
        public static DbType ToDbType(this SqlDbType type)
        {
            switch (type)
            {
                case SqlDbType.UniqueIdentifier: return DbType.Guid;
                case SqlDbType.Date: return DbType.Date;
                case SqlDbType.Time: return DbType.Time;
                case SqlDbType.DateTime2: return DbType.DateTime2;
                case SqlDbType.DateTimeOffset: return DbType.DateTimeOffset;
                case SqlDbType.TinyInt: return DbType.Byte;
                case SqlDbType.SmallInt: return DbType.Int16;
                case SqlDbType.Int: return DbType.Int32;
                case SqlDbType.Real: return DbType.Single;
                case SqlDbType.Money: return DbType.Currency;
                case SqlDbType.DateTime: return DbType.DateTime;
                case SqlDbType.Float: return DbType.Double;
                case SqlDbType.Bit: return DbType.Boolean;
                case SqlDbType.Decimal: return DbType.Decimal;
                case SqlDbType.BigInt: return DbType.Int64;
                case SqlDbType.VarChar: return DbType.AnsiString;
                case SqlDbType.Binary: return DbType.Binary;
                case SqlDbType.VarBinary: return DbType.Binary;
                case SqlDbType.Char: return DbType.AnsiStringFixedLength;
                case SqlDbType.NVarChar: return DbType.String;
                case SqlDbType.NChar: return DbType.StringFixedLength;
                case SqlDbType.Xml: return DbType.Xml;
                default: throw _other.ThrowNotSupportedForEnum<SqlDbType>();
            }
        }
        /// <summary>
        /// Verilen <see cref="DbType"/> enum değerini, SQL Server&#39;a özgü <see cref="SqlDbType"/> enum değerine dönüştürür. ADO.NET&#39;in genel veri türleri (<see cref="DbType"/>) ile SQL Server&#39;ın özel veri türleri (<see cref="SqlDbType"/>) arasında eşleme yapar.
        /// </summary>
        /// <param name="type">Dönüştürülecek <see cref="DbType"/> enum değeri.</param>
        /// <returns>Eşdeğer <see cref="SqlDbType"/> enum değeri.</returns>
        /// <exception cref="NotSupportedException">Desteklenmeyen bir <see cref="DbType"/> değeri verildiğinde fırlatılır.</exception>
        public static SqlDbType ToSqlDbType(this DbType type)
        {
            switch (type)
            {
                case DbType.Guid: return SqlDbType.UniqueIdentifier;
                case DbType.Date: return SqlDbType.Date;
                case DbType.Time: return SqlDbType.Time;
                case DbType.DateTime2: return SqlDbType.DateTime2;
                case DbType.DateTimeOffset: return SqlDbType.DateTimeOffset;
                case DbType.Byte: return SqlDbType.TinyInt;
                case DbType.Int16: return SqlDbType.SmallInt;
                case DbType.Int32: return SqlDbType.Int;
                case DbType.Single: return SqlDbType.Real;
                case DbType.Currency: return SqlDbType.Money;
                case DbType.DateTime: return SqlDbType.DateTime;
                case DbType.Double: return SqlDbType.Float;
                case DbType.Boolean: return SqlDbType.Bit;
                case DbType.Decimal: return SqlDbType.Decimal;
                case DbType.Int64: return SqlDbType.BigInt;
                case DbType.AnsiString: return SqlDbType.VarChar;
                case DbType.Binary: return SqlDbType.VarBinary;
                case DbType.AnsiStringFixedLength: return SqlDbType.Char;
                case DbType.String: return SqlDbType.NVarChar;
                case DbType.StringFixedLength: return SqlDbType.NChar;
                case DbType.Xml: return SqlDbType.Xml;
                default: throw _other.ThrowNotSupportedForEnum<DbType>();
            }
        }
        /// <summary>
        /// Bir <see cref="JToken"/> nesnesini belirtilen <typeparamref name="TKey"/> türündeki bir diziye dönüştürür. Eğer <see cref="JToken"/> null ise boş bir dizi döner, array türünde ise içindeki değerleri <typeparamref name="TKey"/> türüne çevirip dizi olarak döner. Diğer durumlarda bir istisna fırlatır.
        /// </summary>
        /// <typeparam name="TKey">Dönüştürülecek hedef veri türü.</typeparam>
        /// <param name="jtoken">Dönüştürülecek <see cref="JToken"/> nesnesi.</param>
        /// <returns><typeparamref name="TKey"/> türünden bir dizi.</returns>
        /// <exception cref="NotSupportedException"><see cref="JToken"/> türü null veya array değilse fırlatılır.</exception>
        public static TKey[] ToArrayFromJToken<TKey>(this JToken jtoken)
        {
            if (jtoken == null || jtoken.Type == JTokenType.Null) { return Array.Empty<TKey>(); }
            if (jtoken.Type == JTokenType.Array) { return jtoken.Select(x => x.Value<TKey>()).ToArray(); }
            throw new NotSupportedException($"\"{nameof(jtoken)}\" türü uyumsuzdur!");
        }
        /// <summary>
        /// <see cref="QueryString"/> içindeki belirtilen anahtarı alır ve uygun türde bir değere dönüştürür. Eğer anahtar bulunamazsa veya dönüştürme başarısız olursa, varsayılan değeri döner.
        /// </summary>
        /// <typeparam name="TKey">Dönüştürülecek hedef tür.</typeparam>
        /// <param name="querystring">İçinde sorgu parametrelerini barındıran <see cref="QueryString"/> nesnesi.</param>
        /// <param name="key">Alınacak sorgu parametresinin adı (anahtar).</param>
        /// <returns>Başarılıysa sorgu parametresi uygun türe dönüştürülür, aksi halde varsayılan değer döner.</returns>
        public static TKey ParseOrDefault<TKey>(this QueryString querystring, string key)
        {
            var _querydic = (querystring.HasValue ? HttpUtility.ParseQueryString(querystring.Value) : new NameValueCollection());
            key = key.ToStringOrEmpty();
            if (_querydic.AllKeys.Contains(key)) { return _querydic[key].ParseOrDefault<TKey>(); }
            return default;
        }
        /// <summary>
        /// Belirtilen sütun adını kullanarak <see cref="IDataReader"/> içinden değeri okur ve verilen türde (<typeparamref name="TKey"/>) döndürür. Eğer sütun değeri <c>null</c>, <see cref="DBNull"/> ya da dönüştürülemeyen bir değer içeriyorsa, türün varsayılan değerini (<c>default</c>) geri döner.
        /// </summary>
        /// <typeparam name="TKey">Dönüştürülmek istenen hedef tür.</typeparam>
        /// <param name="reader">Veri kaynağından okuma yapan <see cref="IDataReader"/> nesnesi.</param>
        /// <param name="key">Okunacak sütunun adı.</param>
        /// <returns>Sütun değeri başarıyla dönüştürülebilirse <typeparamref name="TKey"/> tipinde değer, aksi durumda varsayılan değer döner.</returns>
        public static TKey ParseOrDefault<TKey>(this IDataReader reader, string key)
        {
            if (reader == null || key.IsNullOrEmpty()) { return default; }
            try
            {
                var _value = reader[key];
                if (_value == null || _value == DBNull.Value) { return default; }
                return _value.ToString().ParseOrDefault<TKey>();
            }
            catch { return default; }
        }
        /// <summary>
        /// Stopwatch&#39;ı durdurur ve geçen süreyi döner.
        /// </summary>
        /// <param name="stopwatch">Zamanlayıcı nesnesi.</param>
        /// <returns>Durdurulduktan sonra geçen süre.</returns>
        public static TimeSpan StopThenGetElapsed(this Stopwatch stopwatch)
        {
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
        /// <summary>
        /// <paramref name="data"/> verisini önbelleğe ekler ve başarılı işlem sonucunu (<see cref="IslemSonucResult{T}"/>) döndürür.
        /// </summary>
        /// <typeparam name="T">Önbelleğe eklenecek verinin tipi.</typeparam>
        /// <param name="memorycache">Önbellek nesnesi.</param>
        /// <param name="cachekey">Önbelleğe eklenecek değerin anahtarı.</param>
        /// <param name="data">Önbelleğe eklenecek veri.</param>
        /// <param name="timespan">Önbellekte tutulma süresi. Boş bırakılırsa varsayılan olarak 1 dakika kullanılır.</param>
        /// <returns>Başarılı işlem sonucunu temsil eden <see cref="IslemSonucResult{T}"/> nesnesi.</returns>
        public static IslemSonucResult<T> SetCacheAndReturnSuccess<T>(this IMemoryCache memorycache, object cachekey, T data, TimeSpan? timespan = null)
        {
            memorycache.Set(cachekey, data, timespan ?? TimeSpan.FromMinutes(1));
            return new(data, true, default);
        }
        /// <summary>
        /// Verilen <see cref="MemberInfo"/> nesnesine tanımlanmış olan <see cref="DescriptionAttribute"/> bilgisini döndürür. Eğer attribute yoksa veya hata oluşursa boş string (&quot;&quot;) döner.
        /// </summary>
        /// <param name="memberinfo">Üzerinde <see cref="DescriptionAttribute"/> aranacak üye bilgisi (sınıf, property, metod vb.).</param>
        /// <returns><see cref="DescriptionAttribute"/> içindeki açıklama metni, yoksa boş string (&quot;&quot;).</returns>
        public static string GetDescription(this MemberInfo memberinfo)
        {
            if (memberinfo == null) { return ""; }
            try
            {
                var _attr = memberinfo.GetCustomAttribute<DescriptionAttribute>();
                return _attr == null ? "" : _attr.Description.ToStringOrEmpty();
            }
            catch { return ""; }
        }
    }
}