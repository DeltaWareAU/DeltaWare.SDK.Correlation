using Microsoft.AspNetCore.Builder;
using TraceLink.Abstractions.Context;
using TraceLink.AspNetCore.Middleware;

namespace TraceLink.AspNetCore
{
    public static class TraceLinkAspNetApplicationBuilder
    {
        public static IApplicationBuilder UseTraceLink<TTracingContext>(this IApplicationBuilder app) where TTracingContext : struct, ITracingContext
            => app.UseMiddleware<AspNetTracingScopeInitializationMiddleware<TTracingContext>>();
    }
}
