using DeltaWare.SDK.Correlation.Context;
using DeltaWare.SDK.Correlation.Context.Accessors;
using DeltaWare.SDK.Correlation.Providers;

namespace DeltaWare.SDK.Correlation.Forwarder
{
    public sealed class DefaultCorrelationIdForwarder : BaseIdForwarder<CorrelationContext>
    {
        public DefaultCorrelationIdForwarder(IContextAccessor<CorrelationContext> contextAccessor, IIdProvider<CorrelationContext> idProvider) : base(contextAccessor, idProvider)
        {
        }

        public override string GetForwardingId() => ContextAccessor.Context.CorrelationId;
    }
}
