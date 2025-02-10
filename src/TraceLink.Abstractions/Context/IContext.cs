using System;

namespace TraceLink.Abstractions.Context
{
    public interface IContext
    {
        Guid Id { get; }
    }
}
