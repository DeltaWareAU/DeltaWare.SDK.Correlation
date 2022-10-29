using DeltaWare.SDK.Correlation.AspNetCore.Context.Scopes;
using DeltaWare.SDK.Correlation.Context.Accessors;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DeltaWare.SDK.Correlation.AspNetCore.Filters
{
    internal sealed class CorrelationIdSetFilter : IActionFilter
    {
        private readonly AspNetCorrelationContextScope _contextScope;
        private readonly CorrelationContextAccessor _contextAccessor;

        public CorrelationIdSetFilter(AspNetCorrelationContextScope contextScope, CorrelationContextAccessor contextAccessor)
        {
            _contextScope = contextScope;
            _contextAccessor = contextAccessor;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _contextAccessor.InternalScope = _contextScope;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
