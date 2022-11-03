using System;

namespace DeltaWare.SDK.Correlation.Providers
{
    public class IdProviderWrapper<TContext, TInnerProvider> : IIdProvider<TContext> where TContext : class where TInnerProvider : IIdProvider
    {
        private readonly IIdProvider _innerProvider;

        public IdProviderWrapper()
        {
            _innerProvider = Activator.CreateInstance<TInnerProvider>();
        }

        public IdProviderWrapper(TInnerProvider instance)
        {
            _innerProvider = instance;
        }

        public string GenerateId() => _innerProvider.GenerateId();
    }
}
