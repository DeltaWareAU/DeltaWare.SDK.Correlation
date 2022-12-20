using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Context.Accessors;
using TraceLink.Abstractions.Providers;

namespace TraceLink.Abstractions.Forwarder
{
    public sealed class DefaultCorrelationIdForwarder : BaseIdForwarder<CorrelationContext>
    {
        public DefaultCorrelationIdForwarder(IContextAccessor<CorrelationContext> contextAccessor, IIdProvider<CorrelationContext> idProvider) : base(contextAccessor, idProvider)
        {
        }

        public override string GetForwardingId() => ContextAccessor.Context.CorrelationId;
    }
}
