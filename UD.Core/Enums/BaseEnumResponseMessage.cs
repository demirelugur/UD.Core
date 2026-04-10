namespace UD.Core.Enums
{
    using System.ComponentModel;
    using UD.Core.Extensions;
    using UD.Core.Helper;
    public sealed class BaseEnumResponseMessage
    {
        public enum EnumResponseMessage : byte
        {
            /// <summary>İsteğiniz başarılı bir şekilde sonuçlandı.</summary>
            [Description("İsteğiniz başarılı bir şekilde sonuçlandı.")]
            Success = 1,
            /// <summary>İşlem sırasında beklenmeyen bir sonuç meydana geldi! Yönetici ile iletişime geçiniz.</summary>
            [Description("İşlem sırasında beklenmeyen bir sonuç meydana geldi! Yönetici ile iletişime geçiniz.")]
            Error,
            /// <summary>Parametrelere uyumlu kayıt bulunamadı!</summary>
            [Description("Parametrelere uyumlu kayıt bulunamadı!")]
            NotFound,
            /// <summary>Girilen değer tarih biçimine uygun değildir! Kontrol ediniz.</summary>
            [Description("Girilen değer tarih biçimine uygun değildir! Kontrol ediniz.")]
            InvalidDate,
            /// <summary>Metin içinde \"yasaklı\" kelimeler geçmektedir! Yönetici ile iletişime geçiniz.</summary>
            [Description("Metin içinde \"yasaklı\" kelimeler geçmektedir! Yönetici ile iletişime geçiniz.")]
            UnethicalContent,
            /// <summary>İşlem için yetkiniz bulunmamaktadır! Yönetici ile iletişime geçiniz.</summary>
            [Description("İşlem için yetkiniz bulunmamaktadır! Yönetici ile iletişime geçiniz.")]
            Unauthorized,
            /// <summary>Sunucu bilgisayar ile iletişim kurulamıyor! Yönetici ile iletişime geçiniz.</summary>
            [Description("Sunucu bilgisayar ile iletişim kurulamıyor! Yönetici ile iletişime geçiniz.")]
            ConnectionError,
            /// <summary>Girilebilecek maksimum karakter sınırı aşıldı! Yönetici ile iletişime geçiniz.</summary>
            [Description("Girilebilecek maksimum karakter sınırı aşıldı! Yönetici ile iletişime geçiniz.")]
            MaxLengthExceeded
        }
        public static string GetDescriptionLocalizationValue(object data)
        {
            var enumValue = data.TryToEnum<EnumResponseMessage>();
            if (!enumValue.HasValue) { throw Utilities.ThrowNotSupportedForEnum<EnumResponseMessage>(); }
            if (Checks.IsEnglishCurrentUICulture)
            {
                return enumValue.Value switch
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
            return enumValue.Value.GetDescriptionFromEnum();
        }
    }
}