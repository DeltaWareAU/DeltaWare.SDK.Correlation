using DeltaWare.SDK.Correlation.AspNetCore.Helpers;
using DeltaWare.SDK.Correlation.Context.Accessors;
using DeltaWare.SDK.Correlation.Options;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DeltaWare.SDK.Correlation.AspNetCore.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class AttachCorrelationIdToResponseHeaderAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ILogger? logger = context.HttpContext.RequestServices.GetService<ILogger<AttachCorrelationIdToResponseHeaderAttribute>>();

            ICorrelationContextAccessor contextAccessor = context.HttpContext.RequestServices.GetRequiredService<ICorrelationContextAccessor>();
            ICorrelationOptions options = context.HttpContext.RequestServices.GetRequiredService<ICorrelationOptions>();

            CorrelationFilterHelper.AttachIdToResponseHeader(context, contextAccessor, options, logger);
        }
    }
}
