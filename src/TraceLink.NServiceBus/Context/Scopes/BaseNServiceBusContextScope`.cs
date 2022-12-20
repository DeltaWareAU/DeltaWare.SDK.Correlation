using Microsoft.Extensions.Logging;
using NServiceBus.Pipeline;
using TraceLink.Abstractions.Context.Scope;
using TraceLink.Abstractions.Options;

namespace TraceLink.NServiceBus.Context.Scopes
{
    internal abstract class NServiceBusContextScope<TContext> : IContextScope<TContext> where TContext : class
    {
        private readonly IIncomingPhysicalMessageContext _context;
        private readonly IOptions _options;

        protected ILogger? Logger { get; }

        public abstract bool DidReceiveContextId { get; }

        public abstract string ContextId { get; }

        public abstract TContext Context { get; }

        protected NServiceBusContextScope(IContextScopeSetter<TContext> contextScopeSetter, IOptions<TContext> options, IIncomingPhysicalMessageContext context, ILogger? logger = null)
        {
            contextScopeSetter.SetScope(this);

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

            if (DidReceiveContextId)
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
