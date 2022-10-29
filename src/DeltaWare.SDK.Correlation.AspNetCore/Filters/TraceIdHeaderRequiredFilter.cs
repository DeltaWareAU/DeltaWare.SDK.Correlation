using DeltaWare.SDK.Correlation.AspNetCore.Helpers;
using DeltaWare.SDK.Correlation.Context.Accessors;
using DeltaWare.SDK.Correlation.Options;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace DeltaWare.SDK.Correlation.AspNetCore.Filters
{
    internal sealed class TraceIdHeaderRequiredFilter : IActionFilter
    {
        private readonly ITraceContextAccessor _contextAccessor;
        private readonly ITraceOptions _options;
        private readonly ILogger? _logger;

        public TraceIdHeaderRequiredFilter(ITraceContextAccessor contextAccessor, ITraceOptions options, ILogger<TraceIdHeaderRequiredFilter>? logger)
        {
            _contextAccessor = contextAccessor;
            _options = options;
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            TraceFilterHelper.IdRequired(context, _contextAccessor, _options, _logger);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
