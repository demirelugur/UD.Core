namespace UD.Core.Extensions
{
    using System;
    using System.Numerics;
    using System.Text;
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
        internal const string _base62Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        /// <summary>Dönüştürülen <paramref name="guid"/> değerini, 62 tabanında (Base62) temsil eden bir dizeye dönüştürür. Bu yöntem, GUID&#39;in benzersiz değerini daha kısa ve okunabilir bir biçimde ifade etmek için kullanılır. Sonuç olarak, GUID&#39;in benzersizliğini koruyan ve daha kompakt bir dize temsili elde edilir.</summary>
        /// <param name="guid">Dönüştürülecek GUID değeri.</param>
        /// <returns>GUID&#39;in temsil ettiği Base62 dize.</returns>
        public static string ToBase62String(this Guid guid)
        {
            var value = guid.ToBigInteger();
            if (value == 0) { return "0"; }
            var result = new StringBuilder();
            while (value > 0)
            {
                value = BigInteger.DivRem(value, _base62Chars.Length, out BigInteger _remainder);
                result.Insert(0, _base62Chars[(int)_remainder]);
            }
            return result.ToString();
        }
    }
}