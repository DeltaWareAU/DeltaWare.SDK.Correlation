using DeltaWare.SDK.Correlation.AspNetCore.Context.Scopes;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DeltaWare.SDK.Correlation.AspNetCore.Attributes
{
    /// <summary>
    /// Ensures a CorrelationId is provided in the Request Headers, otherwise a 400 is returned to the caller.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class CorrelationIdHeaderRequiredAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await context.HttpContext.RequestServices
                .GetRequiredService<AspNetCorrelationContextScope>()
                .ValidateContextAsync(context.HttpContext, true);

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
