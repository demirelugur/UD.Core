namespace UD.Core.Extensions
{
    using Microsoft.Extensions.Caching.Memory;
    using UD.Core.Helper.Results;
    public static class ApiResultExtensions
    {
        /// <summary>
        /// Verilen hata mesajını başarısız sonucu temsil edecek şekilde döndürür.
        /// </summary>
        public static ApiResult<T> ReturnFailed<T>(this string error) => new string[] { error }.ReturnFailed<T>();
        /// <summary>
        /// Verilen hata mesajlarını başarısız sonucu temsil edecek şekilde döndürür.
        /// </summary>
        public static ApiResult<T> ReturnFailed<T>(this string[] errors) => new(default, false, errors);
        /// <summary>
        /// Verilen hata mesajını başarısız sonucu temsil edecek şekilde döndürür.
        /// </summary>
        public static ApiResult<object[]> ReturnFailedObjectArray(this string error) => new string[] { error }.ReturnFailedObjectArray();
        /// <summary>
        /// Verilen hata mesajlarını başarısız sonucu temsil edecek şekilde döndürür.
        /// </summary>
        public static ApiResult<object[]> ReturnFailedObjectArray(this string[] errors) => new(default, false, errors);
        /// <summary>
        /// <paramref name="data"/> verisini önbelleğe ekler ve başarılı işlem sonucunu (<see cref="ApiResult{T}"/>) döndürür.
        /// </summary>
        /// <typeparam name="T">Önbelleğe eklenecek verinin tipi.</typeparam>
        /// <param name="memorycache">Önbellek nesnesi.</param>
        /// <param name="cachekey">Önbelleğe eklenecek değerin anahtarı.</param>
        /// <param name="data">Önbelleğe eklenecek veri.</param>
        /// <param name="timespan">Önbellekte tutulma süresi. Boş bırakılırsa varsayılan olarak 1 dakika kullanılır.</param>
        /// <returns>Başarılı işlem sonucunu temsil eden <see cref="ApiResult{T}"/> nesnesi.</returns>
        public static ApiResult<T> SetCacheAndReturnSuccess<T>(this IMemoryCache memorycache, object cachekey, T data, TimeSpan? timespan = null)
        {
            memorycache.Set(cachekey, data, timespan ?? TimeSpan.FromMinutes(1));
            return new(data, true, default);
        }
    }
}