namespace UD.Core.Extensions
{
    using System.ComponentModel;
    using UD.Core.Enums;
    using UD.Core.Helper;
    public static class EnumExtensions
    {
        /// <summary>Verilen <see cref="EnumResponseMessage"/> değeri için, mevcut UI kültürüne göre açıklama (description) metnini döner. İngilizce kültür aktif ise karşılık gelen sabit İngilizce mesaj döndürülür. Varsayılan (Türkçe) durumda ise enum üzerinde tanımlı <see cref="DescriptionAttribute"/> değeri kullanılır.</summary>
        public static string GetLocalizedDescription(this EnumResponseMessage value)
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
    }
}