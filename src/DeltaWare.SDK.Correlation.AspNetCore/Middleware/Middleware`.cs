using DeltaWare.SDK.Correlation.AspNetCore.Context.Scopes;
using DeltaWare.SDK.Correlation.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeltaWare.SDK.Correlation.AspNetCore.Middleware
{
    internal class ContextMiddleware<TContext> where TContext : class
    {
        private readonly RequestDelegate _next;
        private readonly IOptions _options;
        private readonly ILogger? _logger;

        public ContextMiddleware(RequestDelegate next, IOptions<TContext> options, ILogger<ContextMiddleware<TContext>>? logger)
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
