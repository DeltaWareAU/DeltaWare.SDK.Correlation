namespace TraceLink.Abstractions.Providers
{
    /// <summary>
    /// Provides IDs.
    /// </summary>
    public interface IIdProvider
    {
        /// <summary>
        /// Generates an ID.
        /// </summary>
        string GenerateId();
    }
}
