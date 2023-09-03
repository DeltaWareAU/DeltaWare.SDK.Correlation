using System.Threading;
using TraceLink.Abstractions.Context.Accessors;

namespace TraceLink.Abstractions.Context.Scope
{
    public sealed class AsyncLocalTracingScope<TTracingContext> : ITracingContextAccessor<TTracingContext>, ITracingScopeSetter<TTracingContext> where TTracingContext : ITracingContext
    {
        private static readonly AsyncLocal<ITracingScope<TTracingContext>> AsyncLocalScope = new AsyncLocal<ITracingScope<TTracingContext>>();

        /// <inheritdoc/>
        public TTracingContext Context => AsyncLocalScope.Value!.Context;

        /// <inheritdoc/>
        public bool ReceivedId => AsyncLocalScope.Value?.ReceivedId ?? false;

        /// <inheritdoc/>
        public void SetScope(ITracingScope<TTracingContext> tracingScope)
            => AsyncLocalScope.Value = tracingScope;
    }
}
