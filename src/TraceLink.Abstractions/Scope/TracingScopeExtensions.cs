using System;
using TraceLink.Abstractions.Context;

namespace TraceLink.Abstractions.Scope
{
    /// <summary>
    /// Provides extension methods for <see cref="ITracingScope{TTracingContext}"/>.
    /// </summary>
    public static class TracingScopeExtensions
    {
        /// <summary>
        /// Determines whether the specified tracing scope is empty.
        /// </summary>
        /// <typeparam name="TTracingContext">
        /// The type of tracing context. Must be a <see langword="struct"/> implementing <see cref="ITracingContext"/>.
        /// </typeparam>
        /// <param name="scope">The tracing scope to check.</param>
        /// <returns>
        /// <see langword="true"/> if the tracing scope is empty; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool IsEmpty<TTracingContext>(this ITracingScope<TTracingContext> scope) where TTracingContext : struct, ITracingContext
            => scope.Context.Id == Guid.Empty;
    }
}
