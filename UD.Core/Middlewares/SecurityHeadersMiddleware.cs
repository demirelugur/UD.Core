namespace UD.Core.Middlewares
{
    using Microsoft.AspNetCore.Http;
    using System.Text;
    using UD.Core.Extensions;
    public sealed class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate next;
        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        public Task InvokeAsync(HttpContext context)
        {
            var nonce = Convert.ToBase64String(Convert.ToInt64(32).GenerateRandomKey());
            var sb = new StringBuilder();
            sb.Append("default-src 'self'; ");
            sb.Append("img-src 'self' data:; ");
            sb.Append("font-src 'self'; ");
            sb.Append($"style-src 'self' 'nonce-{nonce}'; ");
            sb.Append($"script-src 'self' 'nonce-{nonce}'; ");
            sb.Append("object-src 'none'; ");
            sb.Append("base-uri 'self'; ");
            sb.Append("form-action 'self'; ");
            sb.Append("frame-src 'self'; ");
            sb.Append("connect-src 'self'; ");
            sb.Append("upgrade-insecure-requests;");
            var headers = context.Response.Headers;
            headers.Append("X-Frame-Options", "DENY");
            headers.Append("X-Content-Type-Options", "nosniff");
            headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
            headers.Append("Permissions-Policy", "accelerometer=(), camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), payment=(), usb=()");
            headers.Append("Content-Security-Policy", sb.ToString());
            headers.Append("Strict-Transport-Security", "max-age=31536000; includeSubDomains; preload");
            headers.Append("Cross-Origin-Opener-Policy", "same-origin");
            headers.Append("Cross-Origin-Resource-Policy", "same-origin");
            return this.next(context);
        }
    }
}