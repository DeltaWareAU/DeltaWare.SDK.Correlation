using Microsoft.Extensions.Logging;
using NServiceBus.Pipeline;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Context.Scope;
using TraceLink.Abstractions.Options;

namespace TraceLink.NServiceBus.Context.Scopes
{
    internal abstract class NServiceBusTracingScope<TTracingContext> : ITracingScope<TTracingContext> where TTracingContext : ITracingContext
    {
        private readonly IIncomingPhysicalMessageContext _context;
        private readonly ITracingOptions _options;

        protected ILogger? Logger { get; }

        public abstract bool ReceivedId { get; }

        public abstract string Id { get; }

        public abstract TTracingContext Context { get; }

        protected NServiceBusTracingScope(ITracingScopeSetter<TTracingContext> tracingScopeSetter, ITracingOptions<TTracingContext> options, IIncomingPhysicalMessageContext context, ILogger? logger = null)
        {
            tracingScopeSetter.SetScope(this);

            _options = options;
            _context = context;
            Logger = logger;
        }

        public bool TryGetId(out string? idValue)
            => _context.MessageHeaders.TryGetValue(_options.Key, out idValue);

        public bool ValidateHeader(bool force = false)
        {
            if (force)
            {
                Logger?.LogTrace("Header Validation will done as it has been forced.");
            }
            else if (!_options.IsRequired)
            {
                Logger?.LogTrace("Header Validation will be skipped as it is not required.");

                return true;
            }

            if (ReceivedId)
            {
                OnValidationPassed();

                return true;
            }

            OnValidationFailed();

            return false;
        }

        protected abstract void OnValidationPassed();

        protected abstract void OnValidationFailed();
    }
}
