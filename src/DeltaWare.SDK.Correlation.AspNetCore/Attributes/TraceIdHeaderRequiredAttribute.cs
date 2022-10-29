using DeltaWare.SDK.Correlation.AspNetCore.Helpers;
using DeltaWare.SDK.Correlation.Context.Accessors;
using DeltaWare.SDK.Correlation.Options;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
            ILogger? logger = context.HttpContext.RequestServices.GetService<ILogger<TraceIdHeaderRequiredAttribute>>();

            ITraceContextAccessor contextAccessor = context.HttpContext.RequestServices.GetRequiredService<ITraceContextAccessor>();
            ITraceOptions options = context.HttpContext.RequestServices.GetRequiredService<ITraceOptions>();

            TraceFilterHelper.IdRequired(context, contextAccessor, options, logger);
        }
    }
}
