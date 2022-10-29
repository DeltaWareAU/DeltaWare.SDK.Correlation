using DeltaWare.SDK.Correlation.AspNetCore.Helpers;
using DeltaWare.SDK.Correlation.Context.Accessors;
using DeltaWare.SDK.Correlation.Options;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace DeltaWare.SDK.Correlation.AspNetCore.Filters
{
    internal sealed class AttachCorrelationIdToResponseHeaderFilter : IActionFilter
    {
        private readonly ICorrelationContextAccessor _contextAccessor;
        private readonly ICorrelationOptions _options;
        private readonly ILogger? _logger;

        public AttachCorrelationIdToResponseHeaderFilter(ICorrelationContextAccessor contextAccessor, ICorrelationOptions options, ILogger<AttachCorrelationIdToResponseHeaderFilter>? logger = null)
        {
            _contextAccessor = contextAccessor;
            _options = options;
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            CorrelationFilterHelper.AttachIdToResponseHeader(context, _contextAccessor, _options, _logger);
        }
    }
}
