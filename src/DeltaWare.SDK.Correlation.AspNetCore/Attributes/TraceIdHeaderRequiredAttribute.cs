using DeltaWare.SDK.Correlation.AspNetCore.Context.Scopes;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace DeltaWare.SDK.Correlation.AspNetCore.Attributes
{
    /// <summary>
    /// Ensures a TraceId is provided in the Request Headers, otherwise a 400 is returned to the caller.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class TraceIdHeaderRequiredAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.RequestServices
                .GetRequiredService<AspNetTraceContextScope>()
                .ValidateContext(context, true);
        }
    }
}
