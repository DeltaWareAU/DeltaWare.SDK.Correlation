using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Context.Accessors;
using TraceLink.Abstractions.Context.Scope;

namespace TraceLink.NServiceBus.Tests.Mocking
{
    /// <inheritdoc cref="ITracingContextAccessor{TContext}"/>
    public class MockTracingTracingScope<TTracingContext> : ITracingContextAccessor<TTracingContext>, ITracingScopeSetter<TTracingContext> where TTracingContext : ITracingContext
    {
        private ITracingScope<TTracingContext>? _internalScope = null;

        /// <inheritdoc/>
        public ITracingScope Scope => _internalScope;

        /// <inheritdoc/>
        public TTracingContext Context => _internalScope.Context;

        public void SetScope(ITracingScope<TTracingContext> tracingScope)
            => _internalScope = tracingScope;
    }
}
