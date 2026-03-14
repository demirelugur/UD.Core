namespace UD.Core.Enums
{
    using System.ComponentModel;
    using UD.Core.Extensions;
    using UD.Core.Helper.Configuration;
    using UD.Core.Helper.Validation;
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
            Guard.ThrowIfUnSupportLanguage(dil, nameof(dil));
            if (dil == "en")
            {
                return value switch
                {
                    MonthTR.oca => "January",
                    MonthTR.sub => "February",
                    MonthTR.mrt => "March",
                    MonthTR.nis => "April",
                    MonthTR.may => "May",
                    MonthTR.haz => "June",
                    MonthTR.tem => "July",
                    MonthTR.agu => "August",
                    MonthTR.eyu => "September",
                    MonthTR.eki => "October",
                    MonthTR.kas => "November",
                    MonthTR.ara => "December",
                    _ => throw Utilities.ThrowNotSupportedForEnum<MonthTR>(),
                };
            }
            return value.GetDescriptionFromEnum();
        }
    }
}