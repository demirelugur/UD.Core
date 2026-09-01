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
        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.GetToken();
            if (!token.IsNullOrEmpty() && this._tokenBlacklistService.Any(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(ApiResponse.setWarning(Checks.IsEnglishCurrentUICulture ? "Token invalid!" : "Token geçersiz!"), context.RequestAborted);
                return;
            }
            await this._next(context);
        }
    }
}