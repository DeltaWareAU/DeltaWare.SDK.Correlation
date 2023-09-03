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
    internal sealed class CorrelationOptionsBuilder : CorrelationOptions, ICorrelationOptionsBuilder
    {
        public IServiceCollection Services { get; }

        public CorrelationOptionsBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public void Build()
        {
            Services.AddHttpContextAccessor();

            Services.TryAddScoped<IAspNetTracingScope<CorrelationContext>, AspNetCorrelationScope>();

            Services.TryAddSingleton<AsyncLocalTracingScope<CorrelationContext>>();
            Services.TryAddSingleton<ITracingScopeSetter<CorrelationContext>>(p => p.GetRequiredService<AsyncLocalTracingScope<CorrelationContext>>());
            Services.TryAddSingleton<ITracingContextAccessor<CorrelationContext>>(p => p.GetRequiredService<AsyncLocalTracingScope<CorrelationContext>>());

            Services.TryAddSingleton<IIdForwarder<CorrelationContext>, DefaultCorrelationIdForwarder>();
            Services.TryAddSingleton<IIdProvider<CorrelationContext>, IdProviderWrapper<CorrelationContext, GuidIdProvider>>();
            Services.TryAddSingleton<ITracingOptions<CorrelationContext>>(this);

            Services.TryAddSingleton<IdForwardingHandler<CorrelationContext>>();
        }
    }
}
