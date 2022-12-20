using NServiceBus.Pipeline;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Forwarder;
using TraceLink.Abstractions.Options;

namespace TraceLink.NServiceBus.Behaviors
{
    internal sealed class AttachTraceIdBehavior : AttachContextIdBehavior
    {
        public AttachTraceIdBehavior(IIdForwarder<TraceContext> idForwarder, IOptions<TraceContext> options) : base(idForwarder, options)
        {
        }

        internal sealed class Register : RegisterStep
        {
            public Register() : base(nameof(AttachTraceIdBehavior), typeof(AttachTraceIdBehavior), "Attach Trace Id to Outgoing Message")
            {
            }
        }
    }
}
