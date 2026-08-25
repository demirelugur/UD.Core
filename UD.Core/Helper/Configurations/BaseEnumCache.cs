namespace UD.Core.Helper.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UD.Core.Extensions;
    using UD.Core.Helper.Results;
    public class BaseEnumCache<TEnum> where TEnum : struct, Enum
    {
        private static readonly Type _type = typeof(TEnum);
        public static readonly Type UnderlyingType = _type.GetEnumUnderlyingType();
        public static readonly TEnum[] EnumArray = (TEnum[])Enum.GetValues(_type);
        public static readonly EnumResult[] EnumArrayDetail = _type.ToEnumResultArray();
        public static readonly Dictionary<string, long> ToDictionaryFromEnum = _type.ToDictionaryFromEnum();
        public static bool IsDefined(object value) => Enum.IsDefined(_type, value);
        public static TEnum[] FlagEnumArray(TEnum flagValue) => EnumArray.Where(x => flagValue.HasFlag(x)).ToArray();
    }
}