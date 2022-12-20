using TraceLink.Abstractions.Context.Accessors;
using TraceLink.Abstractions.Context.Scope;

namespace TraceLink.NServiceBus.Tests.Mocking
{
    /// <inheritdoc cref="IContextAccessor{TContext}"/>
    public class MockContextScope<TContext> : IContextAccessor<TContext>, IContextScopeSetter<TContext> where TContext : class
    {
        private IContextScope<TContext>? _internalScope = null;

        /// <inheritdoc/>
        public IContextScope Scope => _internalScope;

        /// <inheritdoc/>
        public TContext Context => _internalScope.Context;

        public void SetScope(IContextScope<TContext> contextScope)
            => _internalScope = contextScope;
    }
}
