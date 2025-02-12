using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Options;
using TraceLink.AspNetCore.Enum;

namespace TraceLink.AspNetCore.Scope
{
    internal abstract class AspNetTracingScope<TContext> : IAspNetTracingScope<TContext> where TContext : ITracingContext
    {
        private readonly ITracingOptions<TContext> _options;
        private readonly ILogger? _logger;

        private Guid? _receivedTracingId;

        protected AspNetTracingScope(ITracingOptions<TContext> options, ILogger? logger = null)
        {
            _options = options;
            _logger = logger;
        }

        public void InitializeScope()
        {
            if (!_receivedTracingId.HasValue)
            {
                return;
            }

            InitializeScope(_receivedTracingId.Value);
        }

        protected abstract void InitializeScope(Guid tracingId);

        public async Task<bool> ValidateHeaderAsync(HttpContext httpContext, CancellationToken cancellationToken = default)
        {
            if (ValidateHeader(httpContext))
            {
                return true;
            }

            await OnValidationFailedAsync(httpContext, cancellationToken);

            return false;
        }

        private async Task OnValidationFailedAsync(HttpContext httpContext, CancellationToken cancellationToken)
        {
            var validationRequirements = GetHeaderValidationRequirements(httpContext);

            if (validationRequirements == HeaderValidationRequirements.Optional)
            {
                return;
            }

            if (validationRequirements != HeaderValidationRequirements.Required && !_options.IsRequired)
            {
                return;
            }

            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            await httpContext.Response.WriteAsync($"The Request Headers must contain the \"{_options.Key}\" Key.", cancellationToken);
        }

        private bool ValidateHeader(HttpContext httpContext)
        {
            if (!httpContext.Request.Headers.TryGetValue(_options.Key, out var headerValues))
            {
                _logger?.LogWarning("Header Validation Failed - The header {HeaderKey} was no present.", _options.Key);

                return false;
            }

            if (StringValues.IsNullOrEmpty(headerValues))
            {
                _logger?.LogWarning("Header Validation Failed - The header {HeaderKey} was present but wasn't assigned a value.", _options.Key);

                return false;
            }

            if (headerValues.Count > 1)
            {
                _logger?.LogWarning("Header Validation Failed - The header {HeaderKey} had more than one value assigned.", _options.Key);

                return false;
            }

            var headerValue = headerValues.First();

            if (!Guid.TryParse(headerValue, out var tracingId))
            {
                _logger?.LogWarning("Header Validation Failed - The header {HeaderKey} could not be parsed.", _options.Key);

                return false;
            }

            _receivedTracingId = tracingId;

            _logger?.LogDebug("Header Validation Successful.");

            return true;
        }

        protected abstract HeaderValidationRequirements GetHeaderValidationRequirements(HttpContext httpContext);
    }
}
