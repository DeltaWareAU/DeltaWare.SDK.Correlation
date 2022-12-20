namespace TraceLink.Abstractions.Context.Scope
{
    /// <summary>
    /// The context scope used to access the Key.
    /// </summary>
    public interface IContextScope<out TContext> : IContextScope
    {
        /// <summary>
        /// This Scopes Context.
        /// </summary>
        TContext Context { get; }
    }
}
