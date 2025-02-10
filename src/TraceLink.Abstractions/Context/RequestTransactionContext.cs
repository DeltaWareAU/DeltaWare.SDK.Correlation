using System;

namespace TraceLink.Abstractions.Context
{
    internal readonly struct RequestTransactionContext(Guid id) : IContext
    {
        public Guid Id { get; } = id;
    }
}
