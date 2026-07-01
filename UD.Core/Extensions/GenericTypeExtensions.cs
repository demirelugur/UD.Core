namespace UD.Core.Extensions
{
    using System;
    using UD.Core.Helper;
    public static class GenericTypeExtensions
    {
        /// <summary><paramref name="value"/> değeri için standart bir <see cref="ArgumentOutOfRangeException"/> oluşturur.</summary>
        /// <typeparam name="T">Struct türü.</typeparam>
        /// <param name="value">Geçersiz olduğu tespit edilen struct değeri.</param>
        /// <param name="paramName">Parametre adı.</param>
        /// <returns>Oluşturulan <see cref="ArgumentOutOfRangeException"/> örneği.</returns>
        public static ArgumentOutOfRangeException ArgumentOutOfRange<T>(this T value, string paramName = "") where T : struct => new(paramName.ParseOrDefault<string>(), default, Checks.IsEnglishCurrentUICulture ? $"Invalid value: {value}." : $"Geçersiz değer: {value}.");
        /// <summary><paramref name="value"/> değerinin <paramref name="min"/> ve <paramref name="max"/> değerleri arasında olup olmadığını kontrol eder. Karşılaştırma, <typeparamref name="T"/> türünün <see cref="IComparable{T}"/> arayüzünü uyguladığı varsayılarak yapılır. Değerler arasında eşitlik de dahil edilir, yani <paramref name="value"/> değeri <paramref name="min"/> veya <paramref name="max"/> değerine eşit olabilir.</summary>
        /// <typeparam name="T">Karşılaştırma yapılacak tür.</typeparam>
        /// <param name="value">Kontrol edilecek değer.</param>
        /// <param name="min">Minimum değer.</param>
        /// <param name="max">Maksimum değer.</param>
        /// <returns>Değer belirtilen aralıkta ise true, aksi halde false döner.</returns>
        public static bool Between<T>(this T value, T min, T max) where T : struct, IComparable<T> => (value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0);
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
        /// <summary><paramref name="value"/> değerinin <paramref name="values"/> dizisi içinde bulunup bulunmadığını kontrol eder. Eğer <paramref name="values"/> null ise, boş bir dizi olarak değerlendirilir ve sonuç her zaman false olur. Bu yöntem, belirli bir değerin bir dizi içinde olup olmadığını kolayca kontrol etmek için kullanılır.</summary>
        /// <typeparam name="T">Kontrol edilecek değerlerin türü.</typeparam>
        /// <param name="value">Kontrol edilecek değer.</param>
        /// <param name="values">Değerlerin aranacağı dizi.</param>
        /// <returns>Değer dizide bulunuyorsa true, aksi halde false döner.</returns>
        public static bool Includes<T>(this T value, params T[] values) => (values ?? []).Contains(value);
        /// <summary><paramref name="objA"/> ve <paramref name="objB"/> dizilerinin elemanlarının sırasına ve değerlerine göre eşit olup olmadığını kontrol eder. Her iki dizi de null veya boş ise eşit kabul edilir.</summary>
        public static bool IsAbsoluteEqual<T>(this T[] objA, T[] objB) => objA.IsSequenceEqual(objB);
        /// <summary><paramref name="objA"/> ve <paramref name="objB"/> dizilerinin elemanlarının sırasına ve değerlerine göre eşit olup olmadığını kontrol eder. Her iki dizi de null veya boş ise eşit kabul edilir.</summary>
        public static bool IsSequenceEqual<T>(this T[] objA, T[] objB)
        {
            objA ??= [];
            objB ??= [];
            if (objA.Length == 0 && objB.Length == 0) { return true; }
            return objA.AsSpan().SequenceEqual(objB.AsSpan());
        }
        /// <summary><paramref name="value"/> değerinin varsayılan değere eşit olup olmadığını kontrol eder. Eğer <paramref name="value"/> varsayılan değere eşitse, null döner; aksi halde değerin kendisini döner. Bu yöntem, nullable türler için kullanışlıdır ve varsayılan değerleri null olarak değerlendirmek istediğiniz durumlarda kullanılabilir.</summary>
        /// <typeparam name="TKey">Kontrol edilecek değer türü.</typeparam>
        /// <param name="value">Kontrol edilecek değer.</param>
        /// <returns>Değer varsayılan değere eşitse null, aksi halde değerin kendisi döner.</returns>
        public static TKey? NullOrDefault<TKey>(this TKey value) where TKey : struct
        {
            if (EqualityComparer<TKey>.Default.Equals(value, default)) { return null; }
            return value;
        }
        /// <summary><paramref name="value"/> değerinin null olup olmadığını kontrol eder. Eğer null ise, null döner; aksi halde değerin kendisini döner. Bu yöntem, nullable türler için kullanışlıdır ve null değerleri varsayılan değerlerle karşılaştırmak yerine doğrudan null olarak değerlendirmek istediğiniz durumlarda kullanılabilir.</summary>
        /// <typeparam name="TKey">Kontrol edilecek değer türü.</typeparam>
        /// <param name="value">Kontrol edilecek değer.</param>
        /// <returns>Değer null ise null, aksi halde değerin kendisi döner.</returns>
        public static TKey? NullOrDefault<TKey>(this TKey? value) where TKey : struct => (value.HasValue ? value.Value.NullOrDefault() : null);
        /// <summary>Tekil bir değeri enumerable (koleksiyon) olarak döner.</summary>
        public static IEnumerable<T> ToEnumerable<T>(this T value)
        {
            yield return value;
        }
    }
}