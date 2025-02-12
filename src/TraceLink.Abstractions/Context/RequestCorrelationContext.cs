using System;

namespace TraceLink.Abstractions.Context
{
    public sealed class RequestCorrelationContext : ITracingContext
    {
        public RequestCorrelationContext(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
