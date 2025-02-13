using TraceLink.Abstractions.Context;

namespace TraceLink.Abstractions.Scope
{
    /// <summary>
    /// Provides access to the current tracing scope.
    /// </summary>
    /// <typeparam name="TTracingContext">
    /// The type of tracing context associated with the scope. Must be a <see langword="struct"/> implementing <see cref="ITracingContext"/>.
    /// </typeparam>
    public interface ITracingScopeAccessor<out TTracingContext> where TTracingContext : struct, ITracingContext
    {
        /// <summary>
        /// Gets the current tracing scope.
        /// </summary>
        ITracingScope<TTracingContext> Scope { get; }
    }
}
