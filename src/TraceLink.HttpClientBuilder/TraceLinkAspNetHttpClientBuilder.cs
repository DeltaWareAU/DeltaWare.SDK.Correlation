using Microsoft.Extensions.DependencyInjection;
using TraceLink.Abstractions.Context;
using TraceLink.HttpClientBuilder.Handler;

namespace TraceLink.HttpClientBuilder
{
    /// <summary>
    /// Provides extension methods for configuring TraceLink in HTTP client builders.
    /// </summary>
    public static class TraceLinkAspNetHttpClientBuilder
    {
        /// <summary>
        /// Adds TraceLink support to the HTTP client by registering an outbound tracing ID handler.
        /// </summary>
        /// <typeparam name="TTracingContext">
        /// The type of tracing context associated with the handler. Must be a <see langword="struct"/> implementing <see cref="ITracingContext"/>.
        /// </typeparam>
        /// <param name="builder">The HTTP client builder instance.</param>
        /// <returns>The modified <see cref="IHttpClientBuilder"/> instance.</returns>
        public static IHttpClientBuilder UseTraceLink<TTracingContext>(this IHttpClientBuilder builder) where TTracingContext : struct, ITracingContext
            => builder.AddHttpMessageHandler<OutboundTracingIdHandler<TTracingContext>>();
    }
}
