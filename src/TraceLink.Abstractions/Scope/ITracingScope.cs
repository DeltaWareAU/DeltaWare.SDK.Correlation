using TraceLink.Abstractions.Context;

namespace TraceLink.Abstractions.Scope
{
    /// <summary>
    /// Represents a tracing scope that holds a tracing context.
    /// </summary>
    /// <typeparam name="TTracingContext">
    /// The type of tracing context associated with this scope. Must be a <see langword="struct"/> implementing <see cref="ITracingContext"/>.
    /// </typeparam>
    public interface ITracingScope<out TTracingContext> where TTracingContext : struct, ITracingContext
    {
        /// <summary>
        /// Gets the tracing context associated with this scope.
        /// </summary>
        TTracingContext Context { get; }
    }
}
