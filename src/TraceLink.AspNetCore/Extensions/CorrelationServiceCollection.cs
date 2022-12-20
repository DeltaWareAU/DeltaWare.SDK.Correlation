
/* Unmerged change from project 'TraceLink.AspNetCore (netstandard2.1)'
Before:
using TraceLink.AspNetCore.Middleware;
using TraceLink.AspNetCore.Options.Builder;
using Microsoft.AspNetCore.Builder;
After:
using Microsoft.AspNetCore.Builder;
using TraceLink.AspNetCore.Middleware;
using TraceLink.AspNetCore.Options.Builder;
*/

/* Unmerged change from project 'TraceLink.AspNetCore (net5.0)'
Before:
using TraceLink.AspNetCore.Middleware;
using TraceLink.AspNetCore.Options.Builder;
using Microsoft.AspNetCore.Builder;
After:
using Microsoft.AspNetCore.Builder;
using TraceLink.AspNetCore.Middleware;
using TraceLink.AspNetCore.Options.Builder;
*/

/* Unmerged change from project 'TraceLink.AspNetCore (net6.0)'
Before:
using TraceLink.AspNetCore.Middleware;
using TraceLink.AspNetCore.Options.Builder;
using Microsoft.AspNetCore.Builder;
After:
using Microsoft.AspNetCore.Builder;
using TraceLink.AspNetCore.Middleware;
using TraceLink.AspNetCore.Options.Builder;
*/
using System;
using TraceLink.AspNetCore.Options.Builder;

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
        /// Registers the necessary dependencies to the <see cref="IServiceCollection"/> to enable Tracing.
        /// </summary>
        /// <param name="optionsBuilder">Configures Tracing.</param>
        public static IServiceCollection AddTracing(this IServiceCollection services, Action<IOptionsBuilder>? optionsBuilder = null)
        {
            services.AddHttpContextAccessor();

            TraceOptionsBuilder builder = new TraceOptionsBuilder(services);

            optionsBuilder?.Invoke(builder);

            builder.Build();

            return services;
        }
    }
}
