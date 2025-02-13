using Microsoft.Extensions.DependencyInjection;
using TraceLink.Abstractions.Context;
using TraceLink.HttpClientBuilder.Handler;

namespace TraceLink.HttpClientBuilder
{
    public static class TraceLinkAspNetHttpClientBuilder
    {
        public static IHttpClientBuilder UseTraceLink<TTracingContext>(this IHttpClientBuilder builder) where TTracingContext : struct, ITracingContext
            => builder.AddHttpMessageHandler<OutboundTracingIdHandler<TTracingContext>>();
    }
}
