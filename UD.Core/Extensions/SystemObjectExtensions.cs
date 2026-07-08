namespace UD.Core.Extensions
{
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Dynamic;
    using System.Globalization;
    using System.Web;
    using UD.Core.Helper;
    using UD.Core.Helper.Validations;
    public static class SystemObjectExtensions
    {
        /// <summary>Verilen nesneyi JSON formatına dönüştürür. JSON çıktısı None biçiminde ve bazı özel ayarlarla döner.</summary>
        /// <param name="value">JSON&#39;a dönüştürülecek nesne.</param>
        /// <returns>Nesnenin JSON string biçiminde temsili.</returns>
        public static string ToJSON(this object value) => JsonConvert.SerializeObject(value, Formatting.None, GlobalConstants.JsonSerializerSettings);
        /// <summary>Belirtilen değeri istenen türe (<typeparamref name="TKey"/>) güvenli bir şekilde dönüştürür. Anahtar (<paramref name="key"/>) parametresi verildiğinde, <paramref name="value"/> nesnesini farklı veri kaynaklarından (<see cref="IDataReader"/>, <see cref="IDictionary{String, Object}"/>, <see cref="IFormCollection"/>, <see cref="QueryString"/>) anahtar bazlı olarak okuyup dönüştürür.</summary>
        /// <typeparam name="TKey">Dönüştürülecek hedef tür (int, Guid, bool, DateTime, enum vb.).</typeparam>
        /// <param name="value">Dönüştürülecek ana nesne. <paramref name="key"/> parametresi boş ise direkt bu değer dönüştürülür.</param>
        /// <param name="key">Opsiyonel. Değerin hangi anahtardan okunacağını belirtir.
        /// <para>Desteklenen türler (key parametresi dolu olduğunda):</para>
        /// <list type="bullet">
        ///   <item><see cref="IDataReader"/> - DataReader&#39;dan kolon ismiyle değeri okur. <see cref="DBNull"/> durumunda default döner.</item>
        ///   <item><see cref="IDictionary{String, Object}"/> - Dictionary&#39;den anahtara göre değeri çeker.</item>
        ///   <item><see cref="IFormCollection"/> - Form verilerinden <c><see cref="AspNetCoreExtensions.TryGetStringValue(IFormCollection, string, out string)"/></c> ile okur.</item>
        ///   <item><see cref="QueryString"/> - Query string&#39;i parse edip ilgili değeri alır.</item>
        /// </list>
        /// </param>
        /// <returns>Dönüştürme başarılı ise istenen türde değer, aksi takdirde <typeparamref name="TKey"/> türünün default değeri.</returns>
        /// <remarks>
        /// <para>• <paramref name="key"/> boş veya null ise, <paramref name="value"/> direkt olarak <see cref="Converters.ParseOrDefault(object, Type)"/> metodu ile dönüştürülür.</para>
        /// <para>• <paramref name="key"/> dolu ise <paramref name="value"/> parametresi (<see cref="IDataReader"/>, <see cref="IDictionary{String, Object}"/>, <see cref="IFormCollection"/>, <see cref="QueryString"/>) türlerinden biri olmalıdır. Aksi halde default değer döner.</para>
        /// <para>Bu metod, controller&#39;larda, repository&#39;lerde ve veri dönüşüm katmanlarında database reader, dictionary, form ve query string gibi farklı kaynaklardan gelen verileri tek bir yöntemle güvenli şekilde okumak için idealdir.</para>
        /// </remarks>
        public static TKey ParseOrDefault<TKey>(this object value, string key = "")
        {
            if (key.IsNullOrEmpty())
            {
                var pd = Converters.ParseOrDefault(value, typeof(TKey));
                return pd is TKey _tValue ? _tValue : default;
            }
            if (value is IDataReader _dr)
            {
                try
                {
                    var valueObject = _dr[key];
                    if (valueObject == null || valueObject == DBNull.Value) { return default; }
                    return valueObject.ParseOrDefault<TKey>();
                }
                catch { return default; }
            }
            if (value is IDictionary<string, object> _dic && _dic.TryGetValue(key, out object _dicValue)) { return _dicValue.ParseOrDefault<TKey>(); }
            if (value is IFormCollection _form && _form.TryGetStringValue(key, out string _formValue)) { return _formValue.ParseOrDefault<TKey>(); }
            if (value is QueryString _qs && _qs.HasValue)
            {
                var querydic = HttpUtility.ParseQueryString(_qs.Value);
                if (querydic.AllKeys.Contains(key)) { return querydic[key].ParseOrDefault<TKey>(); }
            }
            return default;
        }
        /// <summary>Nesneyi string değere dönüştürür. Nesne null ise boş string döndürür.</summary>
        /// <param name="value">String&#39;e dönüştürülecek nesne</param>
        /// <param name="provider">Kültüre özgü biçimlendirme bilgisi sağlayan nesne (opsiyonel)</param>
        /// <returns>Nesnenin string değeri (baş ve sondaki boşluklar kırpılmış) veya nesne null ise boş string</returns>
        public static string ToStringOrEmpty(this object value, IFormatProvider? provider = null)
        {
            if (value == null) { return ""; }
            return Convert.ToString(value, provider).Trim();
        }
        /// <summary>Belirtilen nesneyi byte&#39;a dönüştürür. Dönüşüm başarısız olursa varsayılan değeri döner.</summary>
        /// <param name="value">Dönüştürülecek nesne.</param>
        /// <param name="defaultValue">Dönüşüm başarısız olursa dönecek varsayılan byte değeri.</param>
        /// <returns>Dönüştürülmüş byte değeri.</returns>
        public static byte ToByte(this object value, byte defaultValue = Byte.MinValue)
        {
            if (value == null) { return defaultValue; }
            try { return Convert.ToByte(value); }
            catch { return defaultValue; }
        }
        /// <summary>Belirtilen nesneyi short&#39;a dönüştürür. Dönüşüm başarısız olursa varsayılan değeri döner.</summary>
        /// <param name="value">Dönüştürülecek nesne.</param>
        /// <param name="defaultValue">Dönüşüm başarısız olursa dönecek varsayılan short değeri.</param>
        /// <returns>Dönüştürülmüş short değeri.</returns>
        public static short ToInt16(this object value, short defaultValue = 0)
        {
            if (value == null) { return defaultValue; }
            try { return Convert.ToInt16(value); }
            catch { return defaultValue; }
        }
        /// <summary>Belirtilen nesneyi short&#39;a dönüştürür. Dönüşüm başarısız olursa varsayılan değeri döner.</summary>
        /// <param name="value">Dönüştürülecek nesne.</param>
        /// <param name="defaultValue">Dönüşüm başarısız olursa dönecek varsayılan short değeri.</param>
        /// <returns>Dönüştürülmüş short değeri.</returns>
        public static short ToShort(this object value, short defaultValue = 0) => value.ToInt16(defaultValue);
        /// <summary>Belirtilen nesneyi int&#39;e dönüştürür. Dönüşüm başarısız olursa varsayılan değeri döner.</summary>
        /// <param name="value">Dönüştürülecek nesne.</param>
        /// <param name="defaultValue">Dönüşüm başarısız olursa dönecek varsayılan int değeri.</param>
        /// <returns>Dönüştürülmüş int değeri.</returns>
        public static int ToInt32(this object value, int defaultValue = 0)
        {
            if (value == null) { return defaultValue; }
            try { return Convert.ToInt32(value); }
            catch { return defaultValue; }
        }
        /// <summary> Belirtilen nesneyi long&#39;a dönüştürür. Dönüşüm başarısız olursa varsayılan değeri döner.</summary>
        /// <param name="value">Dönüştürülecek nesne.</param>
        /// <param name="defaultValue">Dönüşüm başarısız olursa dönecek varsayılan long değeri.</param>
        /// <returns>Dönüştürülmüş long değeri.</returns>
        public static long ToInt64(this object value, long defaultValue = 0)
        {
            if (value == null) { return defaultValue; }
            try { return Convert.ToInt64(value); }
            catch { return defaultValue; }
        }
        /// <summary>Belirtilen nesneyi long&#39;a dönüştürür. Dönüşüm başarısız olursa varsayılan değeri döner.</summary>
        /// <param name="value">Dönüştürülecek nesne.</param>
        /// <param name="defaultValue">Dönüşüm başarısız olursa dönecek varsayılan long değeri.</param>
        /// <returns>Dönüştürülmüş long değeri.</returns>
        public static long ToLong(this object value, long defaultValue = 0) => value.ToInt64(defaultValue);
        /// <summary>Bir <see cref="object"/> türündeki değeri, <see cref="decimal"/> türüne dönüştürür. Eğer dönüşüm başarısız olursa, belirtilen varsayılan değeri döner.</summary>
        /// <param name="value">Dönüştürülecek nesne değeri.</param>
        /// <param name="defaultValue">Dönüşüm başarısız olduğunda dönecek varsayılan <see cref="decimal"/> değeri. Varsayılan olarak sıfırdır.</param>
        /// <returns>Belirtilen nesnenin <see cref="decimal"/> karşılığını veya dönüşüm başarısızsa varsayılan değeri döner.</returns>
        public static decimal ToDecimal(this object value, decimal defaultValue = Decimal.Zero)
        {
            if (value == null) { return defaultValue; }
            return value.ToStringOrEmpty(CultureInfo.InvariantCulture).ParseOrDefault<decimal?>() ?? defaultValue;
        }
        /// <summary>Bir nesneyi dinamik bir nesneye (<see cref="ExpandoObject"/>) dönüştürür. Dönüştürülen nesne, içindeki tüm özellik adları ve değerleriyle birlikte dinamik bir yapı sunar.</summary>
        /// <param name="value">Dönüştürülecek nesne.</param>
        /// <returns>Dinamik bir nesne olarak temsil edilen <see cref="ExpandoObject"/>.</returns>
        public static dynamic ToDynamic(this object value)
        {
            Guard.ThrowIfNull(value, nameof(value));
            IDictionary<string, object> eo = new ExpandoObject();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType())) { eo.Add(property.Name, property.GetValue(value)); }
            return eo as ExpandoObject;
        }
        /// <summary>Verilen <paramref name="value"/> değerini <typeparamref name="TEnum"/> türüne çevirmeyi dener ve başarısız olursa varsayılan değer döndürür. Sayısal tiplerde (<see cref="Byte"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>) ilgili enum değeri <see cref="Enum.ToObject(Type, object)"/> ile oluşturulmaya çalışılır. String değerlerde önce sayısal parse denenir (Int64), değilse enum adı olarak (büyük/küçük harf duyarsız) parse edilir.</summary>
        /// <typeparam name="TEnum">Hedef enum türü.</typeparam>
        /// <param name="value">Enum&#39;a dönüştürülecek değer.</param>
        /// <returns>Dönüştürülen enum değeri; dönüşüm başarısızsa <c>default</c>.</returns>
        public static TEnum? TryToEnum<TEnum>(this object value) where TEnum : struct, Enum
        {
            if (value != null)
            {
                if (value is TEnum _enum) { return _enum; }
                if (value.GetType().IsEnum) { value = value.ToInt64(); }
                if (value is (Byte or Int16 or Int32 or Int64))
                {
                    try { return (TEnum)Enum.ToObject(typeof(TEnum), value); }
                    catch { }
                }
                if (value is String _s && !_s.IsNullOrEmpty())
                {
                    if (Int64.TryParse(_s, out long _valueLong)) { return _valueLong.TryToEnum<TEnum>(); }
                    if (Enum.TryParse(_s, true, out _enum)) { return _enum; }
                }
            }
            return default;
        }
    }
}