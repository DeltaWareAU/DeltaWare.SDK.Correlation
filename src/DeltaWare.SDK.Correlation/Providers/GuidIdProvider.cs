using System;

namespace DeltaWare.SDK.Correlation.Providers
{
    /// <summary>
    /// Provides IDs in GUID format.
    /// </summary>
    public sealed class GuidIdProvider : IIdProvider
    {
        /// <summary>
        /// Generates a ID in GUID format.
        /// </summary>
        public string GenerateId() => Guid.NewGuid().ToString();
    }
}
