using System;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Scope;

namespace TraceLink.Abstractions.Outbound
{
    internal sealed class OutboundTracingIdProvider<TTracingContext> : IOutboundTracingIdProvider<TTracingContext> where TTracingContext : struct, ITracingContext
    {
        private readonly ITracingScopeAccessor<TTracingContext> _tracingScopeAccessor;

        public OutboundTracingIdProvider(ITracingScopeAccessor<TTracingContext> tracingScopeAccessor)
        {
            _tracingScopeAccessor = tracingScopeAccessor;
        }

        public Guid GetOutboundTracingId()
        {
            if (_tracingScopeAccessor.Scope.IsEmpty())
            {
                return Guid.NewGuid();
            }

            return _tracingScopeAccessor.Scope.Context.Id;
        }
    }
}
