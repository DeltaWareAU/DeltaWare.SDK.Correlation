using Microsoft.Extensions.DependencyInjection;
using TraceLink.Abstractions.Context;
using TraceLink.AspNetCore.Handler;

namespace TraceLink.AspNetCore
{
    public static class TraceLinkAspNetHttpClientBuilder
    {
        public static IHttpClientBuilder UseTraceLink<TTracingContext>(this IHttpClientBuilder builder) where TTracingContext : struct, ITracingContext
            => builder.AddHttpMessageHandler<OutboundTracingIdHandler<TTracingContext>>();
    }
}
