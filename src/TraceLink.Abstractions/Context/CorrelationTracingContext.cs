using System;

namespace TraceLink.Abstractions.Context
{
    public readonly struct CorrelationTracingContext : ITracingContext
    {
        public Guid Id { get; }

        public CorrelationTracingContext(Guid id)
        {
            Id = id;
        }
    }
}
