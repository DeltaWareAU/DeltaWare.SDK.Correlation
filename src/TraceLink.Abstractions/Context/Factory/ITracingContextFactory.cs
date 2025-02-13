using System;

namespace TraceLink.Abstractions.Context.Factory
{
    public interface ITracingContextFactory<out TTracingContext> where TTracingContext : struct, ITracingContext
    {
        TTracingContext CreateContext(Guid tracingId);
    }
}
