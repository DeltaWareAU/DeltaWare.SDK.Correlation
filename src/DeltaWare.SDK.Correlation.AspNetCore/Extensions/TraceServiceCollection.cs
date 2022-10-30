using DeltaWare.SDK.Correlation.AspNetCore;
using DeltaWare.SDK.Correlation.AspNetCore.Options.Builder;
using Microsoft.AspNetCore.Builder;
using System;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class TraceServiceCollection
    {
        public static IServiceCollection AdTrace(this IServiceCollection services, Action<TraceOptionsBuilder>? optionsBuilder = null)
        {
            TraceOptionsBuilder builder = new TraceOptionsBuilder(services);

            optionsBuilder?.Invoke(builder);

            builder.InternalBuild();

            return services;
        }

        public static IApplicationBuilder UseTrace(this IApplicationBuilder app)
        {
            app.UseMiddleware<TraceMiddleware>();

            return app;
        }
    }
}
