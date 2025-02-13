using Microsoft.Extensions.DependencyInjection;
using TraceLink.Abstractions.Context;

namespace TraceLink.Abstractions.Configuration
{
    /// <summary>
    /// Defines the configuration settings for TraceLink.
    /// </summary>
    /// <typeparam name="TTracingContext">
    /// The type of tracing context associated with the configuration. Must be a <see langword="struct"/> implementing <see cref="ITracingContext"/>.
    /// </typeparam>
    public interface ITraceLinkConfiguration<TTracingContext> where TTracingContext : struct, ITracingContext
    {
        /// <summary>
        /// Gets or sets the key used to retrieve the tracing identifier.
        /// </summary>
        string Key { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the tracing identifier should be attached to the response.
        /// </summary>
        bool AttachToResponse { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the tracing identifier is required on incoming requests.
        /// </summary>
        bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the tracing identifier should be attached to the logging scope.
        /// </summary>
        bool AttachToLoggingScope { get; set; }

        /// <summary>
        /// Gets or sets the key used to associate the tracing identifier with a logging scope.
        /// </summary>
        /// <remarks>
        /// This key will only be used if <see cref="AttachToLoggingScope"/> is enabled.
        /// </remarks>
        string LoggingScopeKey { get; set; }

        /// <summary>
        /// Gets the service collection used for dependency injection.
        /// </summary>
        IServiceCollection Services { get; }
    }
}
