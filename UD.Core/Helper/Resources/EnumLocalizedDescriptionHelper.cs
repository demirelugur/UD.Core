namespace UD.Core.Helper.Resources
{
    using UD.Core.Enums;
    using UD.Core.Extensions;
    using UD.Core.Helper.TCMB;
    internal class EnumLocalizedDescriptionHelper
    {
        public static string GetLocalizedDescriptionFromEnumResponseMessage(EnumResponseMessage value)
        {
            if (Checks.IsEnglishCurrentUICulture)
            {
                return value switch
                {
                    EnumResponseMessage.Success => "Your request has been completed successfully.",
                    EnumResponseMessage.Error => "An unexpected result occurred during the process! Contact the administrator.",
                    EnumResponseMessage.NotFound => "No records matching the parameters were found.",
                    EnumResponseMessage.InvalidDate => "The entered value does not comply with the date format! Please check.",
                    EnumResponseMessage.UnethicalContent => "\"Prohibited\" words appear in the text! Contact the administrator.",
                    EnumResponseMessage.Unauthorized => "You are not authorized for the transaction! Contact the administrator.",
                    EnumResponseMessage.ConnectionError => "Cannot communicate with the server computer! Contact the administrator.",
                    EnumResponseMessage.MaxLengthExceeded => "The maximum character limit that can be entered has been exceeded! Contact the administrator.",
                    _ => throw Utilities.ThrowNotSupportedForEnum<EnumResponseMessage>()
                };
            }
            return value.GetDescriptionFromEnum();
        }
        public static string GetLocalizedDescriptionFromEnumNVIIdentityCard(EnumNVIIdentityCard value)
        {
            if (Checks.IsEnglishCurrentUICulture)
            {
                return value switch
                {
                    EnumNVIIdentityCard.New => "New ID Card",
                    EnumNVIIdentityCard.Old => "Old Identity Card",
                    _ => throw Utilities.ThrowNotSupportedForEnum<EnumNVIIdentityCard>()
                };
            }
            return value.GetDescriptionFromEnum();
        }
        public static string GetLocalizedDescriptionFromEnumTCMBRateCode(EnumTCMBRateCode value)
        {
            if (Checks.IsEnglishCurrentUICulture)
            {
                return value switch
                {
                    EnumTCMBRateCode.USD => "US DOLLAR",
                    EnumTCMBRateCode.AUD => "AUSTRALIAN DOLLAR",
                    EnumTCMBRateCode.DKK => "DANISH KRONE",
                    EnumTCMBRateCode.EUR => "EURO",
                    EnumTCMBRateCode.GBP => "BRITISH POUND",
                    EnumTCMBRateCode.CHF => "SWISS FRANC",
                    EnumTCMBRateCode.SEK => "SWEDISH KRONA",
                    EnumTCMBRateCode.CAD => "CANADIAN DOLLAR",
                    EnumTCMBRateCode.KWD => "KUWAITI DINAR",
                    EnumTCMBRateCode.NOK => "NORWEGIAN KRONE",
                    EnumTCMBRateCode.SAR => "SAUDI RIYAL",
                    EnumTCMBRateCode.JPY => "JAPANESE YEN",
                    EnumTCMBRateCode.RON => "ROMANIAN LEU",
                    EnumTCMBRateCode.RUB => "RUSSIAN RUBLE",
                    EnumTCMBRateCode.CNY => "CHINESE YUAN",
                    EnumTCMBRateCode.PKR => "PAKISTANI RUPEE",
                    EnumTCMBRateCode.QAR => "QATARI RIYAL",
                    EnumTCMBRateCode.KRW => "SOUTH KOREAN WON",
                    EnumTCMBRateCode.AZN => "AZERBAIJANI MANAT",
                    EnumTCMBRateCode.AED => "UNITED ARAB EMIRATES DIRHAM",
                    EnumTCMBRateCode.KZT => "KAZAKHSTANI TENGE",
                    _ => throw Utilities.ThrowNotSupportedForEnum<EnumTCMBRateCode>()
                };
            }
            return value.GetDescriptionFromEnum();
        }
    }
}