using Microsoft.Extensions.DependencyInjection;
using TraceLink.Abstractions.Context;
using TraceLink.NServiceBus.Scope;

// ReSharper disable once CheckNamespace
namespace TraceLink.Abstractions.Configuration
{
    public static class NServiceBusTraceLinkConfiguration
    {
        public static void UseNServiceBusTracingScope<TTracingContext, TTracingScope>(this ITraceLinkConfiguration<TTracingContext> configuration) where TTracingContext : struct, ITracingContext where TTracingScope : class, INServiceBusTracingScope<TTracingContext>
            => configuration.Services.AddScoped<INServiceBusTracingScope<TTracingContext>, TTracingScope>();
    }
}
