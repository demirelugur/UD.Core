namespace UD.Core.Extensions.Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Dynamic;
    using System.Globalization;
    using UD.Core.Helper.Validation;
    public static class ObjectExtensions
    {
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
        public static byte ToByte(this object value, byte defaultValue = 0)
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
            IDictionary<string, object> e = new ExpandoObject();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType())) { e.Add(property.Name, property.GetValue(value)); }
            return e as ExpandoObject;
        }
        /// <summary>Verilen <paramref name="value"/> değerini <typeparamref name="TEnum"/> türüne çevirmeyi dener ve başarısız olursa varsayılan değer döndürür. Sayısal tiplerde (<see cref="Byte"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>) ilgili enum değeri <see cref="Enum.ToObject(Type, object)"/> ile oluşturulmaya çalışılır. String değerlerde önce sayısal parse denenir (Int64), değilse enum adı olarak (büyük/küçük harf duyarsız) parse edilir.</summary>
        /// <typeparam name="TEnum">Hedef enum türü.</typeparam>
        /// <param name="value">Enum&#39;a dönüştürülecek değer.</param>
        /// <returns>Dönüştürülen enum değeri; dönüşüm başarısızsa <c>default</c>.</returns>
        public static TEnum? TryToEnum<TEnum>(this object value) where TEnum : struct, Enum
        {
            if (value is TEnum _enum) { return _enum; }
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
            return default;
        }
    }
}