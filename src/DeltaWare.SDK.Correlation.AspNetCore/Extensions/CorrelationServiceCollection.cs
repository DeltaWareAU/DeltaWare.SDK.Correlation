using DeltaWare.SDK.Correlation.AspNetCore.Options.Builder;
using Microsoft.AspNetCore.Builder;
using System;
using Prospa.SDK.Correlation.AspNetCore.Options;
using DeltaWare.SDK.Correlation.AspNetCore.Middleware;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class CorrelationServiceCollection
    {
        public static IServiceCollection AddCorrelation(this IServiceCollection services, Action<IOptionsBuilder>? optionsBuilder = null)
        {
            CorrelationOptionsBuilder builder = new CorrelationOptionsBuilder(services);

            optionsBuilder?.Invoke(builder);

            builder.Build();

            return services;
        }

        public static IApplicationBuilder UseCorrelation(this IApplicationBuilder app)
        {
            app.UseMiddleware<CorrelationMiddleware>();

            return app;
        }
    }
}
