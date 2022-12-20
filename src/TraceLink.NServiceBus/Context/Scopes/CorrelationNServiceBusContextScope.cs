using Microsoft.Extensions.Logging;
using NServiceBus.Pipeline;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Context.Scope;
using TraceLink.Abstractions.Options;
using TraceLink.Abstractions.Providers;

namespace TraceLink.NServiceBus.Context.Scopes
{
    internal sealed class CorrelationNServiceBusContextScope : NServiceBusContextScope<CorrelationContext>
    {
        public override bool DidReceiveContextId { get; }
        public override string ContextId => Context.CorrelationId;
        public override CorrelationContext Context { get; }

        public CorrelationNServiceBusContextScope(IContextScopeSetter<CorrelationContext> contextScopeSetter, IIdProvider<CorrelationContext> idProvider, IOptions<CorrelationContext> options, IIncomingPhysicalMessageContext context, ILogger? logger = null) : base(contextScopeSetter, options, context, logger)
        {
            if (!TryGetId(out string? correlationId))
            {
                DidReceiveContextId = false;

                correlationId = idProvider.GenerateId();

                Logger?.LogTrace("No CorrelationId was attached to the Incoming Transport Message Headers. A new CorrelationId has been generated. {CorrelationId}", correlationId);
            }
            else
            {
                DidReceiveContextId = true;

                Logger?.LogDebug("A CorrelationId {CorrelationId} was attached to the Incoming Transport Message Headers.", correlationId);
            }

            Context = new CorrelationContext(correlationId!);
        }

        protected override void OnValidationPassed()
        {
            Logger?.LogDebug("Header Validation Passed. A CorrelationId {CorrelationId} was received in the Incoming Transport Message Headers.", Context.CorrelationId);
        }

        protected override void OnValidationFailed()
        {
            Logger?.LogWarning("Header Validation Failed. A CorrelationId was not received in the Incoming Transport Message Headers.");
        }
    }
}
