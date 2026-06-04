namespace UD.Core.Extensions
{
    using System;
    using System.Numerics;
    public static class SystemGuidExtensions
    {
        /// <summary><paramref name="guid"/> değerini bir <see cref="BigInteger"/>&#39;a dönüştürür. Guid&#39;in byte dizisi alınır ve bu byte dizisi kullanılarak bir BigInteger oluşturulur. Bu yöntem, Guid&#39;in benzersizliğini koruyarak büyük sayılarla çalışmayı mümkün kılar.</summary>
        /// <param name="guid">Dönüştürülecek Guid değeri.</param>
        /// <returns>Guid değerine karşılık gelen BigInteger değeri.</returns>
        public static BigInteger ToBigInteger(this Guid guid)
        {
            var bytes = guid.ToByteArray();
            var unsignedBytes = new byte[bytes.Length + 1];
            Array.Copy(bytes, unsignedBytes, bytes.Length);
            return new(unsignedBytes);
        }
    }
}