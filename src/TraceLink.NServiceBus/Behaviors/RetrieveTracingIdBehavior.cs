using Microsoft.Extensions.Logging;
using NServiceBus.Pipeline;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Options;
using TraceLink.Abstractions.Scope;
using TraceLink.NServiceBus.Scope;

namespace TraceLink.NServiceBus.Behaviors
{
    internal sealed class RetrieveTracingIdBehavior<TTracingContext> : Behavior<IIncomingPhysicalMessageContext> where TTracingContext : struct, ITracingContext
    {
        private readonly INServiceBusTracingScope<TTracingContext> _tracingScope;
        private readonly ITracingScopeSetter<TTracingContext> _scopeSetter;
        private readonly ITracingScopeAccessor<TTracingContext> _scopeAccessor;
        private readonly ITracingOptions<TTracingContext> _options;
        private readonly ILogger? _logger;

        public RetrieveTracingIdBehavior(INServiceBusTracingScope<TTracingContext> tracingScope, ITracingScopeSetter<TTracingContext> scopeSetter, ITracingScopeAccessor<TTracingContext> scopeAccessor, ITracingOptions<TTracingContext> options, ILogger? logger = null)
        {
            _tracingScope = tracingScope;
            _scopeSetter = scopeSetter;
            _scopeAccessor = scopeAccessor;
            _options = options;
            _logger = logger;
        }

        public override async Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
        {
            if (!_tracingScope.TryInitializeScope(context))
            {
                throw new ArgumentException();
            }

            _scopeSetter.SetTracingScope(_tracingScope);

            if (_logger == null || !_options.AttachToLoggingScope)
            {
                await next();

                return;
            }

            using (_logger.BeginScope(GetLoggingState()))
            {
                await next();
            }
        }

        private IReadOnlyDictionary<string, string> GetLoggingState()
            => new Dictionary<string, string>
            {
                [_options.LoggingScopeKey] = _scopeAccessor.Scope.Context!.Id.ToString()
            };

        internal sealed class Register() : RegisterStep(nameof(RetrieveTracingIdBehavior<TTracingContext>), typeof(RetrieveTracingIdBehavior<TTracingContext>), $"Retrieves the Tracing ID for the specific {nameof(ITracingContext)} from the Incoming Message");
    }
}
