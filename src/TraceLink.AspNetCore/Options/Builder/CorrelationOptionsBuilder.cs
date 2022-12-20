
/* Unmerged change from project 'TraceLink.AspNetCore (netstandard2.1)'
Before:
using TraceLink.AspNetCore.Context.Scopes;
using TraceLink.AspNetCore.Handler;
After:
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
*/

/* Unmerged change from project 'TraceLink.AspNetCore (net5.0)'
Before:
using TraceLink.AspNetCore.Context.Scopes;
using TraceLink.AspNetCore.Handler;
After:
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
*/

/* Unmerged change from project 'TraceLink.AspNetCore (net6.0)'
Before:
using TraceLink.AspNetCore.Context.Scopes;
using TraceLink.AspNetCore.Handler;
After:
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
*/

/* Unmerged change from project 'TraceLink.AspNetCore (netstandard2.1)'
Before:
using Microsoft.Extensions.Http;
After:
using TraceLink.AspNetCore.Context.Http;
*/

/* Unmerged change from project 'TraceLink.AspNetCore (net5.0)'
Before:
using Microsoft.Extensions.Http;
After:
using TraceLink.AspNetCore.Context.Http;
*/

/* Unmerged change from project 'TraceLink.AspNetCore (net6.0)'
Before:
using Microsoft.Extensions.Http;
After:
using TraceLink.AspNetCore.Context.Http;
*/

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

            Services.TryAddScoped<IAspNetContextScope<CorrelationContext>, AspNetCorrelationContextScope>();

            Services.TryAddSingleton<AsyncLocalContextScope<CorrelationContext>>();
            Services.TryAddSingleton<IContextScopeSetter<CorrelationContext>>(p => p.GetRequiredService<AsyncLocalContextScope<CorrelationContext>>());
            Services.TryAddSingleton<IContextAccessor<CorrelationContext>>(p => p.GetRequiredService<AsyncLocalContextScope<CorrelationContext>>());

            Services.TryAddSingleton<IIdForwarder<CorrelationContext>, DefaultCorrelationIdForwarder>();
            Services.TryAddSingleton<IIdProvider<CorrelationContext>, IdProviderWrapper<CorrelationContext, GuidIdProvider>>();
            Services.TryAddSingleton<IOptions<CorrelationContext>>(this);

            Services.TryAddSingleton<IdForwardingHandler<CorrelationContext>>();
        }
    }
}
