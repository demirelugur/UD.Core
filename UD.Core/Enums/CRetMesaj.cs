namespace UD.Core.Enums
{
    using System.ComponentModel;
    using UD.Core.Extensions;
    using UD.Core.Extensions.Common;
    using UD.Core.Helper;
    public sealed class CRetMesaj
    {
        public enum RetMesaj : byte
        {
            /// <summary>İsteğiniz başarılı bir şekilde sonuçlandı.</summary>
            [Description("İsteğiniz başarılı bir şekilde sonuçlandı.")]
            basari = 1,
            /// <summary>İşlem sırasında beklenmeyen bir sonuç meydana geldi! Yönetici ile iletişime geçiniz.</summary>
            [Description("İşlem sırasında beklenmeyen bir sonuç meydana geldi! Yönetici ile iletişime geçiniz.")]
            hata,
            /// <summary>Parametrelere uyumlu kayıt bulunamadı!</summary>
            [Description("Parametrelere uyumlu kayıt bulunamadı!")]
            kayityok,
            /// <summary>Girilen değer tarih biçimine uygun değildir! Kontrol ediniz.</summary>
            [Description("Girilen değer tarih biçimine uygun değildir! Kontrol ediniz.")]
            tarih,
            /// <summary>Metin içinde \"yasaklı\" kelimeler geçmektedir! Yönetici ile iletişime geçiniz.</summary>
            [Description("Metin içinde \"yasaklı\" kelimeler geçmektedir! Yönetici ile iletişime geçiniz.")]
            unethical,
            /// <summary>İşlem için yetkiniz bulunmamaktadır! Yönetici ile iletişime geçiniz.</summary>
            [Description("İşlem için yetkiniz bulunmamaktadır! Yönetici ile iletişime geçiniz.")]
            unauthority,
            /// <summary>Sunucu bilgisayar ile iletişim kurulamıyor! Yönetici ile iletişime geçiniz.</summary>
            [Description("Sunucu bilgisayar ile iletişim kurulamıyor! Yönetici ile iletişime geçiniz.")]
            unconnection,
            /// <summary>Girilebilecek maksimum karakter sınırı aşıldı! Yönetici ile iletişime geçiniz.</summary>
            [Description("Girilebilecek maksimum karakter sınırı aşıldı! Yönetici ile iletişime geçiniz.")]
            maxlength
        }
        public static string GetDescriptionLocalizationValue(object data)
        {
            var enumValue = data.TryToEnum<RetMesaj>();
            if (!enumValue.HasValue) { throw Utilities.ThrowNotSupportedForEnum<RetMesaj>(); }
            if (ValidationChecks.IsEnglishDefaultThreadCurrentUICulture)
            {
                return enumValue.Value switch
                {
                    RetMesaj.basari => "Your request has been completed successfully.",
                    RetMesaj.hata => "An unexpected result occurred during the process! Contact the administrator.",
                    RetMesaj.kayityok => "No records matching the parameters were found.",
                    RetMesaj.tarih => "The entered value does not comply with the date format! Please check.",
                    RetMesaj.unethical => "\"Prohibited\" words appear in the text! Contact the administrator.",
                    RetMesaj.unauthority => "You are not authorized for the transaction! Contact the administrator.",
                    RetMesaj.unconnection => "Cannot communicate with the server computer! Contact the administrator.",
                    RetMesaj.maxlength => "The maximum character limit that can be entered has been exceeded! Contact the administrator.",
                    _ => throw Utilities.ThrowNotSupportedForEnum<RetMesaj>()
                };
            }
            return enumValue.Value.GetDescriptionFromEnum();
        }
    }
}