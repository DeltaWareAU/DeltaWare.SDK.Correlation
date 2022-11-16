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
using DeltaWare.SDK.Correlation.Context.Scope;

namespace DeltaWare.SDK.Correlation.AspNetCore.Options.Builder
{
    internal class TraceOptionsBuilder : TraceOptions, ITraceOptionsBuilder
    {
        public IServiceCollection Services { get; }

        public TraceOptionsBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public void Build()
        {
            Services.TryAddScoped<IAspNetContextScope<TraceContext>, AspNetTraceContextScope>();

            Services.TryAddSingleton<AsyncLocalContextScope<TraceContext>>();
            Services.TryAddSingleton<IContextScopeSetter<TraceContext>>(p => p.GetRequiredService<AsyncLocalContextScope<TraceContext>>());
            Services.TryAddSingleton<IContextAccessor<TraceContext>>(p => p.GetRequiredService<AsyncLocalContextScope<TraceContext>>());

            Services.TryAddSingleton<IIdForwarder<TraceContext>, DefaultTraceIdForwarder>();
            Services.TryAddSingleton<IIdProvider<TraceContext>, IdProviderWrapper<TraceContext, GuidIdProvider>>();
            Services.TryAddSingleton<IOptions<TraceContext>>(this);

            TryAddHandler(Services);
        }

        private static void TryAddHandler(IServiceCollection services)
        {
            if (services.Any(x => x.ServiceType == typeof(IdForwardingHandler<TraceContext>)))
            {
                return;
            }

            services.AddSingleton<IdForwardingHandler<TraceContext>>();
            services.Configure<HttpMessageHandlerBuilder>(c =>
            {
                c.AdditionalHandlers.Add(c.Services.GetRequiredService<IdForwardingHandler<TraceContext>>());
            });
        }
    }
}
