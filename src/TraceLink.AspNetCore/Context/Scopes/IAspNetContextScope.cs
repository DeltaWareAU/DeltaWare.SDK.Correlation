using Microsoft.AspNetCore.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TraceLink.Abstractions.Context.Scope;

[assembly: InternalsVisibleTo("TraceLink.AspNetCore.Tests")]
namespace TraceLink.AspNetCore.Context.Scopes
{
    internal interface IAspNetContextScope<out TContext> : IContextScope<TContext> where TContext : class
    {
        void TrySetId(bool force = false);

        Task<bool> ValidateHeaderAsync(HttpContext context, bool force = false);
    }
}
