using DeltaWare.SDK.Correlation.AspNetCore.Options.Builder;
using Prospa.SDK.Correlation.AspNetCore.Options.Builder;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class CorrelationServiceCollection
    {
        public static IServiceCollection UseCorrelationId(this IServiceCollection services, Action<CorrelationOptionsBuilder>? optionsBuilder = null)
        {


            CorrelationOptionsBuilder builder = new CorrelationOptionsBuilder(services);

            optionsBuilder?.Invoke(builder);

            builder.InternalBuild();

            return services;
        }
    }
}
