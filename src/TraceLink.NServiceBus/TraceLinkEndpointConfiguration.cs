using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NServiceBus;
using System;
using TraceLink.Abstractions.Configuration;
using TraceLink.Abstractions.Context;
using TraceLink.NServiceBus.Behaviors;
using TraceLink.NServiceBus.Scope;

namespace TraceLink.NServiceBus
{
    /// <summary>
    /// Provides extension methods for configuring TraceLink in an NServiceBus endpoint.
    /// </summary>
    public static class TraceLinkEndpointConfiguration
    {
        /// <summary>
        /// Configures TraceLink for the given NServiceBus endpoint.
        /// </summary>
        /// <typeparam name="TTracingContext">
        /// The type of tracing context associated with TraceLink. Must be a <see langword="struct"/> implementing <see cref="ITracingContext"/>.
        /// </typeparam>
        /// <param name="endpointConfiguration">The NServiceBus endpoint configuration instance.</param>
        /// <param name="configurationBuilder">A delegate to configure TraceLink settings.</param>
        public static void UseTraceLink<TTracingContext>(this EndpointConfiguration endpointConfiguration, Action<ITraceLinkConfiguration<TTracingContext>> configurationBuilder) where TTracingContext : struct, ITracingContext
        {
            endpointConfiguration.RegisterComponents(services =>
            {
                TraceLinkConfiguration<TTracingContext> configuration = new TraceLinkConfiguration<TTracingContext>(services);

                configurationBuilder.Invoke(configuration);

                configuration.ConfigureTraceLink();

                services.TryAddScoped<INServiceBusTracingScope<TTracingContext>, NServiceBusTracingScope<TTracingContext>>();
                services.AddScoped<AttachOutgoingTracingIdBehavior<TTracingContext>>();
                services.AddScoped<RetrieveTracingIdBehavior<TTracingContext>>();
            });

            endpointConfiguration.Pipeline.Register<AttachOutgoingTracingIdBehavior<TTracingContext>.Register>();
            endpointConfiguration.Pipeline.Register<RetrieveTracingIdBehavior<TTracingContext>.Register>();
        }
    }
}
