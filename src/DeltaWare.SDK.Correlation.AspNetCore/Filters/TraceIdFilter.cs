using DeltaWare.SDK.Correlation.AspNetCore.Context.Scopes;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DeltaWare.SDK.Correlation.AspNetCore.Filters
{
    internal sealed class TraceIdFilter : IActionFilter
    {
        private readonly AspNetTraceContextScope _contextScope;

        public TraceIdFilter(AspNetTraceContextScope contextScope)
        {
            _contextScope = contextScope;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _contextScope.ValidateContext(context);

            context.HttpContext.Response.OnStarting(() =>
            {
                _contextScope.TrySetId();

                return Task.CompletedTask;
            });
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
