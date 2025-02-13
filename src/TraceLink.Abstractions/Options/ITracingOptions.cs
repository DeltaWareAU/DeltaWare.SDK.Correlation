using TraceLink.Abstractions.Context;

namespace TraceLink.Abstractions.Options
{
    /// <summary>
    /// Defines options for configuring tracing behavior.
    /// </summary>
    /// <typeparam name="TTracingContext">
    /// The type of tracing context associated with the options. Must be a <see langword="struct"/> implementing <see cref="ITracingContext"/>.
    /// </typeparam>
    public interface ITracingOptions<TTracingContext> where TTracingContext : struct, ITracingContext
    {
        /// <summary>
        /// Gets the key used to retrieve the tracing identifier.
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Gets the key used to associate the tracing identifier with a logging scope.
        /// </summary>
        /// <remarks>
        /// This key will only be used if <see cref="AttachToLoggingScope"/> is enabled.
        /// </remarks>
        string LoggingScopeKey { get; }

        /// <summary>
        /// Gets a value indicating whether the tracing identifier should be attached to the response.
        /// </summary>
        bool AttachToResponse { get; }

        /// <summary>
        /// Gets a value indicating whether the tracing identifier is required on incoming requests.
        /// </summary>
        bool IsRequired { get; }

        /// <summary>
        /// Gets a value indicating whether the tracing identifier should be attached to the logging scope.
        /// </summary>
        bool AttachToLoggingScope { get; }
    }
}
