using DeltaWare.SDK.Correlation.AspNetCore.Attributes;
using DeltaWare.SDK.Correlation.AspNetCore.Extensions;
using DeltaWare.SDK.Correlation.Context;
using DeltaWare.SDK.Correlation.Context.Accessors;
using DeltaWare.SDK.Correlation.Context.Scope;
using DeltaWare.SDK.Correlation.Options;
using DeltaWare.SDK.Correlation.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Threading.Tasks;

namespace DeltaWare.SDK.Correlation.AspNetCore.Context.Scopes
{
    internal sealed class AspNetCorrelationContextScope : IContextScope<CorrelationContext>
    {
        private readonly ICorrelationOptions _options;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ICorrelationContextAccessor>? _logger;

        public CorrelationContext Context { get; }

        public bool ReceivedId { get; }

        public AspNetCorrelationContextScope(CorrelationContextAccessor contextAccessor, ICorrelationOptions options, ICorrelationIdProvider idProvider, IHttpContextAccessor httpContextAccessor, ILogger<ICorrelationContextAccessor>? logger = null)
        {
            contextAccessor.InternalScope = this;

            _options = options;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;

            if (!TryGetId(out string? correlationId))
            {
                ReceivedId = false;

                correlationId = idProvider.GenerateId();

                _logger?.LogTrace("No CorrelationId was attached to the RequestHeaders. A new CorrelationId has been generated. {CorrelationId}", correlationId);
            }
            else
            {
                ReceivedId = true;

                _logger?.LogDebug("A CorrelationId {CorrelationId} was attached to the RequestHeaders.", correlationId);
            }

            Context = new CorrelationContext(correlationId!);

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
                _logger?.LogWarning("Multiple CorrelationIds found ({CorrelationIds}), only the first value will be used.", values.ToString());
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

            TrySetId(Context.CorrelationId);
        }

        public void TrySetId(string correlationId)
        {
            _httpContextAccessor.HttpContext.Response.OnStarting(() =>
            {
                if (_httpContextAccessor.HttpContext.Response.Headers.ContainsKey(_options.Header))
                {
                    return Task.CompletedTask;
                }

                _httpContextAccessor.HttpContext.Response.Headers.Add(_options.Header, correlationId);

                _logger?.LogDebug("Correlation ID {CorrelationId} has been attached to the Response Headers", correlationId);

                return Task.CompletedTask;
            });
        }

        public async Task<bool> ValidateHeaderAsync(HttpContext context, bool force = false)
        {
            if (!force)
            {
                if (context.Features.HasFeature<CorrelationIdHeaderNotRequiredAttribute>())
                {
                    _logger.LogTrace("Header Validation will be skipped as the CorrelationIdHeaderNotRequiredAttribute is present.");

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
                _logger?.LogDebug("Header Validation Passed. A CorrelationId {CorrelationId} was received in the HttpRequest Headers", Context.CorrelationId);

                return true;
            }

            _logger?.LogWarning("Header Validation Failed. A CorrelationId was not received in the HttpRequest Headers, responding with 400 (Bad Request).");

            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            await context.Response.WriteAsync($"The Request Headers must contain the \"{_options.Header}\" Header.");

            return false;
        }
    }
}
