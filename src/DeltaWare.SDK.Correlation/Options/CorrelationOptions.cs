namespace DeltaWare.SDK.Correlation.Options
{
    public class CorrelationOptions : ICorrelationOptions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <remarks><b>Default value:</b> x-correlation-id</remarks>
        public string Header { get; set; } = "x-correlation-id";

        public bool AttachToResponse { get; set; } = false;
        public bool IsRequired { get; set; } = false;
    }
}
