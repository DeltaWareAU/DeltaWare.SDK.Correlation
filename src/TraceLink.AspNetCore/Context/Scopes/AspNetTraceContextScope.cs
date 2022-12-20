using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Context.Scope;
using TraceLink.Abstractions.Options;
using TraceLink.Abstractions.Providers;
using TraceLink.AspNetCore.Attributes;
using TraceLink.AspNetCore.Extensions;

namespace TraceLink.AspNetCore.Context.Scopes
{
    internal sealed class AspNetTraceContextScope : AspNetContextScope<TraceContext>
    {
        public override TraceContext Context { get; }

        public override bool DidReceiveContextId { get; }

        public override string ContextId => Context.TraceId ?? string.Empty;

        public AspNetTraceContextScope(IContextScopeSetter<TraceContext> contextScopeSetter, IOptions<TraceContext> options, IIdProvider<TraceContext> idProvider, IHttpContextAccessor httpContextAccessor, ILogger<TraceContext>? logger = null) : base(contextScopeSetter, options, httpContextAccessor, logger)
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

        protected override bool IsValidationRequired(HttpContext context)
        {
            if (base.IsValidationRequired(context))
            {
                return true;
            }

            if (!context.Features.HasFeature<TraceIdHeaderRequiredAttribute>())
            {
                return false;
            }

            Logger?.LogTrace("Header Validation will be done as the TraceIdHeaderRequiredAttribute is present.");

            return true;
        }

        protected override bool ShouldAttachToResponse(HttpContext context)
        {
            if (base.ShouldAttachToResponse(context))
            {
                return true;
            }

            if (!context.Features.HasFeature<AttachTraceIdToResponseHeaderAttribute>())
            {
                return false;
            }

            Logger?.LogTrace("TraceId will be attached to the response headers as the AttachTraceIdToResponseHeaderAttribute is present.");

            return true;
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

            Logger?.LogTrace("Header Validation will be skipped as the TraceIdHeaderNotRequiredAttribute is present.");

            return true;

        }

        protected override void OnValidationPassed()
        {
            Logger?.LogDebug("Header Validation Passed. A TraceId {TraceId} was received in the HttpRequest Headers", Context.TraceId);
        }

        protected override void OnValidationFailed()
        {
            Logger?.LogWarning("Header Validation Failed. A TraceId was not received in the HttpRequest Headers, responding with 400 (Bad Request).");
        }
    }
}
