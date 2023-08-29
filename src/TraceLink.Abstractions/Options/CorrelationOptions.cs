using TraceLink.Abstractions.Context;

namespace TraceLink.Abstractions.Options
{
    public class CorrelationOptions : ITracingOptions<CorrelationContext>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <remarks><b>Default value:</b> x-correlation-id</remarks>
        public string Key { get; set; } = "x-correlation-id";

        public bool AttachToResponse { get; set; } = false;
        public bool IsRequired { get; set; } = false;
        public bool AttachToLoggingScope { get; set; }
        public string LoggingScopeKey { get; set; } = "correlation-id";
    }
}
