using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using TraceLink.Abstractions.Options;
using TraceLink.AspNetCore.Context.Scopes;

namespace TraceLink.AspNetCore.Middleware
{
    internal sealed class ContextMiddleware<TContext> where TContext : class
    {
        private readonly RequestDelegate _next;
        private readonly IOptions _options;
        private readonly ILogger? _logger;

        public ContextMiddleware(RequestDelegate next, IOptions<TContext> options, ILogger<TContext>? logger)
        {
            _next = next;
            _options = options;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, IAspNetContextScope<TContext> contextScope)
        {
            bool isValid = await contextScope.ValidateHeaderAsync(context);

            if (!isValid)
            {
                return;
            }

            contextScope.TrySetId();

            if (_logger == null || !_options.AttachToLoggingScope)
            {
                await _next(context);
            }
            else
            {
                Dictionary<string, string> state = new Dictionary<string, string>
                {
                    [_options.LoggingScopeKey] = contextScope.ContextId
                };

                using (_logger.BeginScope(state))
                {
                    await _next(context);
                }
            }
        }
    }
}
