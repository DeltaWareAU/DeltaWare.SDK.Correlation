using System;
using TraceLink.Abstractions.Context;

namespace TraceLink.Abstractions.Outgoing
{
    /// <summary>
    /// Provides a mechanism for retrieving the outbound tracing identifier.
    /// </summary>
    /// <typeparam name="TTracingContext">
    /// The type of tracing context associated with the provider. Must be a <see langword="struct"/> implementing <see cref="ITracingContext"/>.
    /// </typeparam>
    public interface IOutgoingTracingIdProvider<TTracingContext> where TTracingContext : struct, ITracingContext
    {
        /// <summary>
        /// Gets the tracing identifier to be used for outgoing requests.
        /// </summary>
        /// <returns>The outbound tracing identifier.</returns>
        Guid GetOutboundTracingId();
    }
}
