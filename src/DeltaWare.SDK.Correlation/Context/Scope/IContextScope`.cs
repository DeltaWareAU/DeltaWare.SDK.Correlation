namespace DeltaWare.SDK.Correlation.Context.Scope
{
    /// <summary>
    /// The context scope used to access the Key.
    /// </summary>
    public interface IContextScope<out TContext> : IContextScope
    {
        TContext Context { get; }
    }
}
