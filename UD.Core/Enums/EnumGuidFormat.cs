namespace UD.Core.Enums
{
    public enum EnumGuidFormat : byte
    {
        /// <summary>Guid&#39;den dönüştürüldüğünde maksimum 26 karakter uzunluğunda, A-Z, 2-7 karakterlerini içeren bir dize temsili.</summary>
        Base32 = 1,
        /// <summary>Guid&#39;den dönüştürüldüğünde maksimum 25 karakter uzunluğunda, 0-9 ve A-Z karakterlerini içeren bir dize temsili.</summary>
        Base36,
        /// <summary>Guid&#39;den dönüştürüldüğünde maksimum 22 karakter uzunluğunda, 0-9, A-Z ve a-z karakterlerini içeren bir dize temsili.</summary>
        Base62
    }
}