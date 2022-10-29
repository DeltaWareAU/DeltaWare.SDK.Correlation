using DeltaWare.SDK.Correlation.AspNetCore.Helpers;
using DeltaWare.SDK.Correlation.Context.Accessors;
using DeltaWare.SDK.Correlation.Options;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace DeltaWare.SDK.Correlation.AspNetCore.Filters
{
    internal sealed class CorrelationIdHeaderRequiredFilter : IActionFilter
    {
        private readonly ICorrelationContextAccessor _contextAccessor;
        private readonly ICorrelationOptions _options;
        private readonly ILogger? _logger;

        public CorrelationIdHeaderRequiredFilter(ICorrelationContextAccessor contextAccessor, ICorrelationOptions options, ILogger<CorrelationIdHeaderRequiredFilter>? logger = null)
        {
            _contextAccessor = contextAccessor;
            _options = options;
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            CorrelationFilterHelper.IdRequired(context, _contextAccessor, _options, _logger);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
