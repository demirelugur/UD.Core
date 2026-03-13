namespace UD.Core.Extensions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using UD.Core.Helper.Validation;
    public static class UriExtensions
    {
        /// <summary>
        /// Verilen dizeye &quot;v&quot; parametresiyle bir sürüm numarası ekleyerek yeni bir URL oluşturur.
        /// </summary>
        /// <param name="uri">Temel URL.</param>
        /// <returns>Sürüm numarası eklenmiş URL.</returns>
        public static string GenerateVersionedUrl(this Uri uri)
        {
            Guard.CheckNull(uri, nameof(uri));
            var separator = (uri.Query.Length > 0 ? "&" : "?");
            return $"{uri.ToString().TrimEnd('/')}{separator}v={DateTime.Now.Ticks}";
        }
        /// <summary>
        /// Verilen URI&#39;nin bir YouTube gömme(embed) bağlantısı olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="uri">Kontrol edilecek URI.</param>
        /// <returns>URI bir YouTube gömme bağlantısı ise <see langword="true"/>, aksi halde <see langword="false"/> döner.</returns>
        public static bool IsYouTubeEmbedLink(this Uri uri)
        {
            Guard.CheckNull(uri, nameof(uri));
            return (uri.Host.Contains("youtube.com") && uri.AbsolutePath.StartsWith("/embed/"));
        }
        /// <summary>
        /// Verilen URI&#39;nin bağlantı durumunu kontrol eder.
        /// </summary>
        public static async Task<(bool hasError, Uri requestUri)> IsConnectionStatus(this Uri uri, TimeSpan timeSpan, CancellationToken cancellationToken = default)
        {
            Guard.CheckNull(uri, nameof(uri));
            using (var client = new HttpClient
            {
                Timeout = timeSpan
            })
            {
                try
                {
                    var response = await client.GetAsync(uri, cancellationToken);
                    if (response.IsSuccessStatusCode) { return (false, response.RequestMessage.RequestUri); }
                    return (true, default);
                }
                catch { return (true, default); }
            }
        }
        /// <summary>
        /// Belirtilen <see cref="Uri"/> adresinden byte[] veri almaya çalışır.
        /// </summary>
        public static async Task<(bool hasError, byte[] dataBinary, string mimeType, Exception ex)> GetBinaryData(this Uri uri, TimeSpan timeSpan, CancellationToken cancellationToken = default)
        {
            Guard.CheckNull(uri, nameof(uri));
            using (var client = new HttpClient
            {
                Timeout = timeSpan
            })
            {
                try
                {
                    var response = await client.GetAsync(uri, cancellationToken);
                    response.EnsureSuccessStatusCode();
                    return (false, await response.Content.ReadAsByteArrayAsync(cancellationToken), response.Content.Headers.ContentType?.MediaType.CoalesceOrDefault("application/octet-stream"), null);
                }
                catch (Exception ex) { return (true, default, "", ex); }
            }
        }
        /// <summary>
        /// Verilen URI&#39;nin şemasını &#39;https&#39; olarak ayarlar ve &#39;www.&#39; önekini kaldırır.
        /// <list type="bullet">
        /// <item>
        /// Eğer URI&#39;nin host kısmı &#39;www.&#39; ile başlıyorsa, bu önek kaldırılır.
        /// </item>
        /// <item>
        /// URI&#39;nin şeması otomatik olarak &#39;https&#39; olarak ayarlanır.
        /// </item>
        /// <item>
        /// Varsayılan port numarası (&#39;-1&#39;) kullanılır, yani URI&#39;deki port bilgisi kaldırılır.
        /// </item>
        /// <item>
        /// Sonuç olarak, düzenlenmiş URI bir dize olarak döndürülür ve sondaki &#39;/&#39; işareti kaldırılır.
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="uri">Düzenlenecek URI nesnesi.</param>
        /// <returns>Düzenlenmiş ve &#39;https&#39; şemasına sahip URI&#39;nin bir dize temsili.</returns>
        /// <exception cref="ArgumentNullException">Eğer URI null ise bir hata fırlatılır.</exception>
        public static string SetHttpsAndRemoveWww(this Uri uri)
        {
            Guard.CheckNull(uri, nameof(uri));
            var host = uri.Host;
            if (host.StartsWith("www.", StringComparison.OrdinalIgnoreCase)) { host = host.Substring(4); }
            return new UriBuilder(uri)
            {
                Scheme = Uri.UriSchemeHttps,
                Host = host,
                Port = -1
            }.Uri.ToString().TrimEnd('/');
        }
    }
}