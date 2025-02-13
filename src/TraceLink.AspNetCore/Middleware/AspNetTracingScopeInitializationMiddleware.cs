using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Options;
using TraceLink.Abstractions.Scope;
using TraceLink.AspNetCore.Scope;

namespace TraceLink.AspNetCore.Middleware
{
    internal sealed class AspNetTracingScopeInitializationMiddleware<TTracingContext> where TTracingContext : struct, ITracingContext
    {
        private readonly RequestDelegate _next;
        private readonly ITracingOptions<TTracingContext> _options;
        private readonly ILogger? _logger;

        public AspNetTracingScopeInitializationMiddleware(RequestDelegate next, ITracingOptions<TTracingContext> options, ILogger<AspNetTracingScopeInitializationMiddleware<TTracingContext>>? logger = null)
        {
            _next = next;
            _options = options;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext, IAspNetTracingScope<TTracingContext> tracingScope, ITracingScopeSetter<TTracingContext> scopeSetter, ITracingScopeAccessor<TTracingContext> scopeAccessor, CancellationToken cancellationToken = default)
        {
            if (!tracingScope.TryInitializeTracingId(httpContext))
            {
                await RespondToMissingTracingAsync(httpContext, cancellationToken);

                return;
            }

            tracingScope.InitializeScope();

            scopeSetter.SetTracingScope(tracingScope);

            if (_options.AttachToResponse)
            {
                httpContext.Response.Headers.Append(_options.Key, scopeAccessor.Scope.Context.Id.ToString());
            }

            if (_logger == null || !_options.AttachToLoggingScope)
            {
                await _next(httpContext);

                return;
            }

            using (_logger.BeginScope(GetLoggingState(scopeAccessor)))
            {
                await _next(httpContext);
            }
        }

        private IReadOnlyDictionary<string, string> GetLoggingState(ITracingScopeAccessor<TTracingContext> scopeAccessor)
            => new Dictionary<string, string>()
            {
                [_options.LoggingScopeKey] = scopeAccessor.Scope.Context!.Id.ToString()
            };

        private async Task RespondToMissingTracingAsync(HttpContext httpContext, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            httpContext.Response.ContentType = "application/json";

            // Craft a more detailed JSON error response
            var errorResponse = new
            {
                error = new
                {
                    code = httpContext.Response.StatusCode,
                    message = "Bad Request",
                    details = $"The request headers must contain the \"{_options.Key}\" key to proceed."
                }
            };

            string responseContent = JsonSerializer.Serialize(errorResponse);

            await httpContext.Response.WriteAsync(responseContent, cancellationToken);

            // Enhanced logging with structured details
            _logger?.LogWarning("Responding with 400 (Bad Request) as the incoming HTTP request did not have the required '{HeaderKey}' header.", _options.Key);
        }
    }
}
