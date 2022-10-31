using Microsoft.Extensions.DependencyInjection;

namespace DeltaWare.SDK.Correlation.AspNetCore.Options.Builder
{
    public interface IOptionsBuilder
    {
        string Header { get; set; }

        bool AttachToResponse { get; set; }
        bool IsRequired { get; set; }
        bool AttachToLoggingScope { get; set; }
        string LoggingScopeKey { get; set; }

        IServiceCollection Services { get; }
    }
}
