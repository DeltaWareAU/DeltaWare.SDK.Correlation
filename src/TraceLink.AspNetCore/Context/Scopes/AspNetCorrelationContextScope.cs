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
    internal sealed class AspNetCorrelationContextScope : AspNetContextScope<CorrelationContext>
    {
        public override CorrelationContext Context { get; }
        public override bool DidReceiveContextId { get; }
        public override string ContextId => Context.CorrelationId;

        public AspNetCorrelationContextScope(IContextScopeSetter<CorrelationContext> contextScopeSetter, IOptions<CorrelationContext> options, IIdProvider<CorrelationContext> idProvider, IHttpContextAccessor httpContextAccessor, ILogger<CorrelationContext>? logger = null) : base(contextScopeSetter, options, httpContextAccessor, logger)
        {
            if (!TryGetId(out string? correlationId))
            {
                DidReceiveContextId = false;

                correlationId = idProvider.GenerateId();

                Logger?.LogTrace("No CorrelationId was attached to the RequestHeaders. A new CorrelationId has been generated. {CorrelationId}", correlationId);
            }
            else
            {
                DidReceiveContextId = true;

                Logger?.LogDebug("A CorrelationId {CorrelationId} was attached to the RequestHeaders.", correlationId);
            }

            Context = new CorrelationContext(correlationId!);

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

            if (!context.Features.HasFeature<CorrelationIdHeaderRequiredAttribute>())
            {
                return false;
            }

            Logger?.LogTrace("Header Validation will be done as the CorrelationIdHeaderRequiredAttribute is present.");

            return true;
        }

        protected override bool ShouldAttachToResponse(HttpContext context)
        {
            if (base.ShouldAttachToResponse(context))
            {
                return true;
            }

            if (!context.Features.HasFeature<AttachCorrelationIdToResponseHeaderAttribute>())
            {
                return false;
            }

            Logger?.LogTrace("TraceId will be attached to the response headers as the AttachCorrelationIdToResponseHeaderAttribute is present.");

            return true;
        }

        protected override void OnMultipleIdsFounds(string[] foundIds)
        {
            Logger?.LogWarning("Multiple CorrelationIds found ({CorrelationIds}), only the first value will be used.", string.Join(',', foundIds));
        }

        protected override void OnIdAttached(string id)
        {
            Logger?.LogDebug("Correlation ID {CorrelationId} has been attached to the Response Headers", id);
        }

        protected override bool CanSkipValidation(HttpContext context)
        {
            if (!context.Features.HasFeature<CorrelationIdHeaderNotRequiredAttribute>())
            {
                return false;
            }

            Logger?.LogTrace("Key Validation will be skipped as the CorrelationIdHeaderNotRequiredAttribute is present.");

            return true;
        }

        protected override void OnValidationPassed()
        {
            Logger?.LogDebug("Header Validation Passed. A CorrelationId {CorrelationId} was received in the HttpRequest Headers", Context.CorrelationId);
        }

        protected override void OnValidationFailed()
        {
            Logger?.LogWarning("Header Validation Failed. A CorrelationId was not received in the HttpRequest Headers, responding with 400 (Bad Request).");
        }
    }
}
