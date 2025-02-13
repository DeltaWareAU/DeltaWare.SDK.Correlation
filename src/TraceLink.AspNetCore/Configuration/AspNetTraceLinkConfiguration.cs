using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TraceLink.Abstractions.Context;
using TraceLink.AspNetCore.Scope;

// ReSharper disable once CheckNamespace
namespace TraceLink.Abstractions.Configuration
{
    public static class AspNetTraceLinkConfiguration
    {
        /// <summary>
        /// Registers an ASP.NET-specific tracing scope in the service collection.
        /// </summary>
        /// <typeparam name="TTracingContext">
        /// The type of tracing context associated with the scope. Must be a <see langword="struct"/> implementing <see cref="ITracingContext"/>.
        /// </typeparam>
        /// <typeparam name="TTracingScope">
        /// The type of the tracing scope to register. Must implement <see cref="IAspNetTracingScope{TTracingContext}"/>.
        /// </typeparam>
        /// <param name="tracingConfiguration">The TraceLink configuration instance.</param>
        public static void UseAspNetTracingScope<TTracingContext, TTracingScope>(this ITraceLinkConfiguration<TTracingContext> tracingConfiguration) where TTracingContext : struct, ITracingContext where TTracingScope : class, IAspNetTracingScope<TTracingContext>
            => tracingConfiguration.Services.AddScoped<IAspNetTracingScope<TTracingContext>, TTracingScope>();

        /// <summary>
        /// Configures tracing settings using an application configuration section.
        /// </summary>
        /// <typeparam name="TTracingContext">
        /// The type of tracing context associated with the configuration. Must be a <see langword="struct"/> implementing <see cref="ITracingContext"/>.
        /// </typeparam>
        /// <param name="tracingConfiguration">The TraceLink configuration instance.</param>
        /// <param name="configuration">The application configuration instance.</param>
        /// <param name="key">The key of the configuration section to load.</param>
        public static void UseConfiguration<TTracingContext>(this ITraceLinkConfiguration<TTracingContext> tracingConfiguration, IConfiguration configuration, string key) where TTracingContext : struct, ITracingContext
            => tracingConfiguration.UseConfiguration(configuration.GetSection(key));

        /// <summary>
        /// Configures tracing settings using a specific configuration section.
        /// </summary>
        /// <typeparam name="TTracingContext">
        /// The type of tracing context associated with the configuration. Must be a <see langword="struct"/> implementing <see cref="ITracingContext"/>.
        /// </typeparam>
        /// <param name="tracingConfiguration">The TraceLink configuration instance.</param>
        /// <param name="configuration">The configuration section containing tracing settings.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if required configuration values are missing.
        /// </exception>
        public static void UseConfiguration<TTracingContext>(this ITraceLinkConfiguration<TTracingContext> tracingConfiguration, IConfigurationSection configuration) where TTracingContext : struct, ITracingContext
        {
            tracingConfiguration.AttachToLoggingScope = configuration.GetBoolean(nameof(ITraceLinkConfiguration<TTracingContext>.AttachToLoggingScope), true);
            tracingConfiguration.AttachToResponse = configuration.GetBoolean(nameof(ITraceLinkConfiguration<TTracingContext>.AttachToResponse), true);
            tracingConfiguration.IsRequired = configuration.GetBoolean(nameof(ITraceLinkConfiguration<TTracingContext>.IsRequired));
            tracingConfiguration.Key = configuration[nameof(ITraceLinkConfiguration<TTracingContext>.Key)] ?? throw new ArgumentNullException();
            tracingConfiguration.LoggingScopeKey = configuration[nameof(ITraceLinkConfiguration<TTracingContext>.LoggingScopeKey)] ?? throw new ArgumentNullException();
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
