namespace UD.Core.Enums
{
    using System.ComponentModel.DataAnnotations;
    public enum EnumStatus : byte
    {
        /// <summary>Aktif</summary>
        [Display(Name = "Aktif")]
        active = 1,
        /// <summary>Pasif</summary>
        [Display(Name = "Pasif")]
        passive
    }
}