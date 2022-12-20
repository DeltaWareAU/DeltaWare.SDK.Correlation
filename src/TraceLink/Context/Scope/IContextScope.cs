namespace TraceLink.Abstractions.Context.Scope
{
    /// <summary>
    /// The Context Scope used for accessing the Id.
    /// </summary>
    public interface IContextScope
    {
        /// <summary>
        /// Tries to get the ID.
        /// </summary>
        /// <param name="idValue">The IDs value</param>
        /// <returns>Returns <see langword="true"/> if the ID exists; otherwise <see langword="false"/>.</returns>
        bool TryGetId(out string? idValue);

        /// <summary>
        /// Specifies if a Context Id was received.
        /// </summary>
        bool DidReceiveContextId { get; }

        /// <summary>
        /// The Context Id.
        /// </summary>
        public string ContextId { get; }
    }
}
