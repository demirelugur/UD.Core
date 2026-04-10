namespace UD.Core.Enums
{
    using System.ComponentModel;
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
}