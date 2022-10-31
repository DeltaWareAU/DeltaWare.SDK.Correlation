using DeltaWare.SDK.Correlation.Context;
using DeltaWare.SDK.Correlation.Context.Accessors;
using DeltaWare.SDK.Correlation.Context.Scope;
using DeltaWare.SDK.Correlation.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Threading.Tasks;
using DeltaWare.SDK.Correlation.AspNetCore.Attributes;
using DeltaWare.SDK.Correlation.AspNetCore.Extensions;

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

        public void TrySetId(bool force = false)
        {
            if (!force && !_options.AttachToResponse)
            {
                return;
            }

            if (string.IsNullOrEmpty(Context.TraceId))
            {
                return;
            }

            TrySetId(Context.TraceId);
        }

        public void TrySetId(string traceId)
        {
            _httpContextAccessor.HttpContext.Response.OnStarting(() =>
            {
                if (_httpContextAccessor.HttpContext.Response.Headers.ContainsKey(_options.Header))
                {
                    return Task.CompletedTask;
                }

                _httpContextAccessor.HttpContext.Response.Headers.Add(_options.Header, traceId);

                _logger?.LogDebug("Trace ID {TraceId} has been attached to the Response Headers", traceId);

                return Task.CompletedTask;
            });
        }

        public async Task<bool> ValidateHeaderAsync(HttpContext context, bool force = false)
        {
            if (!force)
            {
                if (context.Features.HasFeature<TraceIdHeaderNotRequiredAttribute>())
                {
                    _logger.LogTrace("Header Validation will be skipped as the TraceIdHeaderNotRequiredAttribute is present.");

                    return true;
                }

                if (!_options.IsRequired)
                {
                    _logger.LogTrace("Header Validation will be skipped as it is not required.");

                    return true;
                }
            }
            
            if (ReceivedId)
            {
                _logger?.LogDebug("Header Validation Passed. A TraceId {TraceId} was received in the HttpRequest Headers", Context.TraceId);

                return true;
            }

            _logger?.LogWarning("Header Validation Failed. A TraceId was not received in the HttpRequest Headers, responding with 400 (Bad Request).");

            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            await context.Response.WriteAsync($"The Request Headers must contain the \"{_options.Header}\" Header.");

            return false;
        }
    }
}
