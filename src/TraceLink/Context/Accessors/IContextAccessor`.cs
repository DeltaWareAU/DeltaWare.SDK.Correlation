using System;
using TraceLink.Abstractions.Context.Scope;

namespace TraceLink.Abstractions.Context.Accessors
{
    /// <summary>
    /// Used to access the specified Context.
    /// </summary>
    /// <typeparam name="TContext">The Context <see cref="Type"/>.</typeparam>
    public interface IContextAccessor<out TContext> where TContext : class
    {
        /// <summary>
        /// The current Context.
        /// </summary>
        TContext Context { get; }

        /// <summary>
        /// The Context Scope used for accessing the Id.
        /// </summary>
        IContextScope Scope { get; }
    }
}
