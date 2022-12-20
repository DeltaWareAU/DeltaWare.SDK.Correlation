
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
    internal sealed class TraceOptionsBuilder : TraceOptions, ITraceOptionsBuilder
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

            Services.TryAddSingleton<IdForwardingHandler<TraceContext>>();
        }
    }
}
