namespace DeltaWare.SDK.Correlation.Options
{
    public interface ICorrelationOptions
    {
        /// <summary>
        /// The Header to be used for the Correlation ID
        /// </summary>
        string Header { get; }

        bool AttachToResponse { get; }
        bool IsRequired { get; set; }
    }
}
