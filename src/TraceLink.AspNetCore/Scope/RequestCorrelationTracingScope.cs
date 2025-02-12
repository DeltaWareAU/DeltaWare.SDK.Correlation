using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Options;
using TraceLink.Abstractions.Scope;
using TraceLink.AspNetCore.Attributes;
using TraceLink.AspNetCore.Enum;

namespace TraceLink.AspNetCore.Scope
{
    internal sealed class RequestCorrelationTracingScope : AspNetTracingScope<RequestCorrelationContext>, ITracingScope<RequestCorrelationContext>
    {
        private readonly HeaderValidationManager _headerValidationManager;

        public RequestCorrelationContext? Context { get; private set; }

        public RequestCorrelationTracingScope(HeaderValidationManager headerValidationManager, ITracingOptions<RequestCorrelationContext> options, ILogger? logger = null) : base(options, logger)
        {
            _headerValidationManager = headerValidationManager;
        }

        protected override void InitializeScope(Guid tracingId)
            => Context = new RequestCorrelationContext(tracingId);

        protected override HeaderValidationRequirements GetHeaderValidationRequirements(HttpContext httpContext)
            => _headerValidationManager.GetHeaderValidationRequirements<RequestCorrelationIdRequired, RequestCorrelationIdNotRequired>(httpContext);
    }
}
