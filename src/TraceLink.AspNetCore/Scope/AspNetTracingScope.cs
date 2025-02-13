using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Options;
using TraceLink.AspNetCore.Enum;

namespace TraceLink.AspNetCore.Scope
{
    public abstract class AspNetTracingScope<TTracingContext> : IAspNetTracingScope<TTracingContext> where TTracingContext : struct, ITracingContext
    {
        private readonly ITracingOptions<TTracingContext> _options;
        private readonly ILogger? _logger;

        private Guid? _headerTracingId;

        public TTracingContext Context { get; private set; }

        protected AspNetTracingScope(ITracingOptions<TTracingContext> options, ILogger? logger = null)
        {
            _options = options;
            _logger = logger;
        }

        public void InitializeScope()
        {
            if (!_headerTracingId.HasValue)
            {
                return;
            }

            Context = InitializeContext(_headerTracingId.Value);
        }

        protected abstract TTracingContext InitializeContext(Guid tracingId);

        protected abstract HeaderValidationRequirements GetHeaderValidationRequirements(HttpContext httpContext);

        protected virtual Guid GenerateTracingId() => Guid.NewGuid();

        public bool TryInitializeTracingId(HttpContext httpContext)
        {
            var validationRequirements = GetHeaderValidationRequirements(httpContext);

            if (validationRequirements == HeaderValidationRequirements.Default && _options.IsRequired)
            {
                validationRequirements = HeaderValidationRequirements.Required;
            }

            bool isHeaderRequired = validationRequirements != HeaderValidationRequirements.Required;

            if (TryGetTracingId(httpContext, isHeaderRequired))
            {
                return true;
            }

            if (isHeaderRequired)
            {
                return false;
            }

            _logger?.LogDebug("Generating TraceId as it was not present on the HttpRequest Headers.");

            _headerTracingId = GenerateTracingId();

            return true;

        }

        private bool TryGetTracingId(HttpContext httpContext, bool logWarning)
        {
            if (!httpContext.Request.Headers.TryGetValue(_options.Key, out var headerValues))
            {
                LogWarningIfRequired(logWarning, "was not present.");

                return false;
            }

            if (StringValues.IsNullOrEmpty(headerValues))
            {
                LogWarningIfRequired(logWarning, "was present but empty.");

                return false;
            }

            if (headerValues.Count > 1)
            {
                LogWarningIfRequired(logWarning, "had more than one value.");

                return false;
            }

            var headerValue = headerValues.First();

            if (!Guid.TryParse(headerValue, out var tracingId))
            {
                LogWarningIfRequired(logWarning, "could not be parsed as a GUID.");

                return false;
            }

            _headerTracingId = tracingId;

            _logger?.LogDebug("Header Validation Successful.");

            return true;
        }

        private void LogWarningIfRequired(bool required, string message)
        {
            if (!required)
            {
                return;
            }

            _logger?.LogWarning("Tracing Header Validation Failed - The Header {HeaderKey} {Message}", _options.Key, message);
        }
    }
}
