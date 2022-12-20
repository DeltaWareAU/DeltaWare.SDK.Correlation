using TraceLink.Abstractions.Context;
using TraceLink.AspNetCore.Handler;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class CorrelationHttpClientBuilder
    {
        /// <summary>
        /// Enables the Correlation ID to be attached to the Http Headers.
        /// </summary>
        public static IHttpClientBuilder UseCorrelation(this IHttpClientBuilder builder)
        {
            builder.AddHttpMessageHandler<IdForwardingHandler<CorrelationContext>>();

            return builder;
        }

        /// <summary>
        /// Enables the Trace ID to be attached to the Http Headers.
        /// </summary>
        public static IHttpClientBuilder UseTracing(this IHttpClientBuilder builder)
        {
            builder.AddHttpMessageHandler<IdForwardingHandler<TraceContext>>();

            return builder;
        }
    }
}
