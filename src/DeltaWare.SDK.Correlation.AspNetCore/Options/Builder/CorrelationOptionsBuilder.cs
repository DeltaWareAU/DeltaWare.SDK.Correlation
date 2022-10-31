using DeltaWare.SDK.Correlation.AspNetCore.Context.Scopes;
using DeltaWare.SDK.Correlation.AspNetCore.Handler;
using DeltaWare.SDK.Correlation.Context.Accessors;
using DeltaWare.SDK.Correlation.Options;
using DeltaWare.SDK.Correlation.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using System.Linq;

namespace DeltaWare.SDK.Correlation.AspNetCore.Options.Builder
{
    internal class CorrelationOptionsBuilder : CorrelationOptions, ICorrelationOptionsBuilder
    {
        public IServiceCollection Services { get; }

        public CorrelationOptionsBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public void Build()
        {
            Services.AddHttpContextAccessor();

            Services.TryAddScoped<AspNetCorrelationContextScope>();

            Services.TryAddSingleton<CorrelationContextAccessor>();
            Services.TryAddSingleton<ICorrelationContextAccessor>(p => p.GetRequiredService<CorrelationContextAccessor>());

            Services.TryAddSingleton<ICorrelationIdProvider, GuidCorrelationIdProvider>();
            Services.TryAddSingleton<ICorrelationOptions>(this);

            TryAddHandler(Services);
        }

        private static void TryAddHandler(IServiceCollection services)
        {
            if (services.Any(x => x.ServiceType == typeof(CorrelationIdForwardingHandler)))
            {
                return;
            }

            services.AddSingleton<CorrelationIdForwardingHandler>();
            services.Configure<HttpMessageHandlerBuilder>(c =>
            {
                c.AdditionalHandlers.Add(c.Services.GetRequiredService<CorrelationIdForwardingHandler>());
            });
        }
    }
}
