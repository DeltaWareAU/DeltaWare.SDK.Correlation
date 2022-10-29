using DeltaWare.SDK.Correlation.Context;
using DeltaWare.SDK.Correlation.Context.Accessors;
using DeltaWare.SDK.Correlation.Context.Scope;
using DeltaWare.SDK.Correlation.Options;
using Microsoft.AspNetCore.Http;
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

        public AspNetTraceContextScope(ITraceOptions options, IHttpContextAccessor httpContextAccessor, ILogger<ICorrelationContextAccessor>? logger = null)
        {
            _options = options;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;

            if (!TryGetId(out string? traceId))
            {
                _logger?.LogDebug("No TraceId was attached to the RequestHeaders.");

                Context = new TraceContext();
            }
            else
            {
                _logger?.LogTrace("A TraceId {TraceId} was attached to the RequestHeaders.", traceId);

                Context = new TraceContext(traceId!);
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
    }
}
