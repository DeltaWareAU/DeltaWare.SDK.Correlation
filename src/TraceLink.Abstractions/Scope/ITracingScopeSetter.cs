using TraceLink.Abstractions.Context;

namespace TraceLink.Abstractions.Scope
{
    public interface ITracingScopeSetter<in TContext> where TContext : ITracingContext
    {
        void SetScope<TContextScope>(TContextScope scope) where TContextScope : ITracingScope<TContext>;
    }
}
