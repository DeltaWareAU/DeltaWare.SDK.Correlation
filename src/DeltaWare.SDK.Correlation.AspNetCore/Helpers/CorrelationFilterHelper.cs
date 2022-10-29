using DeltaWare.SDK.Correlation.Context.Accessors;
using DeltaWare.SDK.Correlation.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace DeltaWare.SDK.Correlation.AspNetCore.Helpers
{
    internal static class CorrelationFilterHelper
    {
        public static void IdRequired(ActionExecutingContext context, ICorrelationContextAccessor contextAccessor, ICorrelationOptions options, ILogger? logger = null)
        {
            if (contextAccessor.Scope.TryGetId(out string? correlationId))
            {
                logger?.LogDebug("Header Validation Passed. A CorrelationId {CorrelationId} was found the HttpRequest Headers", correlationId);

                return;
            }

            logger?.LogWarning("Header Validation Failed. A CorrelationId was not found in the HttpRequest Headers, responding with 400 (Bad Request).");

            context.Result = new BadRequestObjectResult($"The Request Headers must contain the {options.Header} Header.");
        }

        public static void AttachIdToResponseHeader(ActionExecutedContext context, ICorrelationContextAccessor contextAccessor, ICorrelationOptions options, ILogger? logger = null)
        {
            context.HttpContext.Response.Headers.Add(options.Header, contextAccessor.Context.CorrelationId);

            logger?.LogDebug("Correlation ID has been attached to the Response Headers");
        }
    }
}
