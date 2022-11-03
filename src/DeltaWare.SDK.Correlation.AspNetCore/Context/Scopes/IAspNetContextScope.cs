using DeltaWare.SDK.Correlation.Context.Scope;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DeltaWare.SDK.Correlation.AspNetCore.Context.Scopes
{
    internal interface IAspNetContextScope<out TContext> : IContextScope<TContext> where TContext : class
    {
        void TrySetId(bool force = false);

        Task<bool> ValidateHeaderAsync(HttpContext context, bool force = false);
    }
}
