using TraceLink.Abstractions.Context;

namespace TraceLink.Abstractions.Options
{
    public class TraceOptions : ITracingOptions<TraceContext>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <remarks><b>Default value:</b> x-tracing-id</remarks>
        public string Key { get; set; } = "x-tracing-id";

        public bool AttachToResponse { get; set; } = false;
        public bool IsRequired { get; set; } = false;
        public bool AttachToLoggingScope { get; set; }
        public string LoggingScopeKey { get; set; } = "tracing-id";
    }
}
