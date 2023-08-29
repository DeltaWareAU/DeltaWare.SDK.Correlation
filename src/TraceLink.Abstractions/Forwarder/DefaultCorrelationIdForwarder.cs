using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Context.Accessors;
using TraceLink.Abstractions.Providers;

namespace TraceLink.Abstractions.Forwarder
{
    public sealed class DefaultCorrelationIdForwarder : IdForwarder<CorrelationContext>
    {
        public DefaultCorrelationIdForwarder(ITracingContextAccessor<CorrelationContext> contextAccessor, IIdProvider<CorrelationContext> idProvider) : base(contextAccessor, idProvider)
        {
        }

        public override string GetForwardingId() => TracingContextAccessor.Context.Id;
    }
}
