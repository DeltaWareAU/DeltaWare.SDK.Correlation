using TraceLink.Abstractions.Context;
using TraceLink.AspNetCore.Middleware;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
    public static class CorrelationApplicationBuilder
    {
        /// <summary>
        /// Adds the Correlation Middleware to AspNet.
        /// </summary>
        public static IApplicationBuilder UseCorrelation(this IApplicationBuilder app)
        {
            app.UseMiddleware<ContextMiddleware<CorrelationContext>>();

            return app;
        }

        /// <summary>
        /// Adds the Tracing Middleware to AspNet.
        /// </summary>
        public static IApplicationBuilder UseTracing(this IApplicationBuilder app)
        {
            app.UseMiddleware<ContextMiddleware<TraceContext>>();

            return app;
        }
    }
}
