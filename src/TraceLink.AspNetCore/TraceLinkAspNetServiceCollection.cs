using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using TraceLink.Abstractions.Configuration;
using TraceLink.Abstractions.Context;
using TraceLink.AspNetCore.Validation;

namespace TraceLink.AspNetCore
{
    /// <summary>
    /// Provides extension methods for configuring TraceLink services in an ASP.NET application.
    /// </summary>
    public static class TraceLinkAspNetServiceCollection
    {
        /// <summary>
        /// Adds TraceLink services to the application's dependency injection container.
        /// </summary>
        /// <typeparam name="TTracingContext">
        /// The type of tracing context associated with the configuration. Must be a <see langword="struct"/> implementing <see cref="ITracingContext"/>.
        /// </typeparam>
        /// <param name="services">The service collection to configure.</param>
        /// <param name="configurationBuilder">A delegate to configure TraceLink settings.</param>
        /// <returns>The modified <see cref="IServiceCollection"/> instance.</returns>
        public static IServiceCollection AddTraceLink<TTracingContext>(this IServiceCollection services, Action<ITraceLinkConfiguration<TTracingContext>> configurationBuilder) where TTracingContext : struct, ITracingContext
        {
            TraceLinkConfiguration<TTracingContext> configuration = new TraceLinkConfiguration<TTracingContext>(services);

            configurationBuilder.Invoke(configuration);

            services.TryAddSingleton<IHeaderValidationManager, HeaderValidationManager>();

            return services;
        }

        /// <summary>
        /// Registers a custom tracing header validation manager in the service collection.
        /// </summary>
        /// <typeparam name="THeaderValidationManager">
        /// The type of the header validation manager to register. Must implement <see cref="IHeaderValidationManager"/>.
        /// </typeparam>
        /// <param name="services">The service collection to configure.</param>
        /// <returns>The modified <see cref="IServiceCollection"/> instance.</returns>
        public static IServiceCollection UseTracingHeaderValidationManager<THeaderValidationManager>(this IServiceCollection services) where THeaderValidationManager : class, IHeaderValidationManager
            => services.AddSingleton<IHeaderValidationManager, THeaderValidationManager>();
    }
}
