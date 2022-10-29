using DeltaWare.SDK.Correlation.AspNetCore.Context.Scopes;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace DeltaWare.SDK.Correlation.AspNetCore.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class AttachTraceIdToResponseHeaderAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext.RequestServices
                .GetRequiredService<AspNetTraceContextScope>()
                .TrySetId(true);
        }
    }
}
