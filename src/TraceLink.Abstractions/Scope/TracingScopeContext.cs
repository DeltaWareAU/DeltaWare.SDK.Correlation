using System;
using TraceLink.Abstractions.Context;

namespace TraceLink.Abstractions.Scope
{
    internal sealed class TracingScopeContext<TTracingContext> : ITracingScopeAccessor<TTracingContext>, ITracingScopeSetter<TTracingContext>, IDisposable where TTracingContext : struct, ITracingContext
    {
        private readonly object _lock = new object();

        private bool _disposed;

        private ITracingScope<TTracingContext> _scope = new EmptyTracingScope<TTracingContext>();

        public ITracingScope<TTracingContext> Scope => _disposed ? throw new ObjectDisposedException(GetType().Name, "The Scope has been Disposed and can no longer be accessed.") : _scope;

        public void SetTracingScope<TTracingScope>(TTracingScope tracingScope) where TTracingScope : ITracingScope<TTracingContext>
        {
            lock (_lock)
            {
                if (!Scope.IsEmpty())
                {
                    throw new InvalidOperationException("The Scope cannot be set more than once.");
                }

                _scope = tracingScope;
            }
        }

        public void Dispose() => _disposed = true;
    }
}
