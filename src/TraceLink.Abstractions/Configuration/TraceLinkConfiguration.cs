using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Options;
using TraceLink.Abstractions.Outgoing;
using TraceLink.Abstractions.Scope;

namespace TraceLink.Abstractions.Configuration
{
    public class TraceLinkConfiguration<TTracingContext> : ITraceLinkConfiguration<TTracingContext> where TTracingContext : struct, ITracingContext
    {
        public string Key { get; set; }
        public bool AttachToResponse { get; set; }
        public bool IsRequired { get; set; }
        public bool AttachToLoggingScope { get; set; }
        public string LoggingScopeKey { get; set; }
        public IServiceCollection Services { get; }

        public TraceLinkConfiguration(IServiceCollection services)
        {
            Services = services;
        }

        public virtual void ConfigureTraceLink()
        {
            Services.AddScoped(_ => BuildTracingOptions());
            Services.TryAddScoped<TracingScopeContext<TTracingContext>>();
            Services.TryAddScoped<ITracingScopeAccessor<TTracingContext>>(p => p.GetRequiredService<TracingScopeContext<TTracingContext>>());
            Services.TryAddScoped<ITracingScopeSetter<TTracingContext>>(p => p.GetRequiredService<TracingScopeContext<TTracingContext>>());

            Services.TryAddScoped<IOutgoingTracingIdProvider<TTracingContext>, OutgoingTracingIdProvider<TTracingContext>>();
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
