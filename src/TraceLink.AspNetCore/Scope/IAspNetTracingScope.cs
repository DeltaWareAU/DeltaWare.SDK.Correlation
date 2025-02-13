using Microsoft.AspNetCore.Http;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Scope;

namespace TraceLink.AspNetCore.Scope
{
    /// <summary>
    /// Represents a tracing scope specific to ASP.NET, providing a mechanism to initialize tracing from an HTTP request.
    /// </summary>
    /// <typeparam name="TTracingContext">
    /// The type of tracing context associated with this scope. Must be a <see langword="struct"/> implementing <see cref="ITracingContext"/>.
    /// </typeparam>
    public interface IAspNetTracingScope<out TTracingContext> : ITracingScope<TTracingContext> where TTracingContext : struct, ITracingContext
    {
        /// <summary>
        /// Attempts to initialize the tracing scope from the given HTTP context.
        /// </summary>
        /// <param name="httpContext">The HTTP context containing request details.</param>
        /// <returns>
        /// <see langword="true"/> if the tracing scope was successfully initialized; otherwise, <see langword="false"/>.
        /// </returns>
        bool TryInitializeScope(HttpContext httpContext);
    }
}
