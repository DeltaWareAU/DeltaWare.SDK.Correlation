using DeltaWare.SDK.Correlation.AspNetCore.Context.Scopes;
using DeltaWare.SDK.Correlation.Context.Accessors;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DeltaWare.SDK.Correlation.AspNetCore.Filters
{
    internal sealed class TraceIdSetFilter : IActionFilter
    {
        private readonly AspNetTraceContextScope _contextScope;
        private readonly TraceContextAccessor _contextAccessor;

        public TraceIdSetFilter(AspNetTraceContextScope contextScope, TraceContextAccessor contextAccessor)
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
