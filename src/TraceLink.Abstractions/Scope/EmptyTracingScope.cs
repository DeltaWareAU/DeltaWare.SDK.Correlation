using TraceLink.Abstractions.Context;

namespace TraceLink.Abstractions.Scope
{
    internal readonly struct EmptyTracingScope<T> : ITracingScope<T> where T : struct, ITracingContext
    {
        public T Context => default;
    }
}
