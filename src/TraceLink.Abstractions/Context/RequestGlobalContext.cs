using System;

namespace TraceLink.Abstractions.Context
{
    internal readonly struct RequestGlobalContext(Guid id) : IContext
    {
        public Guid Id { get; } = id;
    }
}
