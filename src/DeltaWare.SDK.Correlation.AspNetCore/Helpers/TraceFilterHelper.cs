using DeltaWare.SDK.Correlation.Context.Accessors;
using DeltaWare.SDK.Correlation.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace DeltaWare.SDK.Correlation.AspNetCore.Helpers
{
    internal static class TraceFilterHelper
    {
        public static void IdRequired(ActionExecutingContext context, ITraceContextAccessor contextAccessor, ITraceOptions options, ILogger? logger = null)
        {
            if (contextAccessor.Scope.TryGetId(out string? correlationId))
            {
                logger?.LogDebug("Header Validation Passed. A TraceId {TraceId} was found the HttpRequest Headers", correlationId);

                return;
            }

            logger?.LogWarning("Header Validation Failed. A TraceId was not found in the HttpRequest Headers, responding with 400 (Bad Request).");

            context.Result = new BadRequestObjectResult($"The Request Headers must contain the {options.Header} Header.");
        }

        public static void AttachIdToResponseHeader(ActionExecutedContext context, ITraceContextAccessor contextAccessor, ITraceOptions options, ILogger? logger = null)
        {
            context.HttpContext.Response.Headers.Add(options.Header, contextAccessor.Context.TraceId);

            logger?.LogDebug("Correlation ID has been attached to the Response Headers");
        }
    }
}
