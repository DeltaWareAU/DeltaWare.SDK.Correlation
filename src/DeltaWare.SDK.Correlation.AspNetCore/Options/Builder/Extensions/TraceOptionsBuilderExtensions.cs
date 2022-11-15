using DeltaWare.SDK.Correlation.AspNetCore.Options.Builder;
using DeltaWare.SDK.Correlation.Context;
using DeltaWare.SDK.Correlation.Forwarder;
using DeltaWare.SDK.Correlation.Providers;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Prospa.SDK.Correlation.AspNetCore.Options.Builder
{
    public static partial class TraceOptionsBuilderExtensions
    {
        public static void UseIdProvider<TIdProvider>(this ITraceOptionsBuilder builder) where TIdProvider : class, IIdProvider
        {
            builder.Services.AddSingleton<IIdProvider<TraceContext>, IdProviderWrapper<TraceContext, TIdProvider>>();
        }

        public static void UseIdProvider<TIdProvider>(this ITraceOptionsBuilder builder, TIdProvider instance) where TIdProvider : class, IIdProvider
        {
            builder.Services.AddSingleton<IIdProvider<TraceContext>>(new IdProviderWrapper<TraceContext, TIdProvider>(instance));
        }

        public static void UseIdForwarder<TIdForwarder>(this ITraceOptionsBuilder builder) where TIdForwarder : class, IIdForwarder<TraceContext>
        {
            builder.Services.AddSingleton<IIdForwarder<TraceContext>, TIdForwarder>();
        }
    }
}
