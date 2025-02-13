using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NServiceBus;
using TraceLink.Abstractions.Configuration;
using TraceLink.Abstractions.Context;
using TraceLink.NServiceBus.Behaviors;
using TraceLink.NServiceBus.Scope;

namespace TraceLink.NServiceBus
{
    public static class TraceLinkEndpointConfiguration
    {
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
