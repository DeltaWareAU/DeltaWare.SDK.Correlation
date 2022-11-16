using DeltaWare.SDK.Correlation.Context;
using DeltaWare.SDK.Correlation.Context.Scope;
using DeltaWare.SDK.Correlation.NServiceBus.Context.Scopes;
using DeltaWare.SDK.Correlation.Options;
using Microsoft.Extensions.Logging;
using NServiceBus.Pipeline;

namespace DeltaWare.SDK.Correlation.NServiceBus.Behaviors
{
    internal sealed class RetrieveTraceIdBehavior : RetrieveContextIdBehavior<TraceContext>
    {
        private readonly IContextScopeSetter<TraceContext> _scopeSetter;

        private readonly IOptions<TraceContext> _options;

        private readonly ILogger? _logger;

        public RetrieveTraceIdBehavior(IContextScopeSetter<TraceContext> scopeSetter, IOptions<TraceContext> options, ILogger<TraceContext>? logger = null) : base(options, logger)
        {
            _scopeSetter = scopeSetter;
            _options = options;
            _logger = logger;
        }

        protected override NServiceBusContextScope<TraceContext> CreateContextScope(IIncomingPhysicalMessageContext context)
            => new TraceNServiceBusContextScope(_scopeSetter, _options, context, _logger);

        internal sealed class Register : RegisterStep
        {
            public Register() : base(nameof(RetrieveTraceIdBehavior), typeof(RetrieveTraceIdBehavior), "Retrieve Incoming Message Trace Id")
            {
            }
        }
    }
}
