using DeltaWare.SDK.Correlation.Context.Accessors;
using DeltaWare.SDK.Correlation.Context.Scope;
using DeltaWare.SDK.Correlation.Options;
using Microsoft.Extensions.Logging;
using NServiceBus.Pipeline;

namespace DeltaWare.SDK.Correlation.NServiceBus.Context.Scopes
{
    internal abstract class NServiceBusContextScope<TContext> : IContextScope<TContext> where TContext : class
    {
        private readonly IIncomingPhysicalMessageContext _context;
        private readonly IOptions _options;

        protected ILogger? Logger { get; }

        public abstract bool DidReceiveContextId { get; }

        public abstract string ContextId { get; }

        public abstract TContext Context { get; }

        protected NServiceBusContextScope(ContextScopeSetter<TContext> contextScopeSetter, IOptions<TContext> options, IIncomingPhysicalMessageContext context, ILogger? logger = null)
        {
            contextScopeSetter.InternalScope = this;

            _options = options;
            _context = context;
            Logger = logger;
        }

        public bool TryGetId(out string? idValue)
            => _context.MessageHeaders.TryGetValue(_options.Key, out idValue);

        public bool ValidateHeader(bool force = false)
        {
            if (!force)
            {
                if (_options.IsRequired)
                {
                    Logger?.LogTrace("Header Validation will be skipped as it is not required.");

                    return true;
                }
            }
            else
            {
                Logger?.LogTrace("Header Validation will done as it has been forced.");
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
