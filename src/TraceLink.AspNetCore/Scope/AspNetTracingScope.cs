using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Context.Factory;
using TraceLink.Abstractions.Options;
using TraceLink.AspNetCore.Enum;
using TraceLink.AspNetCore.Validation;

namespace TraceLink.AspNetCore.Scope
{
    internal sealed class AspNetTracingScope<TTracingContext> : IAspNetTracingScope<TTracingContext> where TTracingContext : struct, ITracingContext
    {
        private readonly ITracingContextFactory<TTracingContext> _contextFactory;
        private readonly IHeaderValidationManager _headerValidationManager;
        private readonly ITracingOptions<TTracingContext> _options;
        private readonly ILogger<AspNetTracingScope<TTracingContext>>? _logger;

        private Guid? _tracingId;

        public TTracingContext Context { get; private set; }

        public AspNetTracingScope(ITracingContextFactory<TTracingContext> contextFactory, IHeaderValidationManager headerValidationManager, ITracingOptions<TTracingContext> options, ILogger<AspNetTracingScope<TTracingContext>>? logger = null)
        {
            _contextFactory = contextFactory;
            _headerValidationManager = headerValidationManager;
            _options = options;
            _logger = logger;
        }

        public bool TryInitializeScope(HttpContext httpContext)
        {
            var validationRequirements = GetHeaderValidationRequirements(httpContext);

            if (validationRequirements == HeaderValidationRequirements.Default && _options.IsRequired)
            {
                validationRequirements = HeaderValidationRequirements.Required;
            }

            bool isHeaderRequired = validationRequirements != HeaderValidationRequirements.Required;

            if (TryGetTracingId(httpContext, isHeaderRequired, out var tracingId))
            {
                Context = InitializeContext(tracingId);

                return true;
            }

            if (isHeaderRequired)
            {
                return false;
            }

            _logger?.LogDebug("Generating TraceId as it was not present on the HttpRequest Headers.");

            Context = InitializeContext(GenerateTracingId());

            return true;

        }
        
        private bool TryGetTracingId(HttpContext httpContext, bool logWarning, out Guid tracingId)
        {
            tracingId = Guid.Empty;

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

            if (!Guid.TryParse(headerValue, out tracingId))
            {
                LogWarningIfRequired(logWarning, "could not be parsed as a GUID.");

                return false;
            }

            _logger?.LogDebug("Header Validation Successful.");

            return true;
        }

        private TTracingContext InitializeContext(Guid tracingId)
            => _contextFactory.CreateContext(tracingId);

        private HeaderValidationRequirements GetHeaderValidationRequirements(HttpContext httpContext)
            => _headerValidationManager.GetHeaderValidationRequirements<TTracingContext>(httpContext);

        private Guid GenerateTracingId()
            => Guid.NewGuid();

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
