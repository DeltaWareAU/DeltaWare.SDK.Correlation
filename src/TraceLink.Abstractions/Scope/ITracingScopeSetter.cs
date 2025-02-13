using TraceLink.Abstractions.Context;

namespace TraceLink.Abstractions.Scope
{
    /// <summary>
    /// Provides a mechanism to set the current tracing scope.
    /// </summary>
    /// <typeparam name="TTracingContext">
    /// The type of tracing context associated with the scope. Must be a <see langword="struct"/> implementing <see cref="ITracingContext"/>.
    /// </typeparam>
    public interface ITracingScopeSetter<in TTracingContext> where TTracingContext : struct, ITracingContext
    {
        /// <summary>
        /// Sets the tracing scope to the specified instance.
        /// </summary>
        /// <typeparam name="TTracingScope">
        /// The type of tracing scope being set. Must implement <see cref="ITracingScope{TTracingContext}"/>.
        /// </typeparam>
        /// <param name="tracingScope">The tracing scope instance to set.</param>
        void SetTracingScope<TTracingScope>(TTracingScope tracingScope) where TTracingScope : ITracingScope<TTracingContext>;
    }
}
