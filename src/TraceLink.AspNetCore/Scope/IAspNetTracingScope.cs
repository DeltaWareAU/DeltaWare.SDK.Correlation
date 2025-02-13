using Microsoft.AspNetCore.Http;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Scope;

namespace TraceLink.AspNetCore.Scope
{
    public interface IAspNetTracingScope<out TTracingContext> : ITracingScope<TTracingContext> where TTracingContext : struct, ITracingContext
    {
        bool TryInitializeScope(HttpContext httpContext);
    }
}
