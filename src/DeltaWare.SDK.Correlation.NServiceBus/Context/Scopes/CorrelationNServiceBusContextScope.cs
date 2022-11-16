using DeltaWare.SDK.Correlation.Context;
using DeltaWare.SDK.Correlation.Context.Accessors;
using DeltaWare.SDK.Correlation.Options;
using DeltaWare.SDK.Correlation.Providers;
using Microsoft.Extensions.Logging;
using NServiceBus.Pipeline;

namespace DeltaWare.SDK.Correlation.NServiceBus.Context.Scopes
{
    internal class CorrelationNServiceBusContextScope : NServiceBusContextScope<CorrelationContext>
    {
        public override bool DidReceiveContextId { get; }
        public override string ContextId { get; }
        public override CorrelationContext Context { get; }

        public CorrelationNServiceBusContextScope(ContextScopeSetter<CorrelationContext> contextScopeSetter, IIdProvider<CorrelationContext> idProvider, IOptions<CorrelationContext> options, IIncomingPhysicalMessageContext context, ILogger logger = null) : base(contextScopeSetter, options, context, logger)
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
