using DeltaWare.SDK.Correlation.Context;
using DeltaWare.SDK.Correlation.Context.Accessors;
using DeltaWare.SDK.Correlation.Context.Scope;
using DeltaWare.SDK.Correlation.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace DeltaWare.SDK.Correlation.AspNetCore.Context.Scopes
{
    internal sealed class AspNetTraceContextScope : IContextScope<TraceContext>
    {
        private readonly ITraceOptions _options;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ICorrelationContextAccessor>? _logger;

        public TraceContext Context { get; }

        public bool ReceivedId { get; }

        public AspNetTraceContextScope(TraceContextAccessor contextAccessor, ITraceOptions options, IHttpContextAccessor httpContextAccessor, ILogger<ICorrelationContextAccessor>? logger = null)
        {
            contextAccessor.InternalScope = this;

            _options = options;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;

            if (!TryGetId(out string? traceId))
            {
                ReceivedId = false;

                _logger?.LogDebug("No TraceId was attached to the RequestHeaders.");

                Context = new TraceContext();

                return;
            }

            ReceivedId = true;

            _logger?.LogTrace("A TraceId {TraceId} was attached to the RequestHeaders.", traceId);

            Context = new TraceContext(traceId!);

            if (options.AttachToResponse)
            {
                TrySetId();
            }
        }

        public bool TryGetId(out string? idValue)
        {
            IHeaderDictionary headerDictionary = _httpContextAccessor.HttpContext.Request.Headers;

            if (!headerDictionary.TryGetValue(_options.Header, out StringValues values) || StringValues.IsNullOrEmpty(values))
            {
                idValue = null;

                return false;
            }

            string[] valueArray = values.ToArray();

            if (valueArray.Length > 1)
            {
                _logger?.LogWarning("Multiple TraceIds found ({TraceIds}), only the first will be used.", values.ToString());
            }

            idValue = values.First();

            return true;
        }

        public bool TrySetId(bool force = false)
        {
            if (!force && !_options.AttachToResponse)
            {
                return false;
            }

            return TrySetId(Context.TraceId ?? string.Empty);
        }

        public bool TrySetId(string traceId)
        {
            if (!_httpContextAccessor.HttpContext.Response.Headers.ContainsKey(_options.Header) || string.IsNullOrEmpty(traceId))
            {
                return false;
            }

            _httpContextAccessor.HttpContext.Response.Headers.Add(_options.Header, traceId);

            _logger?.LogDebug("Trace ID {TraceId} has been attached to the Response Headers", traceId);

            return true;
        }

        public void ValidateContext(ActionExecutingContext context, bool force = false)
        {
            if (!force && !_options.IsRequired)
            {
                return;
            }

            if (ReceivedId)
            {
                _logger?.LogDebug("Header Validation Passed. A TraceId {TraceId} was received in the HttpRequest Headers", Context.TraceId);

                return;
            }

            _logger?.LogWarning("Header Validation Failed. A TraceId was not received in the HttpRequest Headers, responding with 400 (Bad Request).");

            context.Result = new BadRequestObjectResult($"The Request Headers must contain the \"{_options.Header}\" Header.");
        }
    }
}
