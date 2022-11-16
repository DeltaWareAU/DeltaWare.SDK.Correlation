using System;
using System.Threading;
using DeltaWare.SDK.Correlation.Context.Accessors;
using DeltaWare.SDK.Correlation.Context.Scope;

namespace DeltaWare.SDK.Correlation.NServiceBus.Tests.Mocking
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
