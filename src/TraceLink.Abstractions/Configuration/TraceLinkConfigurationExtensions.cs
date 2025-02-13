using Microsoft.Extensions.DependencyInjection;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Context.Factory;
using TraceLink.Abstractions.Outgoing;

namespace TraceLink.Abstractions.Configuration
{
    /// <summary>
    /// Provides extension methods for configuring TraceLink services.
    /// </summary>
    public static class TraceLinkConfigurationExtensions
    {
        /// <summary>
        /// Registers an outgoing tracing ID provider in the service collection.
        /// </summary>
        /// <typeparam name="TTracingContext">
        /// The type of tracing context associated with the provider. Must be a <see langword="struct"/> implementing <see cref="ITracingContext"/>.
        /// </typeparam>
        /// <typeparam name="TProvider">
        /// The type of the outgoing tracing ID provider to register. Must implement <see cref="IOutgoingTracingIdProvider{TTracingContext}"/>.
        /// </typeparam>
        /// <param name="configuration">The TraceLink configuration instance.</param>
        public static void UseOutgoingIdProvider<TTracingContext, TProvider>(this ITraceLinkConfiguration<TTracingContext> configuration) where TTracingContext : struct, ITracingContext where TProvider : class, IOutgoingTracingIdProvider<TTracingContext>
            => configuration.Services.AddScoped<IOutgoingTracingIdProvider<TTracingContext>, TProvider>();

        /// <summary>
        /// Registers a tracing context factory in the service collection.
        /// </summary>
        /// <typeparam name="TTracingContext">
        /// The type of tracing context associated with the factory. Must be a <see langword="struct"/> implementing <see cref="ITracingContext"/>.
        /// </typeparam>
        /// <typeparam name="TProvider">
        /// The type of the tracing context factory to register. Must implement <see cref="ITracingContextFactory{TTracingContext}"/>.
        /// </typeparam>
        /// <param name="configuration">The TraceLink configuration instance.</param>
        public static void UseTracingContextFactory<TTracingContext, TProvider>(this ITraceLinkConfiguration<TTracingContext> configuration) where TTracingContext : struct, ITracingContext where TProvider : class, ITracingContextFactory<TTracingContext>
            => configuration.Services.AddScoped<ITracingContextFactory<TTracingContext>, TProvider>();
    }
}
