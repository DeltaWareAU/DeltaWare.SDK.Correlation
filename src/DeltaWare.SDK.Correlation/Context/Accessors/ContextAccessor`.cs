using DeltaWare.SDK.Correlation.Context.Scope;
using System.Threading;

namespace DeltaWare.SDK.Correlation.Context.Accessors
{
    public class ContextAccessor<TContext> : IContextAccessor<TContext>
    {
        private static readonly AsyncLocal<IContextScope<TContext>> _internalScope = new AsyncLocal<IContextScope<TContext>>();

        public IContextScope<TContext> InternalScope
        {
            get => _internalScope.Value!;
            set => _internalScope.Value = value;
        }

        public IContextScope Scope => InternalScope;

        public TContext Context => InternalScope.Context;
    }
}
