using Microsoft.Extensions.DependencyInjection;
using TraceLink.Abstractions.Context;
using TraceLink.AspNetCore.Scope;

namespace TraceLink.AspNetCore.Extensions
{
    public static class TracingServiceCollectionExtensions
    {
        public static IServiceCollection AddRequestTracing(this IServiceCollection services) 
            => services
                .AddSingleton<HeaderValidationManager>()
                .AddScoped<IAspNetTracingScope<RequestCorrelationContext>, RequestCorrelationTracingScope>()
                .AddScoped<IAspNetTracingScope<RequestTransactionContext>, RequestTransactionTracingScope>();
    }
}
