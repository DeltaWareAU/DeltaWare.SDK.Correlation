namespace DeltaWare.SDK.Correlation.Options
{
    public interface ITraceOptions
    {
        /// <summary>
        /// The Header to be used for the Trace ID
        /// </summary>
        string Header { get; }

        bool AttachToResponse { get; }
        bool IsRequired { get; set; }
    }
}
