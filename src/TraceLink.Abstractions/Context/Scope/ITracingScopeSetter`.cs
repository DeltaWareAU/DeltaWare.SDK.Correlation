namespace TraceLink.Abstractions.Context.Scope
{
    public interface ITracingScopeSetter<in TTracingContext> where TTracingContext : ITracingContext
    {
        void SetScope(ITracingScope<TTracingContext> tracingScope);
    }
}
