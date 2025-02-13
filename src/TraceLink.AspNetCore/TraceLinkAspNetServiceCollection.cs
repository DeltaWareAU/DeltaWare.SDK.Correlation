using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using TraceLink.Abstractions.Configuration;
using TraceLink.Abstractions.Context;
using TraceLink.AspNetCore.Scope;
using TraceLink.AspNetCore.Validation;

namespace TraceLink.AspNetCore
{
    public static class TraceLinkAspNetServiceCollection
    {
        public static IServiceCollection AddAspNetTracing<TTracingContext>(this IServiceCollection services, Action<ITracingConfiguration<TTracingContext>> configurationBuilder) where TTracingContext : struct, ITracingContext
        {
            TracingConfiguration<TTracingContext> configuration = new TracingConfiguration<TTracingContext>(services);

            configurationBuilder.Invoke(configuration);

            services.TryAddSingleton<IHeaderValidationManager, HeaderValidationManager>();

            return services;
        }

        public static IServiceCollection AddAspNetTracing(this IServiceCollection services, Action<ITracingConfiguration<CorrelationTracingContext>> configurationBuilder)
        {
            TracingConfiguration<CorrelationTracingContext> configuration = new TracingConfiguration<CorrelationTracingContext>(services);

            configurationBuilder.Invoke(configuration);

            configuration.UseAspNetTracingScope<CorrelationTracingContext, CorrelationTracingScope>();

            services.TryAddSingleton<IHeaderValidationManager, HeaderValidationManager>();

            return services;
        }

        public static IServiceCollection UseTracingHeaderValidationManager<THeaderValidationManager>(this IServiceCollection services) where THeaderValidationManager : class, IHeaderValidationManager
            => services.AddSingleton<IHeaderValidationManager, THeaderValidationManager>();
    }
}
