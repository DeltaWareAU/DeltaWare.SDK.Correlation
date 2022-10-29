namespace DeltaWare.SDK.Correlation.Context
{
    /// <summary>
    /// The Current Trace Context.
    /// </summary>
    public sealed class TraceContext
    {
        /// <summary>
        /// The TraceId Id.
        /// </summary>
        public string? TraceId { get; }

        /// <summary>
        /// Indicates if the Context has a Trace Id Associated with it.
        /// </summary>
        public bool HasId { get; }

        public TraceContext()
        {
            TraceId = null;
            HasId = false;
        }

        public TraceContext(string traceId)
        {
            TraceId = traceId;
            HasId = true;
        }
    }
}
