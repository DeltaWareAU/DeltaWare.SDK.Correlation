using DeltaWare.SDK.Correlation.AspNetCore.Middleware;
using DeltaWare.SDK.Correlation.AspNetCore.Options.Builder;
using Microsoft.AspNetCore.Builder;
using System;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class TraceServiceCollection
    {
        public static IServiceCollection AddTracing(this IServiceCollection services, Action<IOptionsBuilder>? optionsBuilder = null)
        {
            services.AddHttpContextAccessor();

            TraceOptionsBuilder builder = new TraceOptionsBuilder(services);

            optionsBuilder?.Invoke(builder);

            builder.Build();

            return services;
        }

        public static IApplicationBuilder UseTracing(this IApplicationBuilder app)
        {
            app.UseMiddleware<TraceMiddleware>();

            return app;
        }
    }
}
