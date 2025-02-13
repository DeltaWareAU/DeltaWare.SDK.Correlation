using System;
using TraceLink.Abstractions.Context;

namespace TraceLink.AspNetCore.Attributes
{
    /// <summary>
    /// Specifies that a tracing identifier is required for the associated class or method.
    /// </summary>
    /// <typeparam name="TTracingContext">
    /// The type of tracing context. Must be a <see langword="struct"/> implementing <see cref="ITracingContext"/>.
    /// </typeparam>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class TracingIdRequired<TTracingContext> : Attribute where TTracingContext : struct, ITracingContext
    {
    }
}
