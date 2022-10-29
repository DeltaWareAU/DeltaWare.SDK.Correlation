namespace DeltaWare.SDK.Correlation.Options
{
    public class TraceOptions : ITraceOptions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <remarks><b>Default value:</b> x-tracing-id</remarks>
        public string Header { get; set; } = "x-tracing-id";

        public bool AttachToResponse { get; set; } = false;
        public bool IsRequired { get; set; } = false;
    }
}
