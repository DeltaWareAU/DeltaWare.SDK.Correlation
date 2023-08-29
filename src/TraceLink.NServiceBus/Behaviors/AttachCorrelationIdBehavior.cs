using NServiceBus.Pipeline;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Forwarder;
using TraceLink.Abstractions.Options;

namespace TraceLink.NServiceBus.Behaviors
{
    internal sealed class AttachCorrelationIdBehavior : AttachContextIdBehavior
    {
        public AttachCorrelationIdBehavior(IIdForwarder<CorrelationContext> idForwarder, ITracingOptions<CorrelationContext> options) : base(idForwarder, options)
        {
        }

        internal sealed class Register : RegisterStep
        {
            public Register() : base(nameof(AttachCorrelationIdBehavior), typeof(AttachCorrelationIdBehavior), "Attach Correlation Id to Outgoing Message")
            {
            }
        }
    }
}
