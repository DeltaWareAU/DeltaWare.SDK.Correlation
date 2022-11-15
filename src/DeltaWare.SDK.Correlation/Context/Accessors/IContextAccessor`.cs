using DeltaWare.SDK.Correlation.Context.Scope;
using System;

namespace DeltaWare.SDK.Correlation.Context.Accessors
{
    /// <summary>
    /// Used to access the specified Context.
    /// </summary>
    /// <typeparam name="TContext">The Context <see cref="Type"/>.</typeparam>
    public interface IContextAccessor<out TContext>
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
