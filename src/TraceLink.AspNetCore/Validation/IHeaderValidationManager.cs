using Microsoft.AspNetCore.Http;
using TraceLink.Abstractions.Context;
using TraceLink.AspNetCore.Enum;

namespace TraceLink.AspNetCore.Validation
{
    /// <summary>
    /// Provides a mechanism to determine the validation requirements for tracing headers in incoming requests.
    /// </summary>
    public interface IHeaderValidationManager
    {
        /// <summary>
        /// Gets the header validation requirements for the specified tracing context in the given HTTP request.
        /// </summary>
        /// <typeparam name="TTracingContext">
        /// The type of tracing context. Must be a <see langword="struct"/> implementing <see cref="ITracingContext"/>.
        /// </typeparam>
        /// <param name="httpContext">The HTTP context containing the request.</param>
        /// <returns>The applicable <see cref="HeaderValidationRequirements"/> for the request.</returns>
        HeaderValidationRequirements GetHeaderValidationRequirements<TTracingContext>(HttpContext httpContext) where TTracingContext : struct, ITracingContext;
    }
}
