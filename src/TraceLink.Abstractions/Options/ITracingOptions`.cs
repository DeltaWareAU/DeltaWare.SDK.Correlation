using TraceLink.Abstractions.Context;

namespace TraceLink.Abstractions.Options
{
    public interface ITracingOptions<TContext> : ITracingOptions where TContext : ITracingContext
    {
    }
}
