using Microsoft.Extensions.Logging;
using NServiceBus.Pipeline;
using System;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Context.Factory;
using TraceLink.Abstractions.Options;

namespace TraceLink.NServiceBus.Scope
{
    internal sealed class NServiceBusTracingScope<TTracingContext> : INServiceBusTracingScope<TTracingContext> where TTracingContext : struct, ITracingContext
    {
        private readonly ITracingContextFactory<TTracingContext> _contextFactory;
        private readonly ITracingOptions<TTracingContext> _options;
        private readonly ILogger<NServiceBusTracingScope<TTracingContext>>? _logger;

        public TTracingContext Context { get; private set; }

        public NServiceBusTracingScope(ITracingContextFactory<TTracingContext> contextFactory, ITracingOptions<TTracingContext> options, ILogger<NServiceBusTracingScope<TTracingContext>>? logger = null)
        {
            _contextFactory = contextFactory;
            _options = options;
            _logger = logger;
        }

        public bool TryInitializeScope(IIncomingPhysicalMessageContext context)
        {
            if (TryGetTracingId(context, out var tracingId))
            {
                Context = InitializeContext(tracingId);

                return true;
            }

            if (_options.IsRequired)
            {
                return false;
            }

            Context = InitializeContext(GenerateTracingId());

            return true;
        }

        private bool TryGetTracingId(IIncomingPhysicalMessageContext context, out Guid tracingId)
        {
            tracingId = Guid.Empty;

            if (!context.MessageHeaders.TryGetValue(_options.Key, out var tracingIdValue))
            {
                return false;
            }

            return Guid.TryParse(tracingIdValue, out tracingId);
        }

        private TTracingContext InitializeContext(Guid tracingId)
            => _contextFactory.CreateContext(tracingId);

        private Guid GenerateTracingId()
            => Guid.NewGuid();
    }
}
