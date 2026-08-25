namespace UD.Core.Enums
{
    using System.ComponentModel.DataAnnotations;
    public enum EnumAlertState : byte
    {
        /// <summary>İşlem başarılı</summary>
        [Display(Name = "İşlem başarılı")]
        success = 1,
        /// <summary>Bilgilendirme</summary>
        [Display(Name = "Bilgilendirme")]
        info,
        /// <summary>Uyarı</summary>
        [Display(Name = "Uyarı")]
        warning,
        /// <summary>Bir hata oluştu</summary>
        [Display(Name = "Bir hata oluştu")]
        error
    }
}