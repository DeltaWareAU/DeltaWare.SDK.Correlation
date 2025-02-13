using System;
using TraceLink.Abstractions.Context;

namespace TraceLink.AspNetCore.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class TracingIdRequired<TTracingContext> : Attribute where TTracingContext : struct, ITracingContext
    {
    }
}
