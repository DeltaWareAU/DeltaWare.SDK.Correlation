using TraceLink.Abstractions.Context;

namespace TraceLink.Abstractions.Scope
{
    public interface ITracingScope<out TTracingContext> where TTracingContext : struct, ITracingContext
    {
        TTracingContext Context { get; }
    }
}
