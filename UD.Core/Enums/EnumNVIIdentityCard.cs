namespace UD.Core.Enums
{
    using System.ComponentModel;
    public enum EnumNVIIdentityCard : byte
    {
        /// <summary>Yeni Kimlik Kartı</summary>
        [Description("Yeni Kimlik Kartı")]
        @new = 1,
        /// <summary>Eski Nüfus Cüzdanı</summary>
        [Description("Eski Nüfus Cüzdanı")]
        old
    }
}