namespace UD.Core.Enums
{
    using System.ComponentModel.DataAnnotations;
    public enum EnumNVIIdentityCard : byte
    {
        /// <summary>Yeni Kimlik Kartı</summary>
        [Display(Name = "Yeni Kimlik Kartı")]
        @new = 1,
        /// <summary>Eski Nüfus Cüzdanı</summary>
        [Display(Name = "Eski Nüfus Cüzdanı")]
        old
    }
}