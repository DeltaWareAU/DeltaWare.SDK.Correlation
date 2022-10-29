using DeltaWare.SDK.Correlation.AspNetCore.Context.Scopes;
using DeltaWare.SDK.Correlation.AspNetCore.Filters;
using DeltaWare.SDK.Correlation.AspNetCore.Handler;
using DeltaWare.SDK.Correlation.Context.Accessors;
using DeltaWare.SDK.Correlation.Options;
using DeltaWare.SDK.Correlation.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;

namespace DeltaWare.SDK.Correlation.AspNetCore.Options.Builder
{
    public sealed class CorrelationOptionsBuilder : CorrelationOptions
    {
        public IServiceCollection Services { get; }

        internal CorrelationOptionsBuilder(IServiceCollection services)
        {
            Services = services;
        }

        internal void InternalBuild()
        {
            Services.TryAddScoped<AspNetCorrelationContextScope>();

            Services.TryAddSingleton<CorrelationContextAccessor>();
            Services.TryAddSingleton<ICorrelationContextAccessor>(p => p.GetRequiredService<CorrelationContextAccessor>());

            Services.TryAddSingleton<ICorrelationIdProvider, GuidCorrelationIdProvider>();
            Services.TryAddSingleton<ICorrelationOptions>(this);

            TryAddHandler(Services);
            TryAddHandler(Services);
        }

        private static void TryAddFilter(IServiceCollection services)
        {
            if (services.Any(s => s.ServiceType == typeof(CorrelationIdFilter)))
            {
                return;
            }

            services.AddScoped<CorrelationIdFilter>();
            services.Configure<MvcOptions>(o =>
            {
                o.Filters.AddService<CorrelationIdFilter>();
            });
        }

        private static void TryAddHandler(IServiceCollection services)
        {
            if (services.Any(x => x.ServiceType == typeof(CorrelationIdHandler)))
            {
                return;
            }

            services.AddSingleton<CorrelationIdHandler>();
            services.Configure<HttpMessageHandlerBuilder>(c =>
            {
                c.AdditionalHandlers.Add(c.Services.GetRequiredService<CorrelationIdHandler>());
            });
        }
    }
}
