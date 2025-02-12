using TraceLink.Abstractions.Context;

namespace TraceLink.Abstractions.Scope
{
    public interface ITracingScope<out TContext> where TContext : ITracingContext
    {
        TContext? Context { get; }
    }
}
