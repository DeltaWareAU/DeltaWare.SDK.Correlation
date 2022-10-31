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
    public class TraceOptionsBuilder : TraceOptions
    {
        public IServiceCollection Services { get; }

        public TraceOptionsBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public virtual void Build()
        {
            Services.TryAddScoped<AspNetTraceContextScope>();
            Services.TryAddSingleton<TraceContextAccessor>();

            Services.TryAddSingleton<ITraceContextAccessor>(p => p.GetRequiredService<TraceContextAccessor>());

            Services.TryAddSingleton<ITraceIdProvider, GuidTraceIdProvider>();
            Services.TryAddSingleton<ITraceOptions>(this);

            TryAddHandler(Services);
        }

        private static void TryAddHandler(IServiceCollection services)
        {
            if (services.Any(x => x.ServiceType == typeof(TraceIdForwardingHandler)))
            {
                return;
            }

            services.AddSingleton<TraceIdForwardingHandler>();
            services.Configure<HttpMessageHandlerBuilder>(c =>
            {
                c.AdditionalHandlers.Add(c.Services.GetRequiredService<TraceIdForwardingHandler>());
            });
        }
    }
}
