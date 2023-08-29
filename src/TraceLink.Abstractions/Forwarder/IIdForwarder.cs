namespace TraceLink.Abstractions.Forwarder
{
    /// <summary>
    /// Gets the Id to be used when forwarding.
    /// </summary>
    public interface IIdForwarder
    {
        /// <summary>
        /// The Id used when for forwarding.
        /// </summary>
        /// <returns>An Id.</returns>
        string GetForwardingId();
    }
}
