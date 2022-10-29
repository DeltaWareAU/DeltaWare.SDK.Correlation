using DeltaWare.SDK.Correlation.AspNetCore.Options.Builder;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class TraceServiceCollection
    {
        public static IServiceCollection UseTraceId(this IServiceCollection services, Action<TraceOptionsBuilder>? optionsBuilder = null)
        {
            TraceOptionsBuilder builder = new TraceOptionsBuilder(services);

            optionsBuilder?.Invoke(builder);

            builder.InternalBuild();

            return services;
        }
    }
}
