using Microsoft.Extensions.DependencyInjection;
using TraceLink.Abstractions.Context;
using TraceLink.NServiceBus.Scope;

// ReSharper disable once CheckNamespace
namespace TraceLink.Abstractions.Configuration
{
    /// <summary>
    /// Provides extension methods for configuring NServiceBus tracing in TraceLink.
    /// </summary>
    public static class NServiceBusTraceLinkConfiguration
    {
        /// <summary>
        /// Registers an NServiceBus-specific tracing scope in the service collection.
        /// </summary>
        /// <typeparam name="TTracingContext">
        /// The type of tracing context associated with the scope. Must be a <see langword="struct"/> implementing <see cref="ITracingContext"/>.
        /// </typeparam>
        /// <typeparam name="TTracingScope">
        /// The type of the tracing scope to register. Must implement <see cref="INServiceBusTracingScope{TTracingContext}"/>.
        /// </typeparam>
        /// <param name="configuration">The TraceLink configuration instance.</param>
        public static void UseNServiceBusTracingScope<TTracingContext, TTracingScope>(this ITraceLinkConfiguration<TTracingContext> configuration) where TTracingContext : struct, ITracingContext where TTracingScope : class, INServiceBusTracingScope<TTracingContext>
            => configuration.Services.AddScoped<INServiceBusTracingScope<TTracingContext>, TTracingScope>();
    }
}
