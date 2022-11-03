using DeltaWare.SDK.Correlation.Context;

namespace DeltaWare.SDK.Correlation.Options
{
    public class TraceOptions : IOptions<TraceContext>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <remarks><b>Default value:</b> x-tracing-id</remarks>
        public string Header { get; set; } = "x-tracing-id";

        public bool AttachToResponse { get; set; } = false;
        public bool IsRequired { get; set; } = false;
        public bool AttachToLoggingScope { get; set; }
        public string LoggingScopeKey { get; set; } = "tracing-id";
    }
}
