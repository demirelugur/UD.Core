namespace UD.Core.Helper.Cryptography
{
    using System;
    using UD.Core.Extensions;
    using UD.Core.Helper.Validation;
    public sealed class CaesarHelper
    {
        private static string ApplyShift(string text, int shift)
        {
            if (shift < 0) { return ApplyShift(text, shift + 26); }
            var r = "";
            var textChars = text.ToStringOrEmpty().ToCharArray();
            foreach (var item in textChars)
            {
                if (item >= 'A' && item <= 'Z') { r = String.Concat(r, Convert.ToChar(((item - 'A' + shift) % 26) + 'A').ToString()); }
                else if (item >= 'a' && item <= 'z') { r = String.Concat(r, Convert.ToChar(((item - 'a' + shift) % 26) + 'a').ToString()); }
                else { r = String.Concat(r, item.ToString()); }
            }
            return r;
        }
        /// <summary><paramref name="plainText"/> değerini <paramref name="shift"/> kadar kaydırarak şifreler. Şifreleme işlemi sırasında, yalnızca İngilizce alfabesindeki harfler (büyük ve küçük) kaydırılırken, diğer karakterler (boşluk, noktalama işaretleri, sayılar vb.) olduğu gibi bırakılır. Kaydırma işlemi, harflerin alfabedeki konumlarına göre gerçekleştirilir ve kaydırma miktarı 26&#39;ya bölünerek döngüsel bir şekilde uygulanır. Örneğin, &#39;A&#39; harfi 3 kaydırıldığında &#39;D&#39; olurken, &#39;Z&#39; harfi 1 kaydırıldığında &#39;A&#39; olur.</summary>
        /// <param name="plainText">Şifrelenecek metin.</param>
        /// <param name="shift">Kaydırma miktarı. Pozitif bir değer olmalıdır.</param>
        /// <returns>Şifrelenmiş metin.</returns>
        public static string Encrypt(string plainText, int shift)
        {
            Guard.ThrowIfZeroOrNegative(shift, nameof(shift));
            return ApplyShift(plainText, shift);
        }
        /// <summary><paramref name="cipherText"/> değerini <paramref name="shift"/> kadar kaydırarak çözer. Şifre çözme işlemi sırasında, yalnızca İngilizce alfabesindeki harfler (büyük ve küçük) kaydırılırken, diğer karakterler (boşluk, noktalama işaretleri, sayılar vb.) olduğu gibi bırakılır. Kaydırma işlemi, harflerin alfabedeki konumlarına göre gerçekleştirilir ve kaydırma miktarı 26&#39;ya bölünerek döngüsel bir şekilde uygulanır. Örneğin, &#39;D&#39; harfi 3 kaydırıldığında &#39;A&#39; olurken, &#39;A&#39; harfi 1 kaydırıldığında &#39;Z&#39; olur.</summary>
        /// <param name="cipherText">Şifreli metin.</param>
        /// <param name="shift">Kaydırma miktarı. Pozitif bir değer olmalıdır.</param>
        /// <returns>Deşifre edilmiş metin.</returns>
        public static string Decrypt(string cipherText, int shift)
        {
            Guard.ThrowIfZeroOrNegative(shift, nameof(shift));
            return ApplyShift(cipherText, (0 - shift));
        }
    }
}