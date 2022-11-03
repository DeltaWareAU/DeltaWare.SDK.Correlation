using DeltaWare.SDK.Correlation.AspNetCore.Options.Builder;
using DeltaWare.SDK.Correlation.Context;
using DeltaWare.SDK.Correlation.Forwarder;
using DeltaWare.SDK.Correlation.Providers;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Prospa.SDK.Correlation.AspNetCore.Options.Builder
{
    public static partial class CorrelationOptionsBuilderExtensions
    {
        public static void UseIdProvider<TIdProvider>(this ICorrelationOptionsBuilder builder) where TIdProvider : IIdProvider
        {
            builder.Services.AddSingleton<IIdProvider<CorrelationContext>, IdProviderWrapper<CorrelationContext, TIdProvider>>();
        }

        public static void UseIdProvider<TIdProvider>(this ITraceOptionsBuilder builder, TIdProvider instance) where TIdProvider : IIdProvider
        {
            builder.Services.AddSingleton<IIdProvider<CorrelationContext>>(new IdProviderWrapper<CorrelationContext, TIdProvider>(instance));
        }

        public static void UseIdForwarder<TIdForwarder>(this ITraceOptionsBuilder builder) where TIdForwarder : class, IIdForwarder<CorrelationContext>
        {
            builder.Services.AddSingleton<IIdForwarder<CorrelationContext>, TIdForwarder>();
        }
    }
}
