namespace DeltaWare.SDK.Correlation.Providers
{
    /// <summary>
    /// Provides Correlation IDs in GUID format.
    /// </summary>
    public sealed class GuidCorrelationIdProvider : ICorrelationIdProvider
    {
        /// <summary>
        /// Generates a Correlation ID in GUID format.
        /// </summary>
        public string GenerateId() => Guid.NewGuid().ToString();
    }
}
