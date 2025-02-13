using Microsoft.Extensions.DependencyInjection;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Context.Factory;
using TraceLink.Abstractions.Outgoing;

namespace TraceLink.Abstractions.Configuration
{
    public static class TraceLinkConfigurationExtensions
    {
        public static void UseOutgoingIdProvider<TTracingContext, TProvider>(this ITraceLinkConfiguration<TTracingContext> configuration) where TTracingContext : struct, ITracingContext where TProvider : class, IOutgoingTracingIdProvider<TTracingContext>
            => configuration.Services.AddScoped<IOutgoingTracingIdProvider<TTracingContext>, TProvider>();

        public static void UseTracingContextFactory<TTracingContext, TProvider>(this ITraceLinkConfiguration<TTracingContext> configuration) where TTracingContext : struct, ITracingContext where TProvider : class, ITracingContextFactory<TTracingContext>
            => configuration.Services.AddScoped<ITracingContextFactory<TTracingContext>, TProvider>();
    }
}
