using System;
using TraceLink.Abstractions.Context;

namespace TraceLink.AspNetCore.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class TracingIdNotRequired<TTracingContext> : Attribute where TTracingContext : struct, ITracingContext
    {
    }
}
