using DeltaWare.SDK.Correlation.AspNetCore.Middleware;
using DeltaWare.SDK.Correlation.AspNetCore.Options.Builder;

/* Unmerged change from project 'DeltaWare.SDK.Correlation.AspNetCore (net6.0)'
Before:
using System;
using Prospa.SDK.Correlation.AspNetCore.Options;
using DeltaWare.SDK.Correlation.AspNetCore.Middleware;
After:
using DeltaWare.SDK.Correlation.Context;
using Microsoft.AspNetCore.Builder;
using Prospa.SDK.Correlation.AspNetCore.Options;
*/
using DeltaWare.SDK.Correlation.Context;
using Microsoft.AspNetCore.Builder;
using System;

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
            app.UseMiddleware<ContextMiddleware<CorrelationContext>>();

            return app;
        }
    }
}
