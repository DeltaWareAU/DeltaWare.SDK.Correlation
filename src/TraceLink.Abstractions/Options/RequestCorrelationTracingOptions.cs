using TraceLink.Abstractions.Context;

namespace TraceLink.Abstractions.Options
{
    public sealed class RequestCorrelationTracingOptions : ITracingOptions<RequestCorrelationContext>
    {
        public string Key { get; set; } = "request-correlation-id";
        public string LoggingScopeKey { get; set; } = "request-correlation-id";
        public bool AttachToResponse { get; set; } = false;
        public bool IsRequired { get; set; } = false;
        public bool AttachToLoggingScope { get; set; } = true;
    }
}
