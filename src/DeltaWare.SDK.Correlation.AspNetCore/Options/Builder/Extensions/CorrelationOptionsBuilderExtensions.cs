using DeltaWare.SDK.Correlation.AspNetCore.Options.Builder;
using DeltaWare.SDK.Correlation.Providers;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Prospa.SDK.Correlation.AspNetCore.Options.Builder
{
    public static partial class CorrelationOptionsBuilderExtensions
    {
        public static void UseIdProvider<TIdProvider>(this ICorrelationOptionsBuilder builder) where TIdProvider : class, ICorrelationIdProvider
        {
            builder.Services.AddSingleton<ICorrelationIdProvider, TIdProvider>();
        }
    }
}
