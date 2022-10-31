using DeltaWare.SDK.Correlation.AspNetCore;
using DeltaWare.SDK.Correlation.AspNetCore.Options.Builder;
using Microsoft.AspNetCore.Builder;
using System;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class TraceServiceCollection
    {
        public static IServiceCollection AddTrace(this IServiceCollection services, Action<TraceOptionsBuilder>? optionsBuilder = null)
        {
            services.AddHttpContextAccessor();

            TraceOptionsBuilder builder = new TraceOptionsBuilder(services);

            optionsBuilder?.Invoke(builder);

            builder.Build();

            return services;
        }

        public static IApplicationBuilder UseTrace(this IApplicationBuilder app)
        {
            app.UseMiddleware<TraceMiddleware>();

            return app;
        }
    }
}
