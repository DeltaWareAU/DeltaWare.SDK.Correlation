using DeltaWare.SDK.Correlation.AspNetCore.Context.Scopes;
using DeltaWare.SDK.Correlation.AspNetCore.Handler;
using DeltaWare.SDK.Correlation.Context;
using DeltaWare.SDK.Correlation.Context.Accessors;
using DeltaWare.SDK.Correlation.Forwarder;
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

            Services.TryAddScoped<IAspNetContextScope<CorrelationContext>, AspNetCorrelationContextScope>();

            Services.TryAddSingleton<IIdForwarder<CorrelationContext>, DefaultCorrelationIdForwarder>();
            Services.TryAddSingleton<ContextAccessor<CorrelationContext>>();
            Services.TryAddSingleton<IContextAccessor<CorrelationContext>>(p => p.GetRequiredService<ContextAccessor<CorrelationContext>>());

            Services.TryAddSingleton<IIdProvider<CorrelationContext>, IdProviderWrapper<CorrelationContext, GuidIdProvider>>();
            Services.TryAddSingleton<IOptions<CorrelationContext>>(this);

            TryAddHandler(Services);
        }

        private static void TryAddHandler(IServiceCollection services)
        {
            if (services.Any(x => x.ServiceType == typeof(IdForwardingHandler<CorrelationContext>)))
            {
                return;
            }

            services.AddSingleton<IdForwardingHandler<CorrelationContext>>();
            services.Configure<HttpMessageHandlerBuilder>(c =>
            {
                c.AdditionalHandlers.Add(c.Services.GetRequiredService<IdForwardingHandler<CorrelationContext>>());
            });
        }
    }
}
