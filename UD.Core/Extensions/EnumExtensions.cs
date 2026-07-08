namespace UD.Core.Extensions
{
    using Microsoft.AspNetCore.Http;
    using UD.Core.Enums;
    using UD.Core.Helper;
    public static class EnumExtensions
    {
        /// <summary><paramref name="value"/> değerine göre, geçerli UI kültürüne uygun açıklamayı döndürür. Eğer geçerli UI kültürü İngilizce ise, Enum değerlerine özel tanımlanmış İngilizce açıklamaları döndürür. Diğer durumlarda, Enum değerlerinin açıklamalarını enum tanımlarında belirtilen açıklamalara göre döndürür.</summary>
        /// <param name="value">Açıklaması alınacak Enum değeri.</param>
        /// <returns>Geçerli UI kültürüne uygun açıklama.</returns>
        public static string GetDescriptionLocalized(this EnumAlertState value)
        {
            if (Checks.IsEnglishCurrentUICulture)
            {
                return value switch
                {
                    EnumAlertState.success => "Operation successful.",
                    EnumAlertState.info => "Information.",
                    EnumAlertState.warning => "Warning.",
                    EnumAlertState.error => "An error occurred.",
                    _ => throw value.ArgumentOutOfRange(nameof(value))
                };
            }
            return value.GetDescriptionFromEnum();
        }
        /// <summary><paramref name="value"/> değerine göre, geçerli UI kültürüne uygun HTTP durum kodunu döndürür. Eğer geçerli UI kültürü İngilizce ise, Enum değerlerine özel tanımlanmış HTTP durum kodlarını döndürür. Diğer durumlarda, Enum değerlerinin açıklamalarını enum tanımlarında belirtilen açıklamalara göre döndürür.</summary>
        /// <param name="value">HTTP durum kodu alınacak Enum değeri.</param>
        /// <returns>Geçerli UI kültürüne uygun HTTP durum kodu.</returns>
        public static int GetStatusCode(this EnumAlertState value) => value switch
        {
            EnumAlertState.success => StatusCodes.Status200OK,
            EnumAlertState.info => StatusCodes.Status202Accepted,
            EnumAlertState.warning => StatusCodes.Status202Accepted,
            EnumAlertState.error => StatusCodes.Status400BadRequest,
            _ => throw value.ArgumentOutOfRange(nameof(value))
        };
        /// <summary><paramref name="value"/> değerine göre, geçerli UI kültürüne uygun açıklamayı döndürür. Eğer geçerli UI kültürü İngilizce ise, Enum değerlerine özel tanımlanmış İngilizce açıklamaları döndürür. Diğer durumlarda, Enum değerlerinin açıklamalarını enum tanımlarında belirtilen açıklamalara göre döndürür.</summary>
        /// <param name="value">Açıklaması alınacak Enum değeri.</param>
        /// <returns>Geçerli UI kültürüne uygun açıklama.</returns>
        public static string GetDescriptionLocalized(this EnumNVIIdentityCard value)
        {
            if (Checks.IsEnglishCurrentUICulture)
            {
                return value switch
                {
                    EnumNVIIdentityCard.@new => "New ID Card",
                    EnumNVIIdentityCard.old => "Old Identity Card",
                    _ => throw value.ArgumentOutOfRange(nameof(value))
                };
            }
            return value.GetDescriptionFromEnum();
        }
        /// <summary><paramref name="value"/> değerine göre, geçerli UI kültürüne uygun açıklamayı döndürür. Eğer geçerli UI kültürü İngilizce ise, Enum değerlerine özel tanımlanmış İngilizce açıklamaları döndürür. Diğer durumlarda, Enum değerlerinin açıklamalarını enum tanımlarında belirtilen açıklamalara göre döndürür.</summary>
        /// <param name="value">Açıklaması alınacak Enum değeri.</param>
        /// <returns>Geçerli UI kültürüne uygun açıklama.</returns>
        public static string GetDescriptionLocalized(this EnumTCMBRateCode value)
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
                    _ => throw value.ArgumentOutOfRange(nameof(value))
                };
            }
            return value.GetDescriptionFromEnum();
        }
    }
}