using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Context.Accessors;
using TraceLink.Abstractions.Providers;

namespace TraceLink.Abstractions.Forwarder
{
    public abstract class IdForwarder<TTracingContext> : IIdForwarder<TTracingContext> where TTracingContext : ITracingContext
    {
        protected IIdProvider IdProvider { get; }

        protected ITracingContextAccessor<TTracingContext> TracingContextAccessor { get; }

        protected IdForwarder(ITracingContextAccessor<TTracingContext> tracingContextAccessor, IIdProvider<TTracingContext> idProvider)
        {
            TracingContextAccessor = tracingContextAccessor;
            IdProvider = idProvider;
        }

        public abstract string GetForwardingId();
    }
}
