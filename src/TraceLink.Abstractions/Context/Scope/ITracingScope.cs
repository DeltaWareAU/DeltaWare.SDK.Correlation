namespace TraceLink.Abstractions.Context.Scope
{
    public interface ITracingScope
    {
        /// <summary>
        /// Tries to get the Tracing Id.
        /// </summary>
        /// <param name="idValue">The Tracing Ids value.</param>
        /// <returns>Returns <see langword="true"/> if the Tracing Id exists; otherwise <see langword="false"/>.</returns>
        bool TryGetId(out string? idValue);

        /// <summary>
        /// Specifies if a Tracing Context Id was received.
        /// </summary>
        bool ReceivedId { get; }

        /// <summary>
        /// The Tracing Context Id.
        /// </summary>
        string? Id { get; }
    }
}
