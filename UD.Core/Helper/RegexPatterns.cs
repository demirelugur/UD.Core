namespace UD.Core.Helper
{
    using System.Text.RegularExpressions;
    public static partial class RegexPatterns
    {
        /// <summary>HTML etiketlerini tespit etmek için kullanılan regex. Etiket açma/kapama, self-closing ve attributeleri yakalar.</summary>
        [GeneratedRegex(@"</?\w+\s*[^>]*>")]
        public static partial Regex HtmlTagPattern();
        /// <summary>Şifrede en az bir rakam (0-9) olup olmadığını kontrol eden regex.</summary>
        [GeneratedRegex(@"[\d]")]
        public static partial Regex PasswordHasDigit();
        /// <summary>Şifrede en az bir küçük harf (a-z) olup olmadığını kontrol eden regex.</summary>
        [GeneratedRegex(@"[a-z]")]
        public static partial Regex PasswordHasLowercase();
        /// <summary>Şifrede en az bir büyük harf (A-Z) olup olmadığını kontrol eden regex.</summary>
        [GeneratedRegex(@"[A-Z]")]
        public static partial Regex PasswordHasUppercase();
        /// <summary>Şifrede en az bir küçük harf olup olmadığını kontrol eden regex.</summary>
        [GeneratedRegex(@"[a-zçğıöşü]")]
        public static partial Regex PasswordHasTurkishLowercase();
        /// <summary>Şifrede en az bir büyük harf olup olmadığını kontrol eden regex.</summary>
        [GeneratedRegex(@"[A-ZÇĞİÖŞÜ]")]
        public static partial Regex PasswordHasTurkishUppercase();
        /// <summary>Şifrede en az bir özel karakter olup olmadığını kontrol eden regex.</summary>
        [GeneratedRegex(@"[!@#$%^&*()_+\-=\[\]{}|;:',.<>?]")]
        public static partial Regex PasswordHasSpecialChar();
        /// <summary>SEO friendly URL oluşturmak için alfanumerik ve tire dışındaki tüm karakterleri tespit eden regex.</summary>
        [GeneratedRegex(@"[^a-z0-9-]")]
        public static partial Regex NonAlphanumericPattern();
        /// <summary>Ardışık birden fazla tire karakterini tespit eden regex.</summary>
        [GeneratedRegex(@"-+")]
        public static partial Regex MultipleHyphensPattern();
        /// <summary>Ardışık birden fazla boşluk karakterini tespit eden regex.</summary>
        [GeneratedRegex(@" +")]
        public static partial Regex MultipleSpacesPattern();
        /// <summary>Yalnızca rakamlardan oluşan string&#39;i doğrulayan regex. Türk telefon numarası validasyonu için kullanılır.</summary>
        [GeneratedRegex(@"^\d+$")]
        public static partial Regex NumericOnlyPattern();
        /// <summary>Türk araç plakası biçimi - 2 rakam + 1 harf + 4-5 rakam (Örnek: 06 A 1234, 06 A 12345)</summary>
        [GeneratedRegex(@"^(?<city>\d{2})(?<letters>[A-Z]{1})(?<number>\d{4,5})$")]
        public static partial Regex TurkishPlatePattern1();
        /// <summary>Türk araç plakası biçimi - 2 rakam + 2 harf + 3-4 rakam (Örnek: 34 AB 123, 34 AB 1234)</summary>
        [GeneratedRegex(@"^(?<city>\d{2})(?<letters>[A-Z]{2})(?<number>\d{3,4})$")]
        public static partial Regex TurkishPlatePattern2();
        /// <summary>Türk araç plakası biçimi - 2 rakam + 3 harf + 2-3 rakam (Örnek: 35 ABC 12, 35 ABC 123)</summary>
        [GeneratedRegex(@"^(?<city>\d{2})(?<letters>[A-Z]{3})(?<number>\d{2,3})$")]
        public static partial Regex TurkishPlatePattern3();
        /// <summary>MAC adresi biçimini doğrulayan regex. Hex biçiminde, : veya - ayraçlı (Örnek: 00:1A:2B:3C:4D:5E)</summary>
        [GeneratedRegex(@"^([0-9A-F]{2}[:-]){5}([0-9A-F]{2})$")]
        public static partial Regex MacAddressPattern();
    }
}