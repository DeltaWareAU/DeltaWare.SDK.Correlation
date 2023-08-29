using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Options;
using TraceLink.AspNetCore.Context.Scopes;

namespace TraceLink.AspNetCore.Middleware
{
    internal sealed class TracingContextMiddleware<TTracingContext> where TTracingContext : ITracingContext
    {
        private readonly RequestDelegate _next;
        private readonly ITracingOptions _options;
        private readonly ILogger? _logger;

        public TracingContextMiddleware(RequestDelegate next, ITracingOptions<TTracingContext> options, ILogger<TTracingContext>? logger)
        {
            _next = next;
            _options = options;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, IAspNetTracingScope<TTracingContext> tracingScope)
        {
            bool isValid = await tracingScope.ValidateHeaderAsync(context);

            if (!isValid)
            {
                return;
            }

            tracingScope.TrySetId();

            if (_logger == null || !_options.AttachToLoggingScope)
            {
                await _next(context);
            }
            else
            {
                Dictionary<string, string> state = new Dictionary<string, string>
                {
                    [_options.LoggingScopeKey] = tracingScope.Id
                };

                using (_logger.BeginScope(state))
                {
                    await _next(context);
                }
            }
        }
    }
}
