using DeltaWare.SDK.Correlation.Context.Accessors;
using DeltaWare.SDK.Correlation.Providers;

namespace DeltaWare.SDK.Correlation.Forwarder
{
    public abstract class BaseIdForwarder<TContext> : IIdForwarder<TContext> where TContext : class
    {
        protected IIdProvider IdProvider { get; }

        protected IContextAccessor<TContext> ContextAccessor { get; }

        protected BaseIdForwarder(IContextAccessor<TContext> contextAccessor, IIdProvider<TContext> idProvider)
        {
            ContextAccessor = contextAccessor;
            IdProvider = idProvider;
        }

        public abstract string GetForwardingId();
    }
}
