namespace UD.Core.Extensions
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Linq;
    using System.Net;

    public static class HttpContextExtensions
    {
        /// <summary> İstemcinin mobil bir cihaz olup olmadığını kontrol eder. </summary>
        /// <param name="context">HttpContext nesnesi.</param>
        /// <returns>Mobil bir cihaz ise <see langword="true"/>, değilse <see langword="false"/> döner.</returns>
        public static bool IsMobileDevice(this HttpContext context)
        {
            var _useragent = context.Request.Headers.UserAgent.ToStringOrEmpty().ToLower();
            if (_useragent != "") { foreach (var item in new string[] { "android", "iphone", "ipad", "mobile" }) { if (_useragent.Contains(item)) { return true; } } }
            return false;
        }
        /// <summary> Mevcut HTTP isteğinin şema (http/https) ve host bilgisini kullanarak uygulamanın temel (base) adresini Uri olarak döner. </summary>
        public static Uri GetBaseUri(this HttpContext context)
        {
            var request = context.Request;
            return new($"{request.Scheme}://{(request.Host.HasValue ? request.Host.Value : "")}");
        }
        /// <summary> Mevcut HTTP isteğinin tam adresini (base adres + path + query string) Uri formatında döner. </summary>
        public static Uri GetFullRequestUri(this HttpContext context)
        {
            var request = context.Request;
            return new(String.Concat(
                context.GetBaseUri().ToString().TrimEnd('/'),
                request.Path.HasValue ? request.Path.Value : "",
                request.QueryString.HasValue ? request.QueryString.Value : ""
            ));
        }
        /// <summary>
        /// Bearer token&#39;ı HttpContext&#39;den alır.
        /// </summary>
        /// <param name="context">HttpContext nesnesi.</param>
        /// <returns>Bearer token.</returns>
        public static string GetToken(this HttpContext context) => context.Request.Headers.Authorization.ToString().Replace("Bearer ", "").ToStringOrEmpty();
        /// <summary>
        /// İstemcinin IP adresini döndürür. Öncelikle <c>X-Forwarded-For</c> HTTP başlığını kontrol eder; eğer geçerli bir IP bulunamazsa bağlantının <see cref="ConnectionInfo.RemoteIpAddress"/> değerini kullanır. Geçerli bir IP adresi elde edilemezse <see cref="IPAddress.None"/> döndürülür.
        /// </summary>
        /// <param name="context">HTTP isteğini temsil eden <see cref="HttpContext"/> nesnesi.</param>
        /// <returns>İstemcinin IPv4 formatındaki IP adresi veya bulunamazsa <see cref="IPAddress.None"/>.</returns>
        public static IPAddress GetIPAddress(this HttpContext context)
        {
            if (IPAddress.TryParse(context.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? "", out IPAddress _ip)) { return _ip.MapToIPv4(); }
            _ip = context.Connection.RemoteIpAddress;
            if (_ip == null) { return IPAddress.None; }
            return _ip.MapToIPv4();
        }
    }
}