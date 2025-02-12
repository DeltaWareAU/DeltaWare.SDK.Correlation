using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Options;
using TraceLink.AspNetCore.Scope;

namespace TraceLink.AspNetCore.Middleware
{
    internal sealed class AspNetTracingMiddleware<TContext> where TContext : ITracingContext
    {
        private readonly RequestDelegate _next;
        private readonly ITracingOptions<TContext> _options;
        private readonly ILogger? _logger;

        public AspNetTracingMiddleware(RequestDelegate next, ITracingOptions<TContext> options, ILogger<AspNetTracingMiddleware<TContext>>? logger)
        {
            _next = next;
            _options = options;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, IAspNetTracingScope<TContext> tracingScope)
        {

        }
    }
}
