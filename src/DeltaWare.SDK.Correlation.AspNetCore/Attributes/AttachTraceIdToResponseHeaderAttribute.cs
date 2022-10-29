using DeltaWare.SDK.Correlation.AspNetCore.Helpers;
using DeltaWare.SDK.Correlation.Context.Accessors;
using DeltaWare.SDK.Correlation.Options;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DeltaWare.SDK.Correlation.AspNetCore.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class AttachTraceIdToResponseHeaderAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ILogger? logger = context.HttpContext.RequestServices.GetService<ILogger<AttachTraceIdToResponseHeaderAttribute>>();

            ITraceContextAccessor contextAccessor = context.HttpContext.RequestServices.GetRequiredService<ITraceContextAccessor>();
            ITraceOptions options = context.HttpContext.RequestServices.GetRequiredService<ITraceOptions>();

            TraceFilterHelper.AttachIdToResponseHeader(context, contextAccessor, options, logger);
            
            base.OnActionExecuted(context);
        }
    }
}
