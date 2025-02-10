using System;

namespace TraceLink.Abstractions.Context
{
    internal readonly struct RequestLocalContext(Guid id) : IContext
    {
        public Guid Id { get; } = id;
    }
}
