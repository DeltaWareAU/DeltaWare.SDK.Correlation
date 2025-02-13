using System;
using TraceLink.Abstractions.Context;

namespace TraceLink.Abstractions.Outbound
{
    public interface IOutboundTracingIdProvider<TTracingContext> where TTracingContext : struct, ITracingContext
    {
        Guid GetOutboundTracingId();
    }
}
