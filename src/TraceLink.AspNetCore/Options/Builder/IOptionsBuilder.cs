using Microsoft.Extensions.DependencyInjection;

namespace TraceLink.AspNetCore.Options.Builder
{
    public interface IOptionsBuilder
    {
        string Key { get; set; }

        bool AttachToResponse { get; set; }
        bool IsRequired { get; set; }
        bool AttachToLoggingScope { get; set; }
        string LoggingScopeKey { get; set; }

        IServiceCollection Services { get; }
    }
}
