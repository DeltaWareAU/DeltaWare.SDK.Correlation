using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Providers;

namespace TraceLink.Abstractions.Forwarder
{
    public sealed class DefaultTraceIdForwarder : IIdForwarder<TraceContext>
    {
        private readonly IIdProvider _idProvider;

        public DefaultTraceIdForwarder(IIdProvider<TraceContext> idProvider)
        {
            _idProvider = idProvider;
        }

        public string GetForwardingId() => _idProvider.GenerateId();
    }
}
