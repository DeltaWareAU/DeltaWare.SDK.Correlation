using TraceLink.Abstractions.Context;

namespace TraceLink.Abstractions.Options
{
    public interface ITracingOptions<TContext> where TContext : ITracingContext
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
