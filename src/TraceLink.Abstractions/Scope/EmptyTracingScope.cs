using TraceLink.Abstractions.Context;

namespace TraceLink.Abstractions.Scope
{
    /// <summary>
    /// Represents an empty tracing scope with a default tracing context.
    /// </summary>
    /// <typeparam name="TTracingContext">
    /// The type of tracing context. Must be a <see langword="struct"/> implementing <see cref="ITracingContext"/>.
    /// </typeparam>
    public readonly struct EmptyTracingScope<TTracingContext> : ITracingScope<TTracingContext> where TTracingContext : struct, ITracingContext
    {
        /// <inheritdoc cref="ITracingScope{TTracingContext}.Context"/>
        public TTracingContext Context => default;
    }
}
