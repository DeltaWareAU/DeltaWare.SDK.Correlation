using System;

namespace TraceLink.Abstractions.Context.Accessors
{
    /// <summary>
    /// Used to access the specified Context.
    /// </summary>
    /// <typeparam name="TTracingContext">The Context <see cref="Type"/>.</typeparam>
    public interface ITracingContextAccessor<out TTracingContext> where TTracingContext : ITracingContext
    {
        /// <summary>
        /// The Tracing Context.
        /// </summary>
        TTracingContext Context { get; }

        ///// <summary>
        ///// The Tracing Context Scope.
        ///// </summary>
        //ITracingScope<TTracingContext> Scope { get; }
    }
}
