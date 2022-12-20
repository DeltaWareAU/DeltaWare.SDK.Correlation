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
        private readonly IContextScopeSetter<CorrelationContext> _scopeSetter;

        private readonly IIdProvider<CorrelationContext> _idProvider;

        private readonly IOptions<CorrelationContext> _options;

        private readonly ILogger? _logger;

        public RetrieveCorrelationIdBehavior(IContextScopeSetter<CorrelationContext> scopeSetter, IIdProvider<CorrelationContext> idProvider, IOptions<CorrelationContext> options, ILogger<CorrelationContext>? logger = null) : base(options, logger)
        {
            _scopeSetter = scopeSetter;
            _idProvider = idProvider;
            _options = options;
            _logger = logger;
        }

        protected override NServiceBusContextScope<CorrelationContext> CreateContextScope(IIncomingPhysicalMessageContext context)
            => new CorrelationNServiceBusContextScope(_scopeSetter, _idProvider, _options, context, _logger);

        internal sealed class Register : RegisterStep
        {
            public Register() : base(nameof(RetrieveCorrelationIdBehavior), typeof(RetrieveCorrelationIdBehavior), "Retrieve Incoming Message Correlation Id")
            {
            }
        }
    }
}
