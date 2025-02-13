using System;
using TraceLink.Abstractions.Context;

namespace TraceLink.Abstractions.Scope
{
    public static class TracingScopeExtensions
    {
        public static bool IsEmpty<TTracingContext>(this ITracingScope<TTracingContext> scope) where TTracingContext : struct, ITracingContext
            => scope.Context.Id == Guid.Empty;
    }
}
