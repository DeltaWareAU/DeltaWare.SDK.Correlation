namespace TraceLink.AspNetCore.Enum
{
    /// <summary>
    /// Defines the validation requirements for tracing headers in incoming requests.
    /// </summary>
    public enum HeaderValidationRequirements
    {
        /// <summary>
        /// Uses the default validation behavior.
        /// </summary>
        Default,

        /// <summary>
        /// Requires the tracing header to be present in the request.
        /// </summary>
        Required,

        /// <summary>
        /// Allows the tracing header to be optional in the request.
        /// </summary>
        Optional
    }
}
