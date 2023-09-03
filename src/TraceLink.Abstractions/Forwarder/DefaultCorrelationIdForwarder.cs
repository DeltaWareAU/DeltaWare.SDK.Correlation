using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Context.Accessors;

namespace TraceLink.Abstractions.Forwarder
{
    public sealed class DefaultCorrelationIdForwarder : IIdForwarder<CorrelationContext>
    {
        private readonly ITracingContextAccessor<CorrelationContext> _contextAccessor;

        public DefaultCorrelationIdForwarder(ITracingContextAccessor<CorrelationContext> contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string GetForwardingId() => _contextAccessor.Context.Id;
    }
}
