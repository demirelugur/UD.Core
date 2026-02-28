namespace UD.Core.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UD.Core.Extensions;
    using UD.Core.Helper.Results;
    using static UD.Core.Helper.OrtakTools;
    public class BaseEnumCache<TEnum> where TEnum : Enum
    {
        private static readonly Type _type = typeof(TEnum);
        public static readonly Type UnderlyingType = _type.GetEnumUnderlyingType();
        public static readonly TEnum[] EnumArray = (TEnum[])Enum.GetValues(_type);
        public static readonly EnumResult[] EnumArrayDetail = _type.ToEnumArray();
        public static readonly Dictionary<string, long> ToDictionaryFromEnum = _to.ToDictionaryFromEnum<TEnum>();
        public static bool IsDefined(object value) => Enum.IsDefined(_type, value);
        public static TEnum[] FlagEnumArray(TEnum flagvalue) => EnumArray.Where(x => flagvalue.HasFlag(x)).ToArray();
    }
}