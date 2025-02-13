using System;
using TraceLink.Abstractions.Context;

namespace TraceLink.Abstractions.Outgoing
{
    public interface IOutgoingTracingIdProvider<TTracingContext> where TTracingContext : struct, ITracingContext
    {
        Guid GetOutboundTracingId();
    }
}
