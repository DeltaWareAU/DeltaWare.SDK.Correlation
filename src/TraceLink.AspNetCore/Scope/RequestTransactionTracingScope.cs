using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Options;
using TraceLink.Abstractions.Scope;
using TraceLink.AspNetCore.Attributes;
using TraceLink.AspNetCore.Enum;

namespace TraceLink.AspNetCore.Scope
{
    internal sealed class RequestTransactionTracingScope : AspNetTracingScope<RequestTransactionContext>, ITracingScope<RequestTransactionContext>
    {
        private readonly HeaderValidationManager _headerValidationManager;

        public RequestTransactionContext? Context { get; private set; }

        public RequestTransactionTracingScope(HeaderValidationManager headerValidationManager, ITracingOptions<RequestTransactionContext> options, ILogger? logger = null) : base(options, logger)
        {
            _headerValidationManager = headerValidationManager;
        }

        protected override void InitializeScope(Guid tracingId)
            => Context = new RequestTransactionContext(tracingId);

        protected override HeaderValidationRequirements GetHeaderValidationRequirements(HttpContext httpContext)
            => _headerValidationManager.GetHeaderValidationRequirements<RequestTransactionIdRequired, RequestTransactionIdNotRequired>(httpContext);
    }
}
