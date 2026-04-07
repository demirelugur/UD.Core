namespace UD.Core.Extensions
{
    using System;
    public static class GenericTypeExtensions
    {
        /// <summary>Verilen değeri varsayılan (default) değerine eşit ise null döner; aksi halde değeri döner.</summary>
        public static TKey? NullOrDefault<TKey>(this TKey value) where TKey : struct
        {
            if (EqualityComparer<TKey>.Default.Equals(value, default)) { return null; }
            return value;
        }
        /// <summary>Nullable türde verilen değeri varsayılan (default) değere eşit ise null döner; aksi halde değeri döner.</summary>
        public static TKey? NullOrDefault<TKey>(this TKey? value) where TKey : struct => (value.HasValue ? value.Value.NullOrDefault() : null);
        /// <summary>Verilen değerin belirtilen değerler arasında olup olmadığını kontrol eder.</summary>
        public static bool Includes<T>(this T value, params T[] values) => (values ?? []).Contains(value);
        /// <summary>Verilen enum değerinin açıklamasını döner.</summary>
        public static string GetDescriptionFromEnum<TEnum>(this TEnum value) where TEnum : struct, Enum
        {
            var t = typeof(TEnum);
            try { return t.GetField(Enum.GetName(t, value)).GetDescription(); }
            catch { return ""; }
        }
        /// <summary>Verilen enum değerinin görüntülenebilir adını döner. Genellikle kullanıcı arayüzünde gösterilmek üzere kullanılır.</summary>
        public static string GetDisplayNameFromEnum<TEnum>(this TEnum value) where TEnum : struct, Enum
        {
            var t = typeof(TEnum);
            try { return t.GetField(Enum.GetName(t, value)).GetDisplayName(); }
            catch { return ""; }
        }
        /// <summary>Tekil bir değeri enumerable (koleksiyon) olarak döner.</summary>
        public static IEnumerable<T> ToEnumerable<T>(this T value)
        {
            yield return value;
        }
        /// <summary><paramref name="objA"/> ve <paramref name="objB"/> dizilerinin elemanlarının sırasına ve değerlerine göre eşit olup olmadığını kontrol eder. Her iki dizi de null veya boş ise eşit kabul edilir.</summary>
        public static bool IsAbsoluteEqual<T>(this T[] objA, T[] objB)
        {
            objA ??= [];
            objB ??= [];
            if (objA.Length == 0 && objB.Length == 0) { return true; }
            return objA.AsSpan().SequenceEqual(objB.AsSpan());
        }
    }
}