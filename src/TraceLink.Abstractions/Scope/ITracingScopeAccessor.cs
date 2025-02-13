using TraceLink.Abstractions.Context;

namespace TraceLink.Abstractions.Scope
{
    public interface ITracingScopeAccessor<out TTracingContext> where TTracingContext : struct, ITracingContext
    {
        ITracingScope<TTracingContext> Scope { get; }
    }
}
