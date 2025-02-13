using Microsoft.Extensions.DependencyInjection;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Outbound;

namespace TraceLink.Abstractions.Configuration
{
    public static class TracingConfigurationExtensions
    {
        public static void UseOutboundIdProvider<TTracingContext, TProvider>(this ITracingConfiguration<TTracingContext> configuration) where TTracingContext : struct, ITracingContext where TProvider : class, IOutboundTracingIdProvider<TTracingContext>
            => configuration.Services.AddScoped<IOutboundTracingIdProvider<TTracingContext>, TProvider>();
    }
}
