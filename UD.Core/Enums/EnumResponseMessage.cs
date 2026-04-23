namespace UD.Core.Enums
{
    using System.ComponentModel;
    public enum EnumResponseMessage : byte
    {
        /// <summary>İsteğiniz başarılı bir şekilde sonuçlandı.</summary>
        [Description("İsteğiniz başarılı bir şekilde sonuçlandı.")]
        success = 1,
        /// <summary>İşlem sırasında beklenmeyen bir sonuç meydana geldi! Yönetici ile iletişime geçiniz.</summary>
        [Description("İşlem sırasında beklenmeyen bir sonuç meydana geldi! Yönetici ile iletişime geçiniz.")]
        error,
        /// <summary>Parametrelere uyumlu kayıt bulunamadı!</summary>
        [Description("Parametrelere uyumlu kayıt bulunamadı!")]
        notFound,
        /// <summary>Girilen değer tarih biçimine uygun değildir! Kontrol ediniz.</summary>
        [Description("Girilen değer tarih biçimine uygun değildir! Kontrol ediniz.")]
        invalidDate,
        /// <summary>Metin içinde \"yasaklı\" kelimeler geçmektedir! Yönetici ile iletişime geçiniz.</summary>
        [Description("Metin içinde \"yasaklı\" kelimeler geçmektedir! Yönetici ile iletişime geçiniz.")]
        unethicalContent,
        /// <summary>İşlem için yetkiniz bulunmamaktadır! Yönetici ile iletişime geçiniz.</summary>
        [Description("İşlem için yetkiniz bulunmamaktadır! Yönetici ile iletişime geçiniz.")]
        unauthorized,
        /// <summary>Sunucu bilgisayar ile iletişim kurulamıyor! Yönetici ile iletişime geçiniz.</summary>
        [Description("Sunucu bilgisayar ile iletişim kurulamıyor! Yönetici ile iletişime geçiniz.")]
        connectionError,
        /// <summary>Girilebilecek maksimum karakter sınırı aşıldı! Yönetici ile iletişime geçiniz.</summary>
        [Description("Girilebilecek maksimum karakter sınırı aşıldı! Yönetici ile iletişime geçiniz.")]
        maxLengthExceeded
    }
}