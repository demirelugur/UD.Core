namespace UD.Core.Middlewares
{
    using Microsoft.AspNetCore.Http;
    using UD.Core.Extensions;
    using UD.Core.Helper;
    using UD.Core.Helper.Results;
    using UD.Core.Helper.Services;
    public sealed class TokenBlacklistMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ITokenBlacklistService tokenBlacklistService;
        public TokenBlacklistMiddleware(RequestDelegate next, ITokenBlacklistService tokenBlacklistService)
        {
            this.next = next;
            this.tokenBlacklistService = tokenBlacklistService;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            var token = httpContext.GetToken();
            if (!token.IsNullOrEmpty() && await this.tokenBlacklistService.Any(token))
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsJsonAsync(ApiResult.setWarning(Checks.IsEnglishCurrentUICulture ? "Token invalid!" : "Token geçersiz!"), httpContext.RequestAborted);
                return;
            }
            await this.next(httpContext);
        }
    }
}