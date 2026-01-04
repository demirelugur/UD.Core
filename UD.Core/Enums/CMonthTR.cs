namespace UD.Core.Enums
{
    using System.ComponentModel;
    using UD.Core.Base;
    using UD.Core.Extensions;
    using UD.Core.Helper;
    using static UD.Core.Helper.OrtakTools;
    public sealed class CMonthTR : BaseEnumCache<CMonthTR.MonthTR>
    {
        public enum MonthTR : byte
        {
            [Description("Ocak")]
            oca = 1,
            [Description("Şubat")]
            sub,
            [Description("Mart")]
            mrt,
            [Description("Nisan")]
            nis,
            [Description("Mayıs")]
            may,
            [Description("Haziran")]
            haz,
            [Description("Temmuz")]
            tem,
            [Description("Ağustos")]
            agu,
            [Description("Eylül")]
            eyu,
            [Description("Ekim")]
            eki,
            [Description("Kasım")]
            kas,
            [Description("Aralık")]
            ara
        }
        public static string GetDescriptionLocalizationValue(MonthTR value, string dil)
        {
            Guard.UnSupportLanguage(dil, nameof(dil));
            if (dil == "en")
            {
                switch (value)
                {
                    case MonthTR.oca: return "January";
                    case MonthTR.sub: return "February";
                    case MonthTR.mrt: return "March";
                    case MonthTR.nis: return "April";
                    case MonthTR.may: return "May";
                    case MonthTR.haz: return "June";
                    case MonthTR.tem: return "July";
                    case MonthTR.agu: return "August";
                    case MonthTR.eyu: return "September";
                    case MonthTR.eki: return "October";
                    case MonthTR.kas: return "November";
                    case MonthTR.ara: return "December";
                    default: throw _other.ThrowNotSupportedForEnum<MonthTR>();
                }
            }
            return value.GetDescriptionFromEnum();
        }
    }
}