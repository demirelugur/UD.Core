namespace UD.Core.Middlewares
{
    using Microsoft.AspNetCore.Http;
    using UD.Core.Extensions;
    using UD.Core.Helper;
    using UD.Core.Helper.Responses;
    using UD.Core.Helper.Services;
    public sealed class TokenBlacklistMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITokenBlacklistService _tokenBlacklistService;
        public TokenBlacklistMiddleware(RequestDelegate next, ITokenBlacklistService tokenBlacklistService)
        {
            this._next = next;
            this._tokenBlacklistService = tokenBlacklistService;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            var token = httpContext.GetToken();
            if (!token.IsNullOrEmpty() && this._tokenBlacklistService.Any(token))
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsJsonAsync(ApiResponse.setWarning(Checks.IsEnglishCurrentUICulture ? "Token invalid!" : "Token geçersiz!"), httpContext.RequestAborted);
                return;
            }
            await this._next(httpContext);
        }
    }
}