using TraceLink.Abstractions.Context;

namespace TraceLink.Abstractions.Options
{
    public interface ITracingOptions<TTracingContext> where TTracingContext : struct, ITracingContext
    {
        /// <summary>
        /// The Key used to access the ID.
        /// </summary>
        string Key { get; }
        string LoggingScopeKey { get; }
        bool AttachToResponse { get; }
        bool IsRequired { get; }
        bool AttachToLoggingScope { get; }
    }
}
