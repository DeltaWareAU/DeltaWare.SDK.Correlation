using DeltaWare.SDK.Correlation.AspNetCore;
using DeltaWare.SDK.Correlation.AspNetCore.Options.Builder;
using Microsoft.AspNetCore.Builder;
using System;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class CorrelationServiceCollection
    {
        public static IServiceCollection AddCorrelation(this IServiceCollection services, Action<CorrelationOptionsBuilder>? optionsBuilder = null)
        {
            CorrelationOptionsBuilder builder = new CorrelationOptionsBuilder(services);

            optionsBuilder?.Invoke(builder);

            builder.InternalBuild();

            return services;
        }

        public static IApplicationBuilder UseCorrelation(this IApplicationBuilder app)
        {
            app.UseMiddleware<CorrelationMiddleware>();

            return app;
        }
    }
}
