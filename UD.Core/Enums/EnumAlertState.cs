namespace UD.Core.Enums
{
    using System.ComponentModel;
    public enum EnumAlertState : byte
    {
        /// <summary>İşlem başarılı.</summary>
        [Description("İşlem başarılı.")]
        success = 1,
        /// <summary>Bilgilendirme.</summary>
        [Description("Bilgilendirme.")]
        info,
        /// <summary>Uyarı.</summary>
        [Description("Uyarı.")]
        warning,
        /// <summary>Bir hata oluştu.</summary>
        [Description("Bir hata oluştu.")]
        error
    }
}