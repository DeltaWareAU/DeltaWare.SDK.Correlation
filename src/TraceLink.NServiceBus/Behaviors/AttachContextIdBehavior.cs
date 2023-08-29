using NServiceBus.Pipeline;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TraceLink.Abstractions.Forwarder;
using TraceLink.Abstractions.Options;

[assembly: InternalsVisibleTo("TraceLink.NServiceBus.Tests")]
namespace TraceLink.NServiceBus.Behaviors
{
    internal abstract class AttachContextIdBehavior : Behavior<IOutgoingPhysicalMessageContext>
    {
        private readonly IIdForwarder _idForwarder;

        private readonly ITracingOptions _options;

        protected AttachContextIdBehavior(IIdForwarder idForwarder, ITracingOptions options)
        {
            _idForwarder = idForwarder;
            _options = options;
        }

        public override Task Invoke(IOutgoingPhysicalMessageContext context, Func<Task> next)
        {
            AttachHeader(context);

            return next.Invoke();
        }

        private void AttachHeader(IOutgoingPhysicalMessageContext context)
        {
            if (context.Headers.ContainsKey(_options.Key))
            {
                return;
            }

            string forwardingId = _idForwarder.GetForwardingId();

            context.Headers.Add(_options.Key, forwardingId);
        }
    }
}
