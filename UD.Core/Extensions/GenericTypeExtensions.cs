namespace UD.Core.Extensions
{
    using System;
    public static class GenericTypeExtensions
    {
        /// <summary>Verilen değeri varsayılan (default) değerine eşit ise null döner; aksi halde değeri döner.</summary>
        /// <typeparam name="TKey">Değer türü.</typeparam>
        /// <param name="value">Değer.</param>
        /// <returns>Null veya verilen değeri döner.</returns>
        public static TKey? NullOrDefault<TKey>(this TKey value) where TKey : struct
        {
            if (EqualityComparer<TKey>.Default.Equals(value, default(TKey))) { return null; }
            return value;
        }
        /// <summary>Nullable türde verilen değeri varsayılan (default) değere eşit ise null döner; aksi halde değeri döner.</summary>
        /// <typeparam name="TKey">Değer türü.</typeparam>
        /// <param name="value">Nullable değer.</param>
        /// <returns>Null veya verilen değeri döner.</returns>
        public static TKey? NullOrDefault<TKey>(this TKey? value) where TKey : struct => (value.HasValue ? value.Value.NullOrDefault() : null);
        /// <summary>Verilen değerin belirtilen değerler arasında olup olmadığını kontrol eder.</summary>
        /// <typeparam name="T">Değer türü.</typeparam>
        /// <param name="value">Değer.</param>
        /// <param name="values">Kontrol edilecek değerler.</param>
        /// <returns>True, eğer değer belirtilen değerler arasında ise; aksi halde false döner.</returns>
        public static bool Includes<T>(this T value, params T[] values) => (values ?? Array.Empty<T>()).Contains(value);
        /// <summary>Verilen enum değerinin açıklamasını döner.</summary>
        /// <typeparam name="TEnum">Enum türü.</typeparam>
        /// <param name="value">Enum değeri.</param>
        /// <returns>Enum açıklaması; açıklama yoksa boş dize döner.</returns>
        public static string GetDescriptionFromEnum<TEnum>(this TEnum value) where TEnum : Enum
        {
            var _t = typeof(TEnum);
            try { return _t.GetField(Enum.GetName(_t, value)).GetDescription(); }
            catch { return ""; }
        }
        /// <summary>Tekil bir değeri enumerable (koleksiyon) olarak döner.</summary>
        /// <typeparam name="T">Değer türü.</typeparam>
        /// <param name="value">Kaynak değer.</param>
        /// <returns>Koleksiyon.</returns>
        public static IEnumerable<T> ToEnumerable<T>(this T value)
        {
            yield return value;
        }
    }
}