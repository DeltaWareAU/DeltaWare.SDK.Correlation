using Microsoft.AspNetCore.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Context.Scope;

[assembly: InternalsVisibleTo("TraceLink.AspNetCore.Tests")]
namespace TraceLink.AspNetCore.Context.Scopes
{
    internal interface IAspNetTracingScope<out TTracingContext> : ITracingScope<TTracingContext> where TTracingContext : ITracingContext
    {
        void TrySetId(bool force = false);

        Task<bool> ValidateHeaderAsync(HttpContext context, bool force = false);
    }
}
