using System;

namespace TraceLink.Abstractions.Context
{
    public sealed class CorrelationContext : ITracingContext
    {
        public string Id { get; }

        public CorrelationContext(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Id cannot be null or whitespace", nameof(id));
            }

            Id = id;
        }
    }
}
