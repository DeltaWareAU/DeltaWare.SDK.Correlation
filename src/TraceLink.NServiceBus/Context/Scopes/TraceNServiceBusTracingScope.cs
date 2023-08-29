using Microsoft.Extensions.Logging;
using NServiceBus.Pipeline;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Context.Scope;
using TraceLink.Abstractions.Options;

namespace TraceLink.NServiceBus.Context.Scopes
{
    internal sealed class TraceNServiceBusTracingScope : NServiceBusTracingScope<TraceContext>
    {
        public override bool ReceivedId { get; }
        public override string Id => Context?.Id ?? string.Empty;
        public override TraceContext Context { get; }

        public TraceNServiceBusTracingScope(ITracingScopeSetter<TraceContext> tracingScopeSetter, ITracingOptions<TraceContext> options, IIncomingPhysicalMessageContext context, ILogger? logger = null) : base(tracingScopeSetter, options, context, logger)
        {
            if (!TryGetId(out string? traceId))
            {
                ReceivedId = false;

                logger?.LogDebug("No TraceId was attached to the Incoming Transport Message Headers.");
            }
            else
            {
                ReceivedId = true;

                logger?.LogTrace("A TraceId {TraceId} was attached to the Incoming Transport Message Headers.", traceId);

                Context = new TraceContext(traceId!);
            }
        }

        protected override void OnValidationPassed()
        {
            Logger?.LogDebug("Header Validation Passed. A TraceId {TraceId} was received in the Incoming Transport Message Headers.", Context.Id);
        }

        protected override void OnValidationFailed()
        {
            Logger?.LogWarning("Header Validation Failed. A TraceId was not received in the Incoming Transport Message Headers.");
        }
    }
}
