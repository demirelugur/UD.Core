namespace UD.Core.Extensions
{
    using Microsoft.AspNetCore.Http;
    using System.Data;
    using UD.Core.Enums;
    using UD.Core.Helper;
    public static class EnumExtensions
    {
        /// <summary><paramref name="value"/> değerinin, <see cref="EnumAlertState.warning"/> veya <see cref="EnumAlertState.error"/> değerlerini içerip içermediğini kontrol eder. Eğer <paramref name="value"/> değeri bu iki değerden herhangi birini içeriyorsa, <see langword="true"/> döner; aksi takdirde <see langword="false"/> döner.</summary>
        /// <param name="value">Kontrol edilecek EnumAlertState değeri.</param>
        /// <returns>Belirtilen değerlerin herhangi birini içeriyorsa <see langword="true"/>, aksi takdirde <see langword="false"/>.</returns>
        public static bool IsFailed(this EnumAlertState value) => value.Includes(EnumAlertState.warning, EnumAlertState.error);
        /// <summary><paramref name="value"/> değerine göre, geçerli UI kültürüne uygun açıklamayı döndürür. Eğer geçerli UI kültürü İngilizce ise, Enum değerlerine özel tanımlanmış İngilizce açıklamaları döndürür. Diğer durumlarda, Enum değerlerinin açıklamalarını enum tanımlarında belirtilen açıklamalara göre döndürür.</summary>
        /// <param name="value">Açıklaması alınacak Enum değeri.</param>
        /// <returns>Geçerli UI kültürüne uygun açıklama.</returns>
        public static string GetDisplayNameLocalized(this EnumAlertState value)
        {
            if (Checks.IsEnglishCurrentUICulture)
            {
                return value switch
                {
                    EnumAlertState.success => "Operation successful",
                    EnumAlertState.info => "Information",
                    EnumAlertState.warning => "Warning",
                    EnumAlertState.error => "An error occurred",
                    _ => throw value.ArgumentOutOfRange(nameof(value))
                };
            }
            return value.GetDisplayNameFromEnum();
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
        public static string GetDisplayNameLocalized(this EnumNVIIdentityCard value)
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
            return value.GetDisplayNameFromEnum();
        }
        /// <summary><paramref name="value"/> değerine göre, geçerli UI kültürüne uygun açıklamayı döndürür. Eğer geçerli UI kültürü İngilizce ise, Enum değerlerine özel tanımlanmış İngilizce açıklamaları döndürür. Diğer durumlarda, Enum değerlerinin açıklamalarını enum tanımlarında belirtilen açıklamalara göre döndürür.</summary>
        /// <param name="value">Açıklaması alınacak Enum değeri.</param>
        /// <returns>Geçerli UI kültürüne uygun açıklama.</returns>
        public static string GetDisplayNameLocalized(this EnumTCMBRateCode value)
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
            return value.GetDisplayNameFromEnum();
        }
        /// <summary>Verilen <see cref="SqlDbType"/> enum değerini, SQL Server sistem tür kimliğine (<c>[system_type_id]</c>) dönüştürür. Bu kimlikler, SQL Server&#39;ın [sys].[types] sistem tablosunda bulunan ve her veri türü için benzersiz olan sayısal değerlerdir.</summary>
        /// <param name="type">Dönüştürülecek <see cref="SqlDbType"/> enum değeri.</param>
        /// <returns>SQL Server sistem tür kimliği (<c>[system_type_id]</c>) değeri</returns>
        /// <exception cref="ArgumentOutOfRangeException">Desteklenmeyen bir <see cref="SqlDbType"/> değeri verildiğinde fırlatılır.</exception>
        /// <remarks>SELECT [name], [system_type_id] FROM [sys].[types]</remarks>
        public static int ToSystemTypeId(this SqlDbType type)
        {
            return type switch
            {
                SqlDbType.Image => 34,
                SqlDbType.Text => 35,
                SqlDbType.UniqueIdentifier => 36,
                SqlDbType.Date => 40,
                SqlDbType.Time => 41,
                SqlDbType.DateTime2 => 42,
                SqlDbType.DateTimeOffset => 43,
                SqlDbType.TinyInt => 48,
                SqlDbType.SmallInt => 52,
                SqlDbType.Int => 56,
                SqlDbType.SmallDateTime => 58,
                SqlDbType.Real => 59,
                SqlDbType.Money => 60,
                SqlDbType.DateTime => 61,
                SqlDbType.Float => 62,
                SqlDbType.NText => 99,
                SqlDbType.Bit => 104,
                SqlDbType.Decimal => 106,
                SqlDbType.SmallMoney => 122,
                SqlDbType.BigInt => 127,
                SqlDbType.VarBinary => 165,
                SqlDbType.VarChar => 167,
                SqlDbType.Binary => 173,
                SqlDbType.Char => 175,
                SqlDbType.Timestamp => 189,
                SqlDbType.NVarChar => 231,
                SqlDbType.NChar => 239,
                SqlDbType.Xml => 241,
                _ => throw type.ArgumentOutOfRange(nameof(type))
            };
        }
        internal static (int maxLength, string chars) GetFormatInfo(this EnumGuidFormat format) => format switch
        {
            EnumGuidFormat.Base32 => (26, "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567"),
            EnumGuidFormat.Base36 => (25, "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"),
            EnumGuidFormat.Base62 => (22, "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"),
            _ => throw format.ArgumentOutOfRange(nameof(format))
        };
    }
}