namespace UD.Core.Enums
{
    using System.ComponentModel;
    using UD.Core.Extensions;
    using UD.Core.Helper.Configuration;
    using UD.Core.Helper.Validation;
    using static UD.Core.Helper.OrtakTools;
    public sealed class CDayOfWeekTR : BaseEnumCache<CDayOfWeekTR.DayOfWeekTR>
    {
        public enum DayOfWeekTR : byte
        {
            [Description("Pazar")]
            pzr = 0,
            [Description("Pazartesi")]
            pzts,
            [Description("Salı")]
            sali,
            [Description("Çarşamba")]
            car,
            [Description("Perşembe")]
            per,
            [Description("Cuma")]
            cuma,
            [Description("Cumartesi")]
            cmrts
        }
        public static string GetDescriptionLocalizationValue(DayOfWeekTR value, string dil)
        {
            Guard.UnSupportLanguage(dil, nameof(dil));
            if (dil == "en")
            {
                return value switch
                {
                    DayOfWeekTR.pzr => "Sunday",
                    DayOfWeekTR.pzts => "Monday",
                    DayOfWeekTR.sali => "Tuesday",
                    DayOfWeekTR.car => "Wednesday",
                    DayOfWeekTR.per => "Thursday",
                    DayOfWeekTR.cuma => "Friday",
                    DayOfWeekTR.cmrts => "Saturday",
                    _ => throw Utilities.ThrowNotSupportedForEnum<DayOfWeekTR>(),
                };
            }
            return value.GetDescriptionFromEnum();
        }
        public static bool IsWeekDays(DayOfWeekTR value) => value.Includes(DayOfWeekTR.pzts, DayOfWeekTR.sali, DayOfWeekTR.car, DayOfWeekTR.per, DayOfWeekTR.cuma);
    }
}