namespace UD.Core.Enums
{
    using System.ComponentModel;
    using UD.Core.Base;
    using UD.Core.Extensions;
    using UD.Core.Helper;
    using static UD.Core.Helper.OrtakTools;
    public sealed class CDayOfWeekTR : BaseEnum<CDayOfWeekTR.DayOfWeekTR>
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
                switch (value)
                {
                    case DayOfWeekTR.pzr: return "Sunday";
                    case DayOfWeekTR.pzts: return "Monday";
                    case DayOfWeekTR.sali: return "Tuesday";
                    case DayOfWeekTR.car: return "Wednesday";
                    case DayOfWeekTR.per: return "Thursday";
                    case DayOfWeekTR.cuma: return "Friday";
                    case DayOfWeekTR.cmrts: return "Saturday";
                    default: throw _other.ThrowNotSupportedForEnum<DayOfWeekTR>();
                }
            }
            return value.GetDescriptionFromEnum();
        }
        public static bool IsWeekDays(DayOfWeekTR value) => value.Includes(DayOfWeekTR.pzts, DayOfWeekTR.sali, DayOfWeekTR.car, DayOfWeekTR.per, DayOfWeekTR.cuma);
    }
}