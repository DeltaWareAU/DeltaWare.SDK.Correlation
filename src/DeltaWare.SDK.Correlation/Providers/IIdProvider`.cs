namespace DeltaWare.SDK.Correlation.Providers
{
    /// <summary>
    /// Provides IDs for the specified <see cref="TContext"/>.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface IIdProvider<TContext> : IIdProvider where TContext : class
    {
    }
}
