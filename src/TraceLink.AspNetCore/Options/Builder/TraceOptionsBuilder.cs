using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Context.Accessors;
using TraceLink.Abstractions.Context.Scope;
using TraceLink.Abstractions.Forwarder;
using TraceLink.Abstractions.Options;
using TraceLink.Abstractions.Providers;
using TraceLink.AspNetCore.Context.Scopes;
using TraceLink.AspNetCore.Handler;

namespace TraceLink.AspNetCore.Options.Builder
{
    internal sealed class TraceOptionsBuilder : TraceOptions, ITraceOptionsBuilder
    {
        public IServiceCollection Services { get; }

        public TraceOptionsBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public void Build()
        {
            Services.TryAddScoped<IAspNetTracingScope<TraceContext>, AspNetTraceScope>();

            Services.TryAddSingleton<AsyncLocalTracingScope<TraceContext>>();
            Services.TryAddSingleton<ITracingScopeSetter<TraceContext>>(p => p.GetRequiredService<AsyncLocalTracingScope<TraceContext>>());
            Services.TryAddSingleton<ITracingContextAccessor<TraceContext>>(p => p.GetRequiredService<AsyncLocalTracingScope<TraceContext>>());

            Services.TryAddSingleton<IIdForwarder<TraceContext>, DefaultTraceIdForwarder>();
            Services.TryAddSingleton<IIdProvider<TraceContext>, IdProviderWrapper<TraceContext, GuidIdProvider>>();
            Services.TryAddSingleton<ITracingOptions<TraceContext>>(this);

            Services.TryAddSingleton<IdForwardingHandler<TraceContext>>();
        }
    }
}
