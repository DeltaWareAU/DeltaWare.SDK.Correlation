using Microsoft.Extensions.Logging;
using NServiceBus.Pipeline;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Context.Scope;
using TraceLink.Abstractions.Options;
using TraceLink.NServiceBus.Context.Scopes;

namespace TraceLink.NServiceBus.Behaviors
{
    internal sealed class RetrieveTraceIdBehavior : RetrieveContextIdBehavior<TraceContext>
    {
        private readonly ITracingScopeSetter<TraceContext> _scopeSetter;

        private readonly ITracingOptions<TraceContext> _options;

        private readonly ILogger? _logger;

        public RetrieveTraceIdBehavior(ITracingScopeSetter<TraceContext> scopeSetter, ITracingOptions<TraceContext> options, ILogger<TraceContext>? logger = null) : base(options, logger)
        {
            _scopeSetter = scopeSetter;
            _options = options;
            _logger = logger;
        }

        protected override NServiceBusTracingScope<TraceContext> CreateContextScope(IIncomingPhysicalMessageContext context)
            => new TraceNServiceBusTracingScope(_scopeSetter, _options, context, _logger);

        internal sealed class Register : RegisterStep
        {
            public Register() : base(nameof(RetrieveTraceIdBehavior), typeof(RetrieveTraceIdBehavior), "Retrieve Incoming Message Trace Id")
            {
            }
        }
    }
}
