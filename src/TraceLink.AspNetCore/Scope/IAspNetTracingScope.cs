using System.Threading;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Scope;

namespace TraceLink.AspNetCore.Scope
{
    internal interface IAspNetTracingScope<out TContext> where TContext : ITracingContext
    {
        Task<bool> ValidateHeaderAsync(HttpContext httpContext, CancellationToken cancellationToken = default);

        void InitializeScope();
    }
}
