using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Context.Accessors;
using TraceLink.Abstractions.Providers;

namespace TraceLink.Abstractions.Forwarder
{
    public sealed class DefaultTraceIdForwarder : BaseIdForwarder<TraceContext>
    {
        public DefaultTraceIdForwarder(IContextAccessor<TraceContext> contextAccessor, IIdProvider<TraceContext> idProvider) : base(contextAccessor, idProvider)
        {
        }

        public override string GetForwardingId() => IdProvider.GenerateId();
    }
}
