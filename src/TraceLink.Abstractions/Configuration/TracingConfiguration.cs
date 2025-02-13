using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Options;
using TraceLink.Abstractions.Outbound;
using TraceLink.Abstractions.Scope;

namespace TraceLink.Abstractions.Configuration
{
    public class TracingConfiguration<TTracingContext> : ITracingConfiguration<TTracingContext> where TTracingContext : struct, ITracingContext
    {
        public string Key { get; set; }
        public bool AttachToResponse { get; set; }
        public bool IsRequired { get; set; }
        public bool AttachToLoggingScope { get; set; }
        public string LoggingScopeKey { get; set; }
        public IServiceCollection Services { get; }

        public TracingConfiguration(IServiceCollection services)
        {
            Services = services;
        }

        public virtual void ConfigureTracing()
        {
            Services.AddScoped(_ => BuildTracingOptions());
            Services.AddScoped<TracingScopeContext<TTracingContext>>();
            Services.AddScoped<ITracingScopeAccessor<TTracingContext>>(p => p.GetRequiredService<TracingScopeContext<TTracingContext>>());
            Services.AddScoped<ITracingScopeSetter<TTracingContext>>(p => p.GetRequiredService<TracingScopeContext<TTracingContext>>());

            Services.TryAddScoped<IOutboundTracingIdProvider<TTracingContext>, OutboundTracingIdProvider<TTracingContext>>();
        }

        private ITracingOptions<TTracingContext> BuildTracingOptions()
            => new TracingOptions<TTracingContext>
            {
                AttachToLoggingScope = AttachToLoggingScope,
                AttachToResponse = AttachToResponse,
                IsRequired = IsRequired,
                Key = Key,
                LoggingScopeKey = LoggingScopeKey
            };
    }
}
