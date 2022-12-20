using TraceLink.Abstractions.Context.Accessors;
using TraceLink.Abstractions.Providers;

namespace TraceLink.Abstractions.Forwarder
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
