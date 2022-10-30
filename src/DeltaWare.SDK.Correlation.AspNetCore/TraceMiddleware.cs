using DeltaWare.SDK.Correlation.AspNetCore.Context.Scopes;
using DeltaWare.SDK.Correlation.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeltaWare.SDK.Correlation.AspNetCore
{
    internal class TraceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITraceOptions _options;
        private readonly ILogger _logger;

        public TraceMiddleware(RequestDelegate next, ITraceOptions options, ILogger<TraceMiddleware> logger)
        {
            _next = next;
            _options = options;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, AspNetTraceContextScope contextScope)
        {
            bool isValid = await contextScope.ValidateContextAsync(context);

            if (!isValid || !contextScope.Context.HasId)
            {
                return;
            }

            contextScope.TrySetId();

            if (_options.AttachToLoggingScope)
            {
                using (_logger.BeginScope(new Dictionary<string, string>
                {
                    [_options.LoggingScopeKey] = contextScope.Context.TraceId!
                }))
                {
                    await _next(context);
                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}
