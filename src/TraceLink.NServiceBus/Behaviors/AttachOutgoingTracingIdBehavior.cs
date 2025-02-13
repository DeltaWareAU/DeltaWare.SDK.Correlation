using NServiceBus.Pipeline;
using System;
using System.Threading.Tasks;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Options;
using TraceLink.Abstractions.Outgoing;

namespace TraceLink.NServiceBus.Behaviors
{
    internal sealed class AttachOutgoingTracingIdBehavior<TTracingContext> : Behavior<IOutgoingPhysicalMessageContext> where TTracingContext : struct, ITracingContext
    {
        private readonly IOutgoingTracingIdProvider<TTracingContext> _idProvider;
        private readonly ITracingOptions<TTracingContext> _options;

        public AttachOutgoingTracingIdBehavior(IOutgoingTracingIdProvider<TTracingContext> idProvider, ITracingOptions<TTracingContext> options)
        {
            _idProvider = idProvider;
            _options = options;
        }

        public override Task Invoke(IOutgoingPhysicalMessageContext context, Func<Task> next)
        {
            AttachHeader(context);

            return next.Invoke();
        }

        private void AttachHeader(IOutgoingPhysicalMessageContext context)
            => context.Headers.TryAdd(_options.Key, _idProvider.GetOutboundTracingId().ToString());

        internal sealed class Register() : RegisterStep(nameof(AttachOutgoingTracingIdBehavior<TTracingContext>), typeof(AttachOutgoingTracingIdBehavior<TTracingContext>), $"Attaches the Tracing ID for the specific {nameof(ITracingContext)} to the Outgoing Message");
    }
}
