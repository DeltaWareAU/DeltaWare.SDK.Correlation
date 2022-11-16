namespace DeltaWare.SDK.Correlation.Context.Scope
{
    public interface IContextScopeSetter<in TContext>
    {
        void SetScope(IContextScope<TContext> contextScope);
    }
}
