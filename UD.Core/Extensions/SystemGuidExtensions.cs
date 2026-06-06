namespace UD.Core.Extensions
{
    using System;
    using System.Numerics;
    public static class SystemGuidExtensions
    {
        /// <summary>
        /// <paramref name="guid"/> değerini, büyük-endian (big-endian) bayt sıralaması kullanarak bir <see cref="BigInteger"/> nesnesine dönüştürür. Bu yöntem, GUID&#39;in bayt dizisini büyük-endian biçiminde okuyarak, GUID&#39;in benzersiz değerini temsil eden bir tamsayı oluşturur. Sonuç olarak, GUID&#39;in benzersizliğini koruyan ve matematiksel işlemlerde kullanılabilecek bir <see cref="BigInteger"/> değeri elde edilir.
        /// <para>Değer aralığı: <b>[0 - 340282366920938463463374607431768211455]</b></para>
        /// </summary>
        /// <param name="guid">Dönüştürülecek GUID değeri.</param>
        /// <returns>GUID&#39;in temsil ettiği <see cref="BigInteger"/> değeri.</returns>
        public static BigInteger ToBigInteger(this Guid guid) => new(guid.ToByteArray(true), true, true);
    }
}