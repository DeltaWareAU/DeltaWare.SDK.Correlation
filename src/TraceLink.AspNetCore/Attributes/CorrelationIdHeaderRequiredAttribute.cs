using System;

namespace TraceLink.AspNetCore.Attributes
{
    /// <summary>
    /// Ensures a CorrelationId is provided in the Request Headers, otherwise a 400 is returned to the caller.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class CorrelationIdHeaderRequiredAttribute : Attribute
    {
    }
}
