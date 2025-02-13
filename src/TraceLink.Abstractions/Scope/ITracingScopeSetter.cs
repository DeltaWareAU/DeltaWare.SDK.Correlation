using TraceLink.Abstractions.Context;

namespace TraceLink.Abstractions.Scope
{
    public interface ITracingScopeSetter<in TTracingContext> where TTracingContext : struct, ITracingContext
    {
        void SetTracingScope<TTracingScope>(TTracingScope tracingScope) where TTracingScope : ITracingScope<TTracingContext>;
    }
}
