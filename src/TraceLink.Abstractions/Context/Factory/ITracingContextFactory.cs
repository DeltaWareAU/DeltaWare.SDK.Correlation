using System;

namespace TraceLink.Abstractions.Context.Factory
{
    /// <summary>
    /// Defines a factory for creating instances of a tracing context.
    /// </summary>
    /// <typeparam name="TTracingContext">
    /// The type of tracing context to create. Must be a <see langword="struct"/> implementing <see cref="ITracingContext"/>.
    /// </typeparam>
    public interface ITracingContextFactory<out TTracingContext> where TTracingContext : struct, ITracingContext
    {
        /// <summary>
        /// Creates a new tracing context with the specified tracing identifier.
        /// </summary>
        /// <param name="tracingId">The unique identifier to associate with the tracing context.</param>
        /// <returns>A new instance of <typeparamref name="TTracingContext"/> with the provided identifier.</returns>
        TTracingContext CreateContext(Guid tracingId);
    }
}
