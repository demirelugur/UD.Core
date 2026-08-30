namespace UD.Core.Extensions
{
    using System;
    using System.Numerics;
    using System.Text;
    using UD.Core.Enums;

    public static class SystemGuidExtensions
    {
        /// <summary><paramref name="guid"/> değerini, 32 karakter uzunluğunda ve büyük harflerle temsil eden bir dizeye dönüştürür. Bu yöntem, GUID&#39;in standart biçimlendirilmiş dize temsili yerine, yalnızca alfasayısal karakterleri içeren kompakt bir biçim sağlar. Sonuç olarak, GUID&#39;in benzersizliğini koruyan ve daha kısa bir dize temsili elde edilir.</summary>
        /// <param name="guid">Dönüştürülecek GUID değeri.</param>
        /// <returns>GUID&#39;in temsil ettiği kompakt dize.</returns>
        public static string ToCompactString(this Guid guid) => guid.ToString("N").ToUpperInvariant();
        /// <summary>
        /// <paramref name="guid"/> değerini, büyük-endian (big-endian) bayt sıralaması kullanarak bir <see cref="BigInteger"/> nesnesine dönüştürür. Bu yöntem, GUID&#39;in bayt dizisini büyük-endian biçiminde okuyarak, GUID&#39;in benzersiz değerini temsil eden bir tamsayı oluşturur. Sonuç olarak, GUID&#39;in benzersizliğini koruyan ve matematiksel işlemlerde kullanılabilecek bir <see cref="BigInteger"/> değeri elde edilir.
        /// <para>Değer aralığı: <b>[0 - 340282366920938463463374607431768211455]</b></para>
        /// </summary>
        /// <param name="guid">Dönüştürülecek GUID değeri.</param>
        /// <returns>GUID&#39;in temsil ettiği <see cref="BigInteger"/> değeri.</returns>
        public static BigInteger ToBigInteger(this Guid guid) => new(guid.ToByteArray(true), true, true);
        internal const string _base32Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        internal const string _base36Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        internal const string _base62Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        /// <summary><paramref name="guid"/> değerini, belirtilen <paramref name="format"/> kullanarak bir taban dizesine dönüştürür. Bu yöntem, GUID&#39;in benzersiz değerini temsil eden bir tamsayıyı alır ve belirtilen taban karakter kümesini kullanarak, GUID&#39;in kompakt bir dize temsili elde eder. Sonuç olarak, GUID&#39;in benzersizliğini koruyan ve daha kısa bir dize temsili elde edilir.</summary>
        /// <param name="guid">Dönüştürülecek GUID değeri.</param>
        /// <param name="format">Kullanılacak taban formatı.</param>
        /// <returns>GUID&#39;in belirtilen taban biçiminde temsil ettiği dize.</returns>
        public static string ToBaseString(this Guid guid, EnumGuidFormat format)
        {
            if (guid == Guid.Empty) { return "0"; }
            var value = guid.ToBigInteger();
            if (value == 0) { return "0"; }
            var chars = format switch
            {
                EnumGuidFormat.Base32 => _base32Chars,
                EnumGuidFormat.Base36 => _base36Chars,
                EnumGuidFormat.Base62 => _base62Chars,
                _ => throw format.ArgumentOutOfRange(nameof(format))
            };
            var sb = new StringBuilder();
            while (value > 0)
            {
                value = BigInteger.DivRem(value, chars.Length, out BigInteger _remainder);
                sb.Insert(0, chars[(int)_remainder]);
            }
            return sb.ToString();
        }
    }
}