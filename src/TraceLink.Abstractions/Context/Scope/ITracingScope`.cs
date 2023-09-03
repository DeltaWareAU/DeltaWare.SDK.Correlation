namespace TraceLink.Abstractions.Context.Scope
{
    /// <summary>
    /// The Tracing Context Scope used to access the Tracing Context.
    /// </summary>
    public interface ITracingScope<out TTracingContext> : ITracingScope where TTracingContext : ITracingContext
    {
        /// <summary>
        /// This Scopes Tracing Context.
        /// </summary>
        TTracingContext Context { get; }

        bool ReceivedId { get; }
    }
}
