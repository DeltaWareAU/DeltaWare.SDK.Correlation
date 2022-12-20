namespace TraceLink.Abstractions.Context.Scope
{
    public interface IContextScopeSetter<in TContext>
    {
        void SetScope(IContextScope<TContext> contextScope);
    }
}
