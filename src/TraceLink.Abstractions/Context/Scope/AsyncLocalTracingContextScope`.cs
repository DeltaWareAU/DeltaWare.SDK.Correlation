using System.Threading;
using TraceLink.Abstractions.Context.Accessors;

namespace TraceLink.Abstractions.Context.Scope
{
    /// <inheritdoc cref="ITracingContextAccessor{TContext}"/>
    public sealed class AsyncLocalTracingScope<TTracingContext> : ITracingContextAccessor<TTracingContext>, ITracingScopeSetter<TTracingContext> where TTracingContext : ITracingContext
    {
        private static readonly AsyncLocal<ITracingScope<TTracingContext>> _internalScope = new AsyncLocal<ITracingScope<TTracingContext>>();

        /// <inheritdoc/>
        public TTracingContext Context => _internalScope.Value!.Context;

        public void SetScope(ITracingScope<TTracingContext> tracingScope)
            => _internalScope.Value = tracingScope;
    }
}
