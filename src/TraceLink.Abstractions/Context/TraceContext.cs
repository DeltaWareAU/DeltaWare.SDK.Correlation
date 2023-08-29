using System;

namespace TraceLink.Abstractions.Context
{
    public sealed class TraceContext : ITracingContext
    {
        public string Id { get; }

        public TraceContext(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Id cannot be null or whitespace", nameof(id));
            }

            Id = id;
        }
    }
}
