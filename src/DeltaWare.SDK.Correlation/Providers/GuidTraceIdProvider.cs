namespace DeltaWare.SDK.Correlation.Providers
{
    /// <summary>
    /// Provides Trace IDs in GUID format.
    /// </summary>
    public sealed class GuidTraceIdProvider : ITraceIdProvider
    {
        /// <summary>
        /// Generates a Trace ID in GUID format.
        /// </summary>
        public string GenerateId() => Guid.NewGuid().ToString();
    }
}
