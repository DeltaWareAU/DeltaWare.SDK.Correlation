using DeltaWare.SDK.Correlation.AspNetCore.Options.Builder;
using DeltaWare.SDK.Correlation.Providers;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Prospa.SDK.Correlation.AspNetCore.Options
{
    public static partial class TraceOptionsBuilderExtensions
    {
        public static void UseIdProvider<TIdProvider>(this CorrelationOptionsBuilder builder) where TIdProvider : class, ITraceIdProvider
        {
            builder.Services.AddSingleton<ITraceIdProvider, TIdProvider>();
        }
    }
}
