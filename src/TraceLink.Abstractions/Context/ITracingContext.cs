using System;

namespace TraceLink.Abstractions.Context
{
    public interface ITracingContext
    {
        Guid Id { get; }
    }
}
