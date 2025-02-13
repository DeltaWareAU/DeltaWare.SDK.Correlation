using NServiceBus.Pipeline;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Scope;

namespace TraceLink.NServiceBus.Scope
{
    /// <summary>
    /// Represents a tracing scope for NServiceBus, providing a mechanism to initialize tracing from an incoming message context.
    /// </summary>
    /// <typeparam name="TTracingContext">
    /// The type of tracing context associated with this scope. Must be a <see langword="struct"/> implementing <see cref="ITracingContext"/>.
    /// </typeparam>
    public interface INServiceBusTracingScope<out TTracingContext> : ITracingScope<TTracingContext> where TTracingContext : struct, ITracingContext
    {
        /// <summary>
        /// Attempts to initialize the tracing scope from the given NServiceBus incoming message context.
        /// </summary>
        /// <param name="context">The incoming message context.</param>
        /// <returns>
        /// <see langword="true"/> if the tracing scope was successfully initialized; otherwise, <see langword="false"/>.
        /// </returns>
        bool TryInitializeScope(IIncomingPhysicalMessageContext context);
    }
}
