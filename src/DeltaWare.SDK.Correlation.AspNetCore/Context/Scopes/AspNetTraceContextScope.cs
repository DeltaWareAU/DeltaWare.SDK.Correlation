using DeltaWare.SDK.Correlation.AspNetCore.Attributes;
using DeltaWare.SDK.Correlation.AspNetCore.Extensions;
using DeltaWare.SDK.Correlation.Context;
using DeltaWare.SDK.Correlation.Context.Accessors;
using DeltaWare.SDK.Correlation.Options;
using DeltaWare.SDK.Correlation.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DeltaWare.SDK.Correlation.AspNetCore.Context.Scopes
{
    internal sealed class AspNetTraceContextScope : BaseAspNetContextScope<TraceContext>
    {
        public override TraceContext Context { get; }

        public override bool DidReceiveContextId { get; }

        public override string ContextId => Context.TraceId ?? string.Empty;

        public AspNetTraceContextScope(ContextAccessor<TraceContext> contextAccessor, IOptions<TraceContext> options, IIdProvider<TraceContext> idProvider, IHttpContextAccessor httpContextAccessor, ILogger<AspNetTraceContextScope>? logger = null) : base(contextAccessor, options, idProvider, httpContextAccessor, logger)
        {
            if (!TryGetId(out string? traceId))
            {
                DidReceiveContextId = false;

                logger?.LogDebug("No TraceId was attached to the RequestHeaders.");

                Context = new TraceContext();

                return;
            }

            DidReceiveContextId = true;

            logger?.LogTrace("A TraceId {TraceId} was attached to the RequestHeaders.", traceId);

            Context = new TraceContext(traceId!);

            if (options.AttachToResponse)
            {
                TrySetId();
            }
        }

        protected override void OnMultipleIdsFounds(string[] foundIds)
        {
            Logger?.LogWarning("Multiple TraceIds found ({TraceIds}), only the first will be used.", string.Join(',', foundIds));
        }

        protected override void OnIdAttached(string id)
        {
            Logger?.LogDebug("Trace ID {TraceId} has been attached to the Response Headers", id);
        }

        protected override bool CanSkipValidation(HttpContext context)
        {
            if (!context.Features.HasFeature<TraceIdHeaderNotRequiredAttribute>())
            {
                return false;
            }

            Logger?.LogTrace("Key Validation will be skipped as the TraceIdHeaderNotRequiredAttribute is present.");

            return true;

        }

        protected override void OnValidationPassed()
        {
            Logger?.LogDebug("Key Validation Passed. A TraceId {TraceId} was received in the HttpRequest Headers", Context.TraceId);
        }

        protected override void OnValidationFailed()
        {
            Logger?.LogWarning("Key Validation Failed. A TraceId was not received in the HttpRequest Headers, responding with 400 (Bad Request).");
        }
    }
}
