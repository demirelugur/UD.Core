namespace UD.Core.Helper.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UD.Core.Extensions;
    using UD.Core.Helper.Results;
    public class BaseEnumCache<TEnum> where TEnum : struct, Enum
    {
        private static readonly Type type = typeof(TEnum);
        public static readonly Type UnderlyingType = type.GetEnumUnderlyingType();
        public static readonly TEnum[] EnumArray = (TEnum[])Enum.GetValues(type);
        public static readonly EnumResult[] EnumArrayDetail = type.ToEnumResultArray();
        public static readonly Dictionary<string, long> ToDictionaryFromEnum = type.ToDictionaryFromEnum();
        public static bool IsDefined(object value) => Enum.IsDefined(type, value);
        public static TEnum[] FlagEnumArray(TEnum flagValue) => EnumArray.Where(x => flagValue.HasFlag(x)).ToArray();
    }
}