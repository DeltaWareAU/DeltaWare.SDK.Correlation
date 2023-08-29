using System;
using TraceLink.Abstractions.Context;

namespace TraceLink.Abstractions.Providers
{
    public class IdProviderWrapper<TTracingContext, TInnerProvider> : IIdProvider<TTracingContext> where TTracingContext : ITracingContext where TInnerProvider : IIdProvider
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
