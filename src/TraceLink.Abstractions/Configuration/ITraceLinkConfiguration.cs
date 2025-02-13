using Microsoft.Extensions.DependencyInjection;
using TraceLink.Abstractions.Context;

namespace TraceLink.Abstractions.Configuration
{
    public interface ITraceLinkConfiguration<TTracingContext> where TTracingContext : struct, ITracingContext
    {
        string Key { get; set; }
        bool AttachToResponse { get; set; }
        bool IsRequired { get; set; }
        bool AttachToLoggingScope { get; set; }
        string LoggingScopeKey { get; set; }

        IServiceCollection Services { get; }
    }
}
