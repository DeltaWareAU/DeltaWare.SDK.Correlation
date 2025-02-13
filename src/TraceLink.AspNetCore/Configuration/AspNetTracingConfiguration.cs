using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TraceLink.Abstractions.Context;
using TraceLink.AspNetCore.Scope;

// ReSharper disable once CheckNamespace
namespace TraceLink.Abstractions.Configuration
{
    public static class AspNetTracingConfiguration
    {
        public static void UseAspNetTracingScope<TTracingContext, TTracingScope>(this ITracingConfiguration<TTracingContext> tracingConfiguration) where TTracingContext : struct, ITracingContext where TTracingScope : class, IAspNetTracingScope<TTracingContext>
            => tracingConfiguration.Services.AddScoped<IAspNetTracingScope<TTracingContext>, TTracingScope>();

        public static void UseConfiguration<TTracingContext>(this ITracingConfiguration<TTracingContext> tracingConfiguration, IConfiguration configuration, string key) where TTracingContext : struct, ITracingContext
            => tracingConfiguration.UseConfiguration(configuration.GetSection(key));

        public static void UseConfiguration<TTracingContext>(this ITracingConfiguration<TTracingContext> tracingConfiguration, IConfigurationSection configuration) where TTracingContext : struct, ITracingContext
        {
            tracingConfiguration.AttachToLoggingScope = configuration.GetBoolean(nameof(ITracingConfiguration<TTracingContext>.AttachToLoggingScope), true);
            tracingConfiguration.AttachToResponse = configuration.GetBoolean(nameof(ITracingConfiguration<TTracingContext>.AttachToResponse), true);
            tracingConfiguration.IsRequired = configuration.GetBoolean(nameof(ITracingConfiguration<TTracingContext>.IsRequired));
            tracingConfiguration.Key = configuration[nameof(ITracingConfiguration<TTracingContext>.Key)] ?? throw new ArgumentNullException();
            tracingConfiguration.LoggingScopeKey = configuration[nameof(ITracingConfiguration<TTracingContext>.LoggingScopeKey)] ?? throw new ArgumentNullException();
        }

        private static bool GetBoolean(this IConfigurationSection configuration, string key, bool defaultValue = false)
        {
            var value = configuration[key];

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException($"Missing configuration for {key}");
            }

            return bool.TryParse(value, out var result) ? result : defaultValue;
        }
    }
}
