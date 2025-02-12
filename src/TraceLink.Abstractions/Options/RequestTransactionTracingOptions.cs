using TraceLink.Abstractions.Context;

namespace TraceLink.Abstractions.Options
{
    public sealed class RequestTransactionTracingOptions : ITracingOptions<RequestTransactionContext>
    {
        public string Key { get; set; } = "request-transaction-id";
        public string LoggingScopeKey { get; set; } = "request-transaction-id";
        public bool AttachToResponse { get; set; } = false;
        public bool IsRequired { get; set; } = false;
        public bool AttachToLoggingScope { get; set; } = true;
    }
}
