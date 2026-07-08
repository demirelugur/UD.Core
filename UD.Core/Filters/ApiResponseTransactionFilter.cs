namespace UD.Core.Filters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using UD.Core.Enums;
    using UD.Core.Extensions;
    using UD.Core.Helper.Responses;
    public sealed class ApiResponseTransactionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var executedContext = await next();
            if (executedContext.Result is ObjectResult _or && _or.Value is ApiResponse _ar && _ar.state.Includes(EnumAlertState.warning, EnumAlertState.error)) { context.HttpContext.MarkTransactionRollback(); }
        }
        /*
        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<ApiResponseTransactionFilter>();
        }); 
        */
    }
}