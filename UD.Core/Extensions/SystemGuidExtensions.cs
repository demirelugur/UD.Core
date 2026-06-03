namespace UD.Core.Extensions
{
    using System;
    public static class SystemGuidExtensions
    {
        /// <summary>Verilen <paramref name="guid"/> değerini 16 baytlık bir diziye dönüştürür ve bu dizinin belirli bölümlerini kullanarak bir ulong değeri oluşturur. Guid&#39;in son 8 baytını kullanarak yüksek 16 bitlik kısmı ve düşük 48 bitlik kısmı oluşturur, ardından bu iki kısmı birleştirerek tek bir ulong değeri döndürür.</summary>
        /// <param name="guid">Dönüştürülecek Guid değeri.</param>
        /// <returns>Guid değerine karşılık gelen ulong değeri.</returns>
        public static ulong ToULong(this Guid guid)
        {
            var bytes = guid.ToByteArray();
            var high = (ushort)((bytes[8] << 8) | bytes[9]);
            ulong low = 0;
            low |= (ulong)bytes[10] << 40;
            low |= (ulong)bytes[11] << 32;
            low |= (ulong)bytes[12] << 24;
            low |= (ulong)bytes[13] << 16;
            low |= (ulong)bytes[14] << 8;
            low |= bytes[15];
            return ((ulong)high << 48) | low;
        }
    }
}