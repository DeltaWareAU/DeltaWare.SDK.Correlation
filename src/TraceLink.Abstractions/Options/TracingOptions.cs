using TraceLink.Abstractions.Context;

namespace TraceLink.Abstractions.Options
{
    internal sealed class TracingOptions<TTracingContext> : ITracingOptions<TTracingContext> where TTracingContext : struct, ITracingContext
    {
        public required string Key { get; set; }
        public required string LoggingScopeKey { get; set; }
        public required bool AttachToResponse { get; set; }
        public required bool IsRequired { get; set; }
        public required bool AttachToLoggingScope { get; set; }
    }
}
