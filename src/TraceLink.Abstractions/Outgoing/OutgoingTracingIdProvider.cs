using System;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Scope;

namespace TraceLink.Abstractions.Outgoing
{
    internal sealed class OutgoingTracingIdProvider<TTracingContext> : IOutgoingTracingIdProvider<TTracingContext> where TTracingContext : struct, ITracingContext
    {
        private readonly ITracingScopeAccessor<TTracingContext> _tracingScopeAccessor;

        public OutgoingTracingIdProvider(ITracingScopeAccessor<TTracingContext> tracingScopeAccessor)
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
