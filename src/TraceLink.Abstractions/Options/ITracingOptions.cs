namespace TraceLink.Abstractions.Options
{
    public interface ITracingOptions
    {
        /// <summary>
        /// The Key used to access the ID.
        /// </summary>
        string Key { get; }

        bool AttachToResponse { get; }
        bool IsRequired { get; set; }
        bool AttachToLoggingScope { get; }

        string LoggingScopeKey { get; }
    }
}
