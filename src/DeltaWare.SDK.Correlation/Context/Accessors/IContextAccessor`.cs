using DeltaWare.SDK.Correlation.Context.Scope;
using System;

namespace DeltaWare.SDK.Correlation.Context.Accessors
{
    /// <summary>
    /// Used to access the Context.
    /// </summary>
    /// <typeparam name="TContext">The Context <see cref="Type"/>.</typeparam>
    public interface IContextAccessor<out TContext>
    {
        /// <summary>
        /// The current Context.
        /// </summary>
        TContext Context { get; }

        /// <summary>
        /// The Context Scope used to access the Header.
        /// </summary>
        IContextScope Scope { get; }
    }
}
