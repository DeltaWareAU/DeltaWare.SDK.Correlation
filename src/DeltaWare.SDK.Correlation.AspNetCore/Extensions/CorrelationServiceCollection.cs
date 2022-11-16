using DeltaWare.SDK.Correlation.AspNetCore.Middleware;
using DeltaWare.SDK.Correlation.AspNetCore.Options.Builder;
using DeltaWare.SDK.Correlation.Context;
using Microsoft.AspNetCore.Builder;
using System;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class CorrelationServiceCollection
    {
        /// <summary>
        /// Registers the necessary dependencies to the <see cref="IServiceCollection"/> to enable Correlation.
        /// </summary>
        /// <param name="optionsBuilder">Configures Correlation.</param>
        public static IServiceCollection AddCorrelation(this IServiceCollection services, Action<IOptionsBuilder>? optionsBuilder = null)
        {
            CorrelationOptionsBuilder builder = new CorrelationOptionsBuilder(services);

            optionsBuilder?.Invoke(builder);

            builder.Build();

            return services;
        }

        /// <summary>
        /// Adds the Correlation Middleware to AspNet.
        /// </summary>
        public static IApplicationBuilder UseCorrelation(this IApplicationBuilder app)
        {
            app.UseMiddleware<ContextMiddleware<CorrelationContext>>();

            return app;
        }
    }
}
