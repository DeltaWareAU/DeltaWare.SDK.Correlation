using DeltaWare.SDK.Correlation.Context;
using DeltaWare.SDK.Correlation.Context.Accessors;
using DeltaWare.SDK.Correlation.Providers;

namespace DeltaWare.SDK.Correlation.Forwarder
{
    public class DefaultTraceIdForwarder : BaseIdForwarder<TraceContext>
    {
        public DefaultTraceIdForwarder(IContextAccessor<TraceContext> contextAccessor, IIdProvider<TraceContext> idProvider) : base(contextAccessor, idProvider)
        {
        }

        public override string GetForwardingId() => IdProvider.GenerateId();
    }
}
