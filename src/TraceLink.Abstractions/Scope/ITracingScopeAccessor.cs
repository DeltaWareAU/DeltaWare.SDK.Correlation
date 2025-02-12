using TraceLink.Abstractions.Context;

namespace TraceLink.Abstractions.Scope
{
    public interface ITracingScopeAccessor<out TContext> where TContext : ITracingContext
    {
        ITracingScope<TContext> Scope { get; }
    }
}
