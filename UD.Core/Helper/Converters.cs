namespace UD.Core.Helper
{
    using Microsoft.Data.SqlClient;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using UD.Core.Extensions;
    public sealed class Converters
    {
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
        /// <summary>Verilen nesneyi, özellik isimlerini ve değerlerini içeren bir sözlüğe dönüştürür. Yalnızca özel sınıf türlerinde çalışır.</summary>
        /// <param name="obj">Dönüştürülecek nesne.</param>
        /// <returns>Nesnenin özellik isimlerini ve değerlerini içeren sözlük.</returns>
        public static Dictionary<string, object> ToDictionaryFromObject(object obj)
        {
            if (obj == null) { return []; }
            if (obj is Dictionary<string, object> _d) { return _d; }
            var t = obj.GetType();
            if (t.IsCustomClass()) { return t.GetProperties().ToDictionary(x => x.Name, x => x.GetValue(obj)); }
            if (Checks.IsEnglishCurrentUICulture) { throw new Exception($"The type of {nameof(obj)} is not in a suitable format!"); }
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
        /// <summary>Verilen nesneyi DateTime tipine dönüştürür ve isteğe bağlı bir zaman değeri ekler.<para><paramref name="obj"/> için tanımlanan nesneler: DateTime, DateTimeOffset, DateOnly, Int64, String(DateTime, DateTimeOffset, DateOnly, Int64 türlerine uygun biçimde olmalı), JToken(DateTime türüne uygun biçimde olmalı)</para></summary>
        /// <param name="obj">Dönüştürülecek nesne.</param>
        /// <param name="timeOnly">Zaman bilgisi (isteğe bağlı). <paramref name="obj"/> değeri türü DateOnly iken girilecek değer anlamlıdır</param>
        /// <returns>DateTime değeri.</returns>
        public static DateTime ToDateTimeFromObject(object obj, TimeOnly? timeOnly = null)
        {
            if (obj is DateTime _dt) { return _dt; }
            if (obj is DateTimeOffset _dto) { return _dto.DateTime; }
            if (obj is DateOnly _do) { return _do.ToDateTime(timeOnly ?? default); }
            if (obj is (Byte or Int16 or Int32 or Int64)) { return new(obj.ToLong()); }
            if (obj is JToken _jt && _jt.Type == JTokenType.Date) { return _jt.ToObject<DateTime>(); }
            if (obj is String _s)
            {
                if (DateTime.TryParse(_s, out _dt)) { return _dt; }
                if (DateTimeOffset.TryParse(_s, out _dto)) { return _dto.DateTime; }
                if (DateOnly.TryParse(_s, out _do)) { return _do.ToDateTime(timeOnly ?? default); }
                if (Int64.TryParse(_s, out long _ticks)) { return new(_ticks); }
            }
            return default;
        }
        /// <summary>Verilen bir data URI string&#39;ini binary veriye ve MIME tipine dönüştürür. <see cref="SystemArrayExtensions.ToBase64StringFromBinary(byte[], string)"/> işleminin tersi </summary>
        /// <param name="base64String">Dönüştürülecek data URI string&#39;i. Biçim: &quot;data:[MIME-type];base64,[base64-encoded-data]&quot;</param>
        /// <returns>Binary veri (byte[]) ve MIME tipini içeren bir tuple döner.</returns>
        /// <exception cref="ArgumentException">Geçersiz data URI biçimi veya eksik MIME tipi/base64 verisi durumunda fırlatılır.</exception>
        /// <exception cref="ArgumentException">Desteklenmeyen dil belirtildiğinde fırlatılır.</exception>
        public static (byte[] bytes, string mimeType) ToBinaryFromBase64String(string base64String)
        {
            base64String = base64String.ToStringOrEmpty();
            if (base64String == "" || !base64String.StartsWith("data:")) { throw new ArgumentException(Checks.IsEnglishCurrentUICulture ? "Invalid data URI format." : "Geçersiz veri URI formatı."); }
            var parts = base64String.Substring(5).Split([";base64,"], StringSplitOptions.None);
            if (parts.Length != 2) { throw new ArgumentException(Checks.IsEnglishCurrentUICulture ? "Invalid data URI format: MIME type or base64 data is missing." : "Geçersiz veri URI formatı: MIME tipi veya base64 verisi eksik."); }
            return (Convert.FromBase64String(parts[1]), parts[0]);
        }
        /// <summary>Bir değeri belirtilen türe dönüştürür. Eğer değer null ise ve tip nullable ise null döner. Enum türlerini destekler ve enum değerlerini ilgili türe dönüştürür.</summary>
        /// <param name="value">Dönüştürülecek değer</param>
        /// <param name="type">Dönüştürülecek hedef tür</param>
        /// <returns>Dönüştürülmüş değer</returns>
        public static object ChangeType(object value, Type type)
        {
            var t = TryValidators.TryTypeIsNullable(type, out Type _baseType);
            if (value == null)
            {
                if (t) { return null; }
                if (Checks.IsEnglishCurrentUICulture) { throw new ArgumentException("Value cannot be null for a non-nullable type!"); }
                throw new ArgumentException("Null değer alamayan bir tür için değer null olamaz!");
            }
            if (_baseType.IsEnum) { return Enum.ToObject(_baseType, value); }
            return Convert.ChangeType(value, t ? Nullable.GetUnderlyingType(type) : _baseType);
        }
        /// <summary><paramref name="value"/> değerini <typeparamref name="T"/> türüne dönüştürür.</summary>
        /// <typeparam name="T">Dönüştürülecek hedef tür</typeparam>
        /// <param name="value">Dönüştürülecek değer</param>
        /// <returns><typeparamref name="T"/> türüne dönüştürülmüş değer</returns>
        public static T ChangeType<T>(object value) => (T)ChangeType(value, typeof(T));
        /// <summary><paramref name="value"/> değerini belirtilen <paramref name="type"/> türüne dönüştürmeye çalışır. Dönüştürme işlemi başarısız olursa veya değer null ise, nullable türler için null, nullable olmayan türler için default değer döner. Enum türlerini destekler ve enum değerlerini ilgili türe dönüştürür. Ayrıca bool, DateOnly, Uri, MailAddress ve IPAddress türleri için özel dönüşüm mantığı içerir.</summary>
        public static object ParseOrDefault(object value, Type type)
        {
            var pd = parseOrDefault(value, type);
            if (pd.value == null) { return null; }
            try { return Convert.ChangeType(pd.value, pd.baseType); }
            catch { return null; }
        }
        private static (object value, Type baseType) parseOrDefault(object value, Type propertyType)
        {
            if (value == null) { return (default, default); }
            _ = TryValidators.TryTypeIsNullable(propertyType, out Type _baseType);
            var valueString = value.ToStringOrEmpty();
            if (valueString == "") { return (default, default); }
            if (_baseType.IsEnum)
            {
                if (value.GetType() == _baseType) { return (value, _baseType); }
                if (Enum.TryParse(_baseType, valueString, true, out object _enum)) { return (_enum, _baseType); }
                return (default, _baseType);
            }
            if (_baseType == typeof(bool))
            {
                if (value is bool _bo) { return (_bo, _baseType); }
                if (valueString == "0") { return (false, _baseType); }
                if (valueString == "1") { return (true, _baseType); }
                if (Boolean.TryParse(valueString, out _bo)) { return (_bo, _baseType); }
                return (default, _baseType);
            }
            if (_baseType == typeof(DateOnly))
            {
                if (value is DateOnly _da) { return (_da, _baseType); }
                if (DateOnly.TryParse(valueString, out _da)) { return (_da, _baseType); }
                var date = value.ParseOrDefault<DateTime?>();
                if (date.HasValue) { return (date.Value.ToDateOnly(), _baseType); }
                return (default, _baseType);
            }
            if (_baseType == typeof(TimeSpan))
            {
                if (value is TimeSpan _ts) { return (_ts, _baseType); }
                if (TimeSpan.TryParse(valueString, out _ts)) { return (_ts, _baseType); }
                return (default, _baseType);
            }
            if (_baseType == typeof(TimeOnly))
            {
                if (value is TimeOnly _to) { return (_to, _baseType); }
                if (TimeOnly.TryParse(valueString, out _to)) { return (_to, _baseType); }
                var ts = value.ParseOrDefault<TimeSpan?>();
                if (ts.HasValue) { return (ts.Value.ToTimeOnly(), _baseType); }
                return (default, _baseType);
            }
            if (_baseType == typeof(Uri))
            {
                if (value is Uri _u) { return (_u, _baseType); }
                if (TryValidators.TryUri(valueString, out _u)) { return (_u, _baseType); }
                return (default, _baseType);
            }
            if (_baseType == typeof(MailAddress))
            {
                if (value is MailAddress _ma) { return (_ma, _baseType); }
                if (TryValidators.TryMailAddress(valueString, out _ma)) { return (_ma, _baseType); }
                return (default, _baseType);
            }
            if (_baseType == typeof(IPAddress))
            {
                if (value is IPAddress _ip) { return (_ip, _baseType); }
                if (IPAddress.TryParse(valueString, out _ip)) { return (_ip, _baseType); }
                return (default, _baseType);
            }
            if (valueString.IndexOf('.') > -1 && _baseType.Includes(typeof(float), typeof(double), typeof(decimal))) { valueString = valueString.Replace(".", ",", StringComparison.InvariantCulture); }
            try { return (TypeDescriptor.GetConverter(propertyType).ConvertFrom(valueString), _baseType); }
            catch { return (default, default); }
        }
    }
}