using NServiceBus.Pipeline;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Scope;

namespace TraceLink.NServiceBus.Scope
{
    public interface INServiceBusTracingScope<out TTracingContext> : ITracingScope<TTracingContext> where TTracingContext : struct, ITracingContext
    {
        bool TryInitializeScope(IIncomingPhysicalMessageContext context);
    }
}
