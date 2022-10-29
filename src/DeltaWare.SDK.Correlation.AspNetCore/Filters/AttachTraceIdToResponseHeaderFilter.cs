using DeltaWare.SDK.Correlation.AspNetCore.Helpers;
using DeltaWare.SDK.Correlation.Context.Accessors;
using DeltaWare.SDK.Correlation.Options;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace DeltaWare.SDK.Correlation.AspNetCore.Filters
{
    internal sealed class AttachTraceIdToResponseHeaderFilter : IActionFilter
    {
        private readonly ITraceContextAccessor _contextAccessor;
        private readonly ITraceOptions _options;
        private readonly ILogger? _logger;

        public AttachTraceIdToResponseHeaderFilter(ITraceContextAccessor contextAccessor, ITraceOptions options, ILogger<AttachTraceIdToResponseHeaderFilter>? logger = null)
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
            TraceFilterHelper.AttachIdToResponseHeader(context, _contextAccessor, _options, _logger);
        }
    }
}
