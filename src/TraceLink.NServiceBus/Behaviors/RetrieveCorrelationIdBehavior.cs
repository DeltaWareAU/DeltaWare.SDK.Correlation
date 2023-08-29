using Microsoft.Extensions.Logging;
using NServiceBus.Pipeline;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Context.Scope;
using TraceLink.Abstractions.Options;
using TraceLink.Abstractions.Providers;
using TraceLink.NServiceBus.Context.Scopes;

namespace TraceLink.NServiceBus.Behaviors
{
    internal sealed class RetrieveCorrelationIdBehavior : RetrieveContextIdBehavior<CorrelationContext>
    {
        private readonly ITracingScopeSetter<CorrelationContext> _scopeSetter;

        private readonly IIdProvider<CorrelationContext> _idProvider;

        private readonly ITracingOptions<CorrelationContext> _options;

        private readonly ILogger? _logger;

        public RetrieveCorrelationIdBehavior(ITracingScopeSetter<CorrelationContext> scopeSetter, IIdProvider<CorrelationContext> idProvider, ITracingOptions<CorrelationContext> options, ILogger<CorrelationContext>? logger = null) : base(options, logger)
        {
            _scopeSetter = scopeSetter;
            _idProvider = idProvider;
            _options = options;
            _logger = logger;
        }

        protected override NServiceBusTracingScope<CorrelationContext> CreateContextScope(IIncomingPhysicalMessageContext context)
            => new CorrelationNServiceBusTracingScope(_scopeSetter, _idProvider, _options, context, _logger);

        internal sealed class Register : RegisterStep
        {
            public Register() : base(nameof(RetrieveCorrelationIdBehavior), typeof(RetrieveCorrelationIdBehavior), "Retrieve Incoming Message Correlation Id")
            {
            }
        }
    }
}
