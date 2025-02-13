using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using TraceLink.Abstractions.Configuration;
using TraceLink.Abstractions.Context;
using TraceLink.AspNetCore.Validation;

namespace TraceLink.AspNetCore
{
    public static class TraceLinkAspNetServiceCollection
    {
        public static IServiceCollection AddTraceLink<TTracingContext>(this IServiceCollection services, Action<ITraceLinkConfiguration<TTracingContext>> configurationBuilder) where TTracingContext : struct, ITracingContext
        {
            TraceLinkConfiguration<TTracingContext> configuration = new TraceLinkConfiguration<TTracingContext>(services);

            configurationBuilder.Invoke(configuration);

            services.TryAddSingleton<IHeaderValidationManager, HeaderValidationManager>();

            return services;
        }

        public static IServiceCollection UseTracingHeaderValidationManager<THeaderValidationManager>(this IServiceCollection services) where THeaderValidationManager : class, IHeaderValidationManager
            => services.AddSingleton<IHeaderValidationManager, THeaderValidationManager>();
    }
}
