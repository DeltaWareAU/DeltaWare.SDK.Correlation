using DeltaWare.SDK.Correlation.Context;
using DeltaWare.SDK.Correlation.Forwarder;
using DeltaWare.SDK.Correlation.Options;
using NServiceBus.Pipeline;

namespace DeltaWare.SDK.Correlation.NServiceBus.Behaviors
{
    internal class AttachTraceIdBehavior : AttachContextIdBehavior
    {
        public AttachTraceIdBehavior(IIdForwarder<TraceContext> idForwarder, IOptions<TraceContext> options) : base(idForwarder, options)
        {
        }

        internal class Register : RegisterStep
        {
            public Register() : base(nameof(AttachTraceIdBehavior), typeof(AttachTraceIdBehavior), "Attach Trace Id to Outgoing Message")
            {
            }
        }
    }
}
