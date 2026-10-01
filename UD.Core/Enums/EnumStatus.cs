using System.ComponentModel.DataAnnotations;
namespace UD.Core.Enums
{
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