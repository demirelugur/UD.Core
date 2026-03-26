namespace UD.Core.Middlewares
{
    using Microsoft.AspNetCore.Http;
    using System.Security.Cryptography;
    public sealed class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate next;
        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        public Task Invoke(HttpContext httpContext)
        {
            var nonce = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            httpContext.Response.Headers.Append("X-Frame-Options", "DENY");
            httpContext.Response.Headers.Append("X-Xss-Protection", "1; mode=block");
            httpContext.Response.Headers.Append("X-Content-Type-Options", "nosniff");
            httpContext.Response.Headers.Append("Content-Security-Policy", $"default-src 'self'; img-src 'self'; font-src 'self'; style-src 'self'; script-src 'self' 'nonce-{nonce}'; frame-src 'self'; connect-src 'self';");
            httpContext.Response.Headers.Append("Referrer-Policy", "no-referrer");
            httpContext.Response.Headers.Append("Feature-Policy", "accelerometer 'none'; camera 'none'; geolocation 'none'; gyroscope 'none';  magnetometer 'none'; microphone 'none'; payment 'none'; usb 'none'");
            return next(httpContext);
        }
    }
}