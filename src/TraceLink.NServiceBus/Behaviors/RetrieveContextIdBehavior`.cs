using Microsoft.Extensions.Logging;
using NServiceBus.Pipeline;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Options;
using TraceLink.NServiceBus.Context.Scopes;

[assembly: InternalsVisibleTo("TraceLink.NServiceBus.Tests")]
namespace TraceLink.NServiceBus.Behaviors
{
    internal abstract class RetrieveContextIdBehavior<TTracingContext> : Behavior<IIncomingPhysicalMessageContext> where TTracingContext : ITracingContext
    {
        private readonly ITracingOptions _options;
        private readonly ILogger? _logger;

        protected RetrieveContextIdBehavior(ITracingOptions options, ILogger? logger)
        {
            _options = options;
            _logger = logger;
        }

        public override async Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
        {
            NServiceBusTracingScope<TTracingContext> tracingScope = CreateContextScope(context);

            if (!tracingScope.ValidateHeader())
            {
                throw new ArgumentException($"{_options.Key} was not Attached to the Headers of the Incoming Message.");
            }

            if (_logger == null || !_options.AttachToLoggingScope)
            {
                await next();

                return;
            }

            Dictionary<string, string> state = new Dictionary<string, string>
            {
                [_options.LoggingScopeKey] = tracingScope.Id
            };

            using (_logger.BeginScope(state))
            {
                await next();
            }
        }

        protected abstract NServiceBusTracingScope<TTracingContext> CreateContextScope(IIncomingPhysicalMessageContext context);
    }
}
