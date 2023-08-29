using TraceLink.Abstractions.Context;

namespace TraceLink.Abstractions.Forwarder
{
    /// <summary>
    /// Gets the Id to be used for the specified Context when forwarding.
    /// </summary>
    public interface IIdForwarder<TContext> : IIdForwarder where TContext : ITracingContext
    {
    }
}
