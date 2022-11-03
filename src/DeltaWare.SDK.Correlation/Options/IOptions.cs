namespace DeltaWare.SDK.Correlation.Options
{
    public interface IOptions
    {
        /// <summary>
        /// The Header to be used for the Correlation ID
        /// </summary>
        string Header { get; }

        bool AttachToResponse { get; }
        bool IsRequired { get; set; }
        bool AttachToLoggingScope { get; }

        string LoggingScopeKey { get; }
    }
}
