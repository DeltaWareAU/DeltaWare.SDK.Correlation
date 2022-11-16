using DeltaWare.SDK.Correlation.Context;
using DeltaWare.SDK.Correlation.Forwarder;
using DeltaWare.SDK.Correlation.Options;
using NServiceBus.Pipeline;

namespace DeltaWare.SDK.Correlation.NServiceBus.Behaviors
{
    internal sealed class AttachCorrelationIdBehavior : AttachContextIdBehavior
    {
        public AttachCorrelationIdBehavior(IIdForwarder<CorrelationContext> idForwarder, IOptions<CorrelationContext> options) : base(idForwarder, options)
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
