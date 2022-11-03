namespace DeltaWare.SDK.Correlation.Providers
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
