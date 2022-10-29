namespace DeltaWare.SDK.Correlation.Context
{
    /// <summary>
    /// The Current Correlation Context.
    /// </summary>
    public sealed class CorrelationContext
    {
        /// <summary>
        /// The Correlation Id.
        /// </summary>
        public string CorrelationId { get; }

        public CorrelationContext(string correlationId)
        {
            CorrelationId = correlationId;
        }
    }
}
