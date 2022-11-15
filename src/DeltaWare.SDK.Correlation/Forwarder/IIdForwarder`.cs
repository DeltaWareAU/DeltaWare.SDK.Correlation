namespace DeltaWare.SDK.Correlation.Forwarder
{
    /// <summary>
    /// Gets the Id to be used for the specified Context when forwarding.
    /// </summary>
    public interface IIdForwarder<TContext> : IIdForwarder where TContext : class
    {
    }
}
