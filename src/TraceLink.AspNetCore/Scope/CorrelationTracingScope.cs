using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Options;
using TraceLink.AspNetCore.Attributes;
using TraceLink.AspNetCore.Enum;
using TraceLink.AspNetCore.Validation;

namespace TraceLink.AspNetCore.Scope
{
    internal sealed class CorrelationTracingScope : AspNetTracingScope<CorrelationTracingContext>
    {
        private readonly IHeaderValidationManager _headerValidationManager;

        public CorrelationTracingScope(IHeaderValidationManager headerValidationManager, ITracingOptions<CorrelationTracingContext> options, ILogger<CorrelationTracingScope>? logger = null) : base(options, logger)
        {
            _headerValidationManager = headerValidationManager;
        }

        protected override CorrelationTracingContext InitializeContext(Guid tracingId) => new(tracingId);

        protected override HeaderValidationRequirements GetHeaderValidationRequirements(HttpContext httpContext)
            => _headerValidationManager.GetHeaderValidationRequirements<RequestCorrelationIdRequired, RequestCorrelationIdNotRequired>(httpContext);
    }
}
