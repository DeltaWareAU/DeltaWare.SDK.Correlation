using Microsoft.Extensions.DependencyInjection;
using TraceLink.Abstractions.Context;
using TraceLink.AspNetCore.Handler;

namespace TraceLink.AspNetCore
{
    public static class TraceLinkAspNetHttpClientBuilder
    {
        public static IHttpClientBuilder UseTracing<TTracingContext>(this IHttpClientBuilder builder) where TTracingContext : struct, ITracingContext
            => builder.AddHttpMessageHandler<OutboundTracingIdHandler<TTracingContext>>();

        public static IHttpClientBuilder UseTracing(this IHttpClientBuilder builder)
            => builder.UseTracing<CorrelationTracingContext>();
    }
}
