namespace UD.Core.Base
{
    using FaturaBilgileri.DAL.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UD.Core.Extensions;
    using UD.Core.Helper.Results;
    using static UD.Core.Helper.OrtakTools;

    /// <summary>
    /// Enum&#39;lar için temel bir sınıf.
    /// </summary>
    /// <typeparam name="TEnum">Enum türü</typeparam>
    public abstract class BaseEnumCache<TEnum> where TEnum : Enum
    {
        private static readonly Type _type = typeof(TEnum);
        /// <summary>
        /// Enum&#39;un alttaki veri türü.
        /// </summary>
        public static readonly Type UnderlyingType = _type.GetEnumUnderlyingType();
        /// <summary>
        /// Enum türündeki tüm değerleri içeren dizi.
        /// </summary>
        public static readonly TEnum[] EnumArray = (TEnum[])Enum.GetValues(_type);
        /// <summary>
        /// Enum türündeki detayları içeren dizi.
        /// </summary>
        public static readonly EnumResult[] EnumArrayDetail = _type.ToEnumArray();
        /// <summary>
        /// Enum&#39;dan bir sözlük olarak değerleri döndürür.
        /// </summary>
        public static readonly Dictionary<string, long> ToDictionaryFromEnum = _to.ToDictionaryFromEnum<TEnum>();
        /// <summary>
        /// Belirtilen değerin enum&#39;da tanımlı olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="value">Kontrol edilecek değer</param>
        /// <returns>Tanımlı ise <see langword="true"/>, aksi halde <see langword="false"/></returns>
        public static bool IsDefined(object value) => Enum.IsDefined(_type, value);
        /// <summary>
        /// Belirtilen bayrak değerine sahip enum değerlerini döndürür.
        /// </summary>
        /// <param name="flagvalue">Bayrak değeri</param>
        /// <returns>Bayrak değerine sahip enum değerleri</returns>
        public static TEnum[] FlagEnumArray(TEnum flagvalue) => EnumArray.Where(x => flagvalue.HasFlag(x)).ToArray();
    }
}