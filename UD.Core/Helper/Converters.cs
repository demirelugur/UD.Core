namespace UD.Core.Helper
{
    using Microsoft.Data.SqlClient;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Security.Cryptography;
    using System.Text;
    using UD.Core.Extensions;
    public sealed class Converters
    {
        /// <summary>Verilen nesneyi JSON formatına dönüştürür. JSON çıktısı None formatında ve bazı özel ayarlarla döner.</summary>
        /// <param name="value">JSON&#39;a dönüştürülecek nesne.</param>
        /// <returns>Nesnenin JSON string formatındaki temsili.</returns>
        public static string ToJSON(object value) => JsonConvert.SerializeObject(value, Formatting.None, GlobalConstants.JsonSerializerSettings);
        /// <summary>Verilen string ifadeyi tersine çevirir. Bu metot, Türkçe karakterler (ğ, ü, ş, ç, ö, ı, İ vb.) dahil olmak üzere tüm Unicode metin öğelerini dikkate alarak çalışır. Standart char tabanlı ters çevirme yöntemlerinden farklı olarak <see cref="StringInfo"/> sınıfını kullanır ve her bir metin öğesini (text element) ayrı değerlendirir.</summary>
        /// <param name="value">Tersine çevrilecek string ifade.</param>
        /// <returns>Ters çevrilmiş string ifade.</returns>
        public static string ToReverse(string value)
        {
            value = value.ToStringOrEmpty();
            if (value == "") { return ""; }
            var si = new StringInfo(value);
            int i, length = si.LengthInTextElements;
            var elements = new string[length];
            for (i = 0; i < length; i++) { elements[i] = si.SubstringByTextElements(i, 1); }
            Array.Reverse(elements);
            return String.Concat(elements);
        }
        /// <summary>Verilen nesneyi <see cref="SHA256"/> hash string formatına dönüştürür. Eğer değer null ise boş string döner.</summary>
        /// <param name="value">Hashlenecek nesne.</param>
        /// <returns>Nesnenin <see cref="SHA256"/> hash string temsili.</returns>
        /// <remarks>Not: MSSQL&#39;deki karşılığı SELECT SUBSTRING([sys].[fn_varbintohexstr](HASHBYTES(&#39;SHA2_256&#39;, &#39;Lorem Ipsum&#39;)), 3, 64) AS HashValue</remarks>
        public static string ToHashSHA256FromObject(object value)
        {
            string source; // SELECT SUBSTRING([sys].[fn_varbintohexstr](HASHBYTES('SHA2_256', 'Lorem Ipsum')), 3, 64)
            if (value == null) { source = ""; }
            else if (value is String _s) { source = _s.Trim(); }
            else { source = ToJSON(value); }
            var r = new List<string>();
            foreach (var item in SHA256.HashData(Encoding.UTF8.GetBytes(source))) { r.Add(item.ToString("x2")); }
            return String.Join("", r);
        }
        /// <summary>Verilen nesneyi, özellik isimlerini ve değerlerini içeren bir sözlüğe dönüştürür. Yalnızca özel sınıf türlerinde çalışır.</summary>
        /// <param name="obj">Dönüştürülecek nesne.</param>
        /// <returns>Nesnenin özellik isimlerini ve değerlerini içeren sözlük.</returns>
        public static Dictionary<string, object> ToDictionaryFromObject(object obj)
        {
            if (obj == null) { return []; }
            if (obj is Dictionary<string, object> _d) { return _d; }
            var t = obj.GetType();
            if (t.IsCustomClass()) { return t.GetProperties().ToDictionary(x => x.Name, x => x.GetValue(obj)); }
            if (Guards.IsEnglishDefaultThreadCurrentUICulture) { throw new Exception($"The type of {nameof(obj)} is not in a suitable format!"); }
            throw new Exception($"{nameof(obj)} türü uygun biçimde değildir!");
        }
        /// <summary>Verilen nesneyi SQL parametrelerine dönüştürür. Eğer nesne <see cref="SqlParameter"/> türünde ise doğrudan SQL parametreleri olarak döner. Özel sınıf türlerinde çalışır ve özellik isimlerine göre SQL parametrelerini oluşturur.<para><paramref name="obj"/> için tanımlanan nesneler: SqlParameter, IEnumerable&lt;SqlParameter&gt;, IDictionary&lt;string, object&gt;, AnonymousObjectClass</para></summary>
        /// <param name="obj">Dönüştürülecek nesne.</param>
        /// <returns>Nesneyi temsil eden SQL parametrelerinin dizisi.</returns>
        public static SqlParameter[] ToSqlParameterFromObject(object obj)
        {
            if (obj == null) { return []; }
            if (obj is SqlParameter _sp) { return [_sp]; }
            if (obj is IEnumerable<SqlParameter> _sps) { return _sps.ToArray(); }
            return (obj is IDictionary<string, object> _dic ? _dic : ToDictionaryFromObject(obj)).Select(x => new SqlParameter
            {
                ParameterName = x.Key,
                Value = x.Value ?? DBNull.Value
            }).ToArray();
        }
        /// <summary>Verilen nesneyi DateTime tipine dönüştürür ve isteğe bağlı bir zaman değeri ekler.<para><paramref name="obj"/> için tanımlanan nesneler: DateTime, DateTimeOffset, DateOnly, Int64, String(DateTime, DateTimeOffset, DateOnly, Int64 türlerine uygun biçimde olmalı)</para></summary>
        /// <param name="obj">Dönüştürülecek nesne.</param>
        /// <param name="timeonly">Zaman bilgisi (isteğe bağlı). <paramref name="obj"/> değeri türü DateOnly iken girilecek değer anlamlıdır</param>
        /// <returns>DateTime değeri.</returns>
        public static DateTime ToDateTimeFromObject(object obj, TimeOnly? timeonly)
        {
            if (obj is DateTime _dt) { return _dt; }
            if (obj is DateTimeOffset _dto) { return _dto.DateTime; }
            if (obj is DateOnly _do) { return _do.ToDateTime(timeonly ?? default); }
            if (obj is (Byte or Int16 or Int32 or Int64)) { return new(obj.ToLong()); }
            if (obj is String _s)
            {
                if (DateTime.TryParse(_s, out _dt)) { return _dt; }
                if (DateTimeOffset.TryParse(_s, out _dto)) { return _dto.DateTime; }
                if (DateOnly.TryParse(_s, out _do)) { return _do.ToDateTime(timeonly ?? default); }
                if (Int64.TryParse(_s, out long _ticks)) { return new(_ticks); }
            }
            return default;
        }
        /// <summary>SQL Server&#39;ın sistem tür kimliğini <c>([system_type_id])</c> <see cref="SqlDbType"/> enum değerine dönüştürür.</summary>
        /// <param name="systemTypeId">SQL Server [sys].[types] tablosundaki [system_type_id] değeri.</param>
        /// <returns>Eşleşen <see cref="SqlDbType"/> enum değeri.</returns>
        /// <exception cref="NotSupportedException">Geçersiz veya desteklenmeyen bir sistem tür kimliği verildiğinde fırlatılır.</exception>
        public static SqlDbType ToSqlDbTypeFromSystemTypeID(int systemTypeId)
        {
            return systemTypeId switch
            {
                34 => SqlDbType.Image,
                35 => SqlDbType.Text,
                36 => SqlDbType.UniqueIdentifier,
                40 => SqlDbType.Date,
                41 => SqlDbType.Time,
                42 => SqlDbType.DateTime2,
                43 => SqlDbType.DateTimeOffset,
                48 => SqlDbType.TinyInt,
                52 => SqlDbType.SmallInt,
                56 => SqlDbType.Int,
                58 => SqlDbType.SmallDateTime,
                59 => SqlDbType.Real,
                60 => SqlDbType.Money,
                61 => SqlDbType.DateTime,
                62 => SqlDbType.Float,
                99 => SqlDbType.NText,
                104 => SqlDbType.Bit,
                106 => SqlDbType.Decimal,
                122 => SqlDbType.SmallMoney,
                127 => SqlDbType.BigInt,
                165 => SqlDbType.VarBinary,
                167 => SqlDbType.VarChar,
                173 => SqlDbType.Binary,
                175 => SqlDbType.Char,
                189 => SqlDbType.Timestamp,
                231 => SqlDbType.NVarChar,
                239 => SqlDbType.NChar,
                241 => SqlDbType.Xml,
                _ => throw new NotSupportedException(Guards.IsEnglishDefaultThreadCurrentUICulture ? $"Invalid or unsupported {nameof(systemTypeId)}: {systemTypeId}" : $"Geçersiz veya desteklenmeyen {nameof(systemTypeId)}: {systemTypeId}"),
            };
        }
        /// <summary>Verilen bir data URI string&#39;ini binary veriye ve MIME tipine dönüştürür. <see cref="IOExtensions.ToBase64StringFromBinary(byte[], string)"/> işleminin tersi </summary>
        /// <param name="dataUri">Dönüştürülecek data URI string&#39;i. Biçim: &quot;data:[MIME-type];base64,[base64-encoded-data]&quot;</param>
        /// <returns>Binary veri (byte[]) ve MIME tipini içeren bir tuple döner.</returns>
        /// <exception cref="ArgumentException">Geçersiz data URI biçimi veya eksik MIME tipi/base64 verisi durumunda fırlatılır.</exception>
        /// <exception cref="ArgumentException">Desteklenmeyen dil belirtildiğinde fırlatılır.</exception>
        public static (byte[] bytes, string mimeType) ToBinaryFromBase64String(string dataUri)
        {
            dataUri = dataUri.ToStringOrEmpty();
            if (dataUri == "" || !dataUri.StartsWith("data:")) { throw new ArgumentException(Guards.IsEnglishDefaultThreadCurrentUICulture ? "Invalid data URI format." : "Geçersiz veri URI formatı."); }
            var parts = dataUri.Substring(5).Split([";base64,"], StringSplitOptions.None);
            if (parts.Length != 2) { throw new ArgumentException(Guards.IsEnglishDefaultThreadCurrentUICulture ? "Invalid data URI format: MIME type or base64 data is missing." : "Geçersiz veri URI formatı: MIME tipi veya base64 verisi eksik."); }
            return (Convert.FromBase64String(parts[1]), parts[0]);
        }
        /// <summary>Bir değeri belirtilen türe dönüştürür. Eğer değer null ise ve tip nullable ise null döner. Enum türlerini destekler ve enum değerlerini ilgili türe dönüştürür.</summary>
        /// <param name="value">Dönüştürülecek değer</param>
        /// <param name="type">Dönüştürülecek hedef tür</param>
        /// <returns>Dönüştürülmüş değer</returns>
        public static object ChangeType(object value, Type type)
        {
            var t = Validators.TryTypeIsNullable(type, out Type _genericBaseType);
            if (value == null)
            {
                if (t) { return null; }
                if (Guards.IsEnglishDefaultThreadCurrentUICulture) { throw new ArgumentException("Value cannot be null for a non-nullable type!"); }
                throw new ArgumentException("Null değer alamayan bir tür için değer null olamaz!");
            }
            if (_genericBaseType.IsEnum) { return Enum.ToObject(_genericBaseType, value); }
            return Convert.ChangeType(value, t ? Nullable.GetUnderlyingType(type) : _genericBaseType);
        }
        /// <summary><paramref name="value"/> değerini <typeparamref name="T"/> türüne dönüştürür.</summary>
        /// <typeparam name="T">Dönüştürülecek hedef tür</typeparam>
        /// <param name="value">Dönüştürülecek değer</param>
        /// <returns><typeparamref name="T"/> türüne dönüştürülmüş değer</returns>
        public static T ChangeType<T>(object value) => (T)ChangeType(value, typeof(T));
        /// <summary><paramref name="value"/> değerini belirtilen <paramref name="type"/> türüne dönüştürmeye çalışır. Dönüştürme işlemi başarısız olursa veya değer null ise, nullable türler için null, nullable olmayan türler için default değer döner. Enum türlerini destekler ve enum değerlerini ilgili türe dönüştürür. Ayrıca bool, DateOnly, Uri, MailAddress ve IPAddress türleri için özel dönüşüm mantığı içerir.</summary>
        public static object ParseOrDefault(string value, Type type)
        {
            var pd = parseOrDefault(value, type);
            if (pd.value == null) { return null; }
            try { return Convert.ChangeType(pd.value, pd.genericBaseType); }
            catch { return null; }
        }
        private static (object value, Type genericBaseType) parseOrDefault(string value, Type propertyType)
        {
            try
            {
                value = value.ToStringOrEmpty();
                if (value == "") { return (default, default); }
                _ = Validators.TryTypeIsNullable(propertyType, out Type _genericBaseType);
                if (_genericBaseType.IsEnum)
                {
                    if (Enum.TryParse(_genericBaseType, value, true, out object _enum) && Enum.IsDefined(_genericBaseType, _enum)) { return (_enum, _genericBaseType); }
                    return (default, _genericBaseType);
                }
                if (_genericBaseType == typeof(bool))
                {
                    if (value == "0") { return (false, _genericBaseType); }
                    if (value == "1") { return (true, _genericBaseType); }
                    if (Boolean.TryParse(value, out bool _bo)) { return (_bo, _genericBaseType); }
                    return (default, _genericBaseType);
                }
                if (_genericBaseType == typeof(DateOnly))
                {
                    if (DateOnly.TryParse(value, out DateOnly _da)) { return (_da, _genericBaseType); }
                    var date = value.ParseOrDefault<DateTime?>();
                    if (date.HasValue) { return (date.Value.ToDateOnly(), _genericBaseType); }
                    return (default, _genericBaseType);
                }
                if (_genericBaseType == typeof(TimeSpan))
                {
                    if (TimeSpan.TryParse(value, out TimeSpan _ts)) { return (_ts, _genericBaseType); }
                    return (default, _genericBaseType);
                }
                if (_genericBaseType == typeof(TimeOnly))
                {
                    if (TimeOnly.TryParse(value, out TimeOnly _to)) { return (_to, _genericBaseType); }
                    var ts = value.ParseOrDefault<TimeSpan?>();
                    if (ts.HasValue) { return (ts.Value.ToTimeOnly(), _genericBaseType); }
                    return (default, _genericBaseType);
                }
                if (_genericBaseType == typeof(Uri))
                {
                    if (Validators.TryUri(value, out Uri _u)) { return (_u, _genericBaseType); }
                    return (default, _genericBaseType);
                }
                if (_genericBaseType == typeof(MailAddress))
                {
                    if (Validators.TryMailAddress(value, out MailAddress _ma)) { return (_ma, _genericBaseType); }
                    return (default, _genericBaseType);
                }
                if (_genericBaseType == typeof(IPAddress))
                {
                    if (IPAddress.TryParse(value, out IPAddress _ip)) { return (_ip, _genericBaseType); }
                    return (default, _genericBaseType);
                }
                if (value.IndexOf('.') > -1 && _genericBaseType.Includes(typeof(float), typeof(double), typeof(decimal))) { value = value.Replace(".", ",", StringComparison.InvariantCulture); }
                return (TypeDescriptor.GetConverter(propertyType).ConvertFrom(value), _genericBaseType);
            }
            catch { return (default, default); }
        }
    }
}