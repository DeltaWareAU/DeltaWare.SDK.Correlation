using DeltaWare.SDK.Correlation.AspNetCore.Context.Scopes;
using DeltaWare.SDK.Correlation.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeltaWare.SDK.Correlation.AspNetCore.Middleware
{
    internal class CorrelationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICorrelationOptions _options;
        private readonly ILogger _logger;

        public CorrelationMiddleware(RequestDelegate next, ICorrelationOptions options, ILogger<CorrelationMiddleware> logger)
        {
            _next = next;
            _options = options;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, AspNetCorrelationContextScope contextScope)
        {
            bool isValid = await contextScope.ValidateHeaderAsync(context);

            if (!isValid)
            {
                return;
            }

            contextScope.TrySetId();

            if (_options.AttachToLoggingScope)
            {
                using (_logger.BeginScope(new Dictionary<string, string>
                {
                    [_options.LoggingScopeKey] = contextScope.Context.CorrelationId
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
