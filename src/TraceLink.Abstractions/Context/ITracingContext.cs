using System;

namespace TraceLink.Abstractions.Context
{
    /// <summary>
    /// Represents a tracing context that provides a unique identifier for tracing requests.
    /// </summary>
    public interface ITracingContext
    {
        /// <summary>
        /// Gets the unique identifier associated with the tracing context.
        /// </summary>
        /// <remarks>
        /// This identifier is used to track a request as it moves through the system.
        /// </remarks>
        Guid Id { get; }
    }
}
