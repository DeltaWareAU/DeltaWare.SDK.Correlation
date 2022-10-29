using DeltaWare.SDK.Correlation.Context.Scope;

namespace DeltaWare.SDK.Correlation.Context.Accessors
{
    public class BaseContextAccessor<TContext> : IContextAccessor<TContext>
    {
        private static readonly AsyncLocal<IContextScope<TContext>> _internalScope = new();

        public IContextScope<TContext> InternalScope
        {
            get => _internalScope.Value!;
            set => _internalScope.Value = value;
        }

        public IContextScope Scope => InternalScope;

        public TContext Context => InternalScope.Context;
    }
}
