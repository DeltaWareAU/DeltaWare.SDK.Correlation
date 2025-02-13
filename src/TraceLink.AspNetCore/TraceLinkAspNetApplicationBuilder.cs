using Microsoft.AspNetCore.Builder;
using TraceLink.Abstractions.Context;
using TraceLink.AspNetCore.Middleware;

namespace TraceLink.AspNetCore
{
    /// <summary>
    /// Provides extension methods for configuring TraceLink in an ASP.NET application.
    /// </summary>
    public static class TraceLinkAspNetApplicationBuilder
    {
        /// <summary>
        /// Adds the TraceLink middleware to the application's request pipeline.
        /// </summary>
        /// <typeparam name="TTracingContext">
        /// The type of tracing context used in the middleware. Must be a <see langword="struct"/> implementing <see cref="ITracingContext"/>.
        /// </typeparam>
        /// <param name="app">The application builder instance.</param>
        /// <returns>The modified <see cref="IApplicationBuilder"/> instance.</returns>
        public static IApplicationBuilder UseTraceLink<TTracingContext>(this IApplicationBuilder app) where TTracingContext : struct, ITracingContext
            => app.UseMiddleware<AspNetTracingScopeInitializationMiddleware<TTracingContext>>();
    }
}
