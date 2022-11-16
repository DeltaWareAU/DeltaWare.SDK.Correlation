using DeltaWare.SDK.Correlation.Context.Scope;
using System.Threading;

namespace DeltaWare.SDK.Correlation.Context.Accessors
{
    /// <inheritdoc cref="IContextAccessor{TContext}"/>
    public class ContextScopeSetter<TContext> : IContextAccessor<TContext> where TContext : class
    {
        private static readonly AsyncLocal<IContextScope<TContext>> _internalScope = new AsyncLocal<IContextScope<TContext>>();

        /// <summary>
        /// The Internal Scope used for the specified Context.
        /// </summary>
        public IContextScope<TContext> InternalScope
        {
            get => _internalScope.Value!;
            set => _internalScope.Value = value;
        }

        /// <inheritdoc/>
        public IContextScope Scope => InternalScope;

        /// <inheritdoc/>
        public TContext Context => InternalScope.Context;
    }
}
