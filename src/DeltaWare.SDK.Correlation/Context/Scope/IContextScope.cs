namespace DeltaWare.SDK.Correlation.Context.Scope
{
    /// <summary>
    /// The context scope used to access the Header.
    /// </summary>
    public interface IContextScope
    {
        /// <summary>
        /// Tries to get the ID.
        /// </summary>
        /// <param name="idValue">The IDs value</param>
        /// <returns>Returns <see langword="true"/> if the ID exists; otherwise <see langword="false"/>.</returns>
        bool TryGetId(out string? idValue);
    }
}
