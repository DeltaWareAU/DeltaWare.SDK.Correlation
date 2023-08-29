using Microsoft.Extensions.Logging;
using NServiceBus.Pipeline;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Context.Scope;
using TraceLink.Abstractions.Options;
using TraceLink.Abstractions.Providers;

namespace TraceLink.NServiceBus.Context.Scopes
{
    internal sealed class CorrelationNServiceBusTracingScope : NServiceBusTracingScope<CorrelationContext>
    {
        public override bool ReceivedId { get; }
        public override string Id => Context.Id;
        public override CorrelationContext Context { get; }

        public CorrelationNServiceBusTracingScope(ITracingScopeSetter<CorrelationContext> tracingScopeSetter, IIdProvider<CorrelationContext> idProvider, ITracingOptions<CorrelationContext> options, IIncomingPhysicalMessageContext context, ILogger? logger = null) : base(tracingScopeSetter, options, context, logger)
        {
            if (!TryGetId(out string? correlationId))
            {
                ReceivedId = false;

                correlationId = idProvider.GenerateId();

                Logger?.LogTrace("No CorrelationId was attached to the Incoming Transport Message Headers. A new CorrelationId has been generated. {CorrelationId}", correlationId);
            }
            else
            {
                ReceivedId = true;

                Logger?.LogDebug("A CorrelationId {CorrelationId} was attached to the Incoming Transport Message Headers.", correlationId);
            }

            Context = new CorrelationContext(correlationId!);
        }

        protected override void OnValidationPassed()
        {
            Logger?.LogDebug("Header Validation Passed. A CorrelationId {CorrelationId} was received in the Incoming Transport Message Headers.", Context.Id);
        }

        protected override void OnValidationFailed()
        {
            Logger?.LogWarning("Header Validation Failed. A CorrelationId was not received in the Incoming Transport Message Headers.");
        }
    }
}
