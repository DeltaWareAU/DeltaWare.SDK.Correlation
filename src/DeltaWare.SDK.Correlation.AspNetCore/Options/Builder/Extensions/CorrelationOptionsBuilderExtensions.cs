using DeltaWare.SDK.Correlation.AspNetCore.Filters;
using DeltaWare.SDK.Correlation.AspNetCore.Options.Builder;
using DeltaWare.SDK.Correlation.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Prospa.SDK.Correlation.AspNetCore.Options.Builder
{
    public static partial class CorrelationOptionsBuilderExtensions
    {
        public static void AttachCorrelationIdToResponseHeader(this CorrelationOptionsBuilder builder)
        {
            if (builder.Services.Any(x => x.ServiceType == typeof(AttachCorrelationIdToResponseHeaderFilter)))
            {
                return;
            }

            builder.Services.AddScoped<AttachCorrelationIdToResponseHeaderFilter>();
            builder.Services.Configure<MvcOptions>(o =>
            {
                o.Filters.AddService<AttachCorrelationIdToResponseHeaderFilter>();
            });
        }

        public static void CorrelationIdHeaderRequired(this TraceOptionsBuilder builder)
        {
            builder.Services.AddScoped<CorrelationIdHeaderRequiredFilter>();
            builder.Services.Configure<MvcOptions>(o =>
            {
                o.Filters.AddService<CorrelationIdHeaderRequiredFilter>();
            });
        }

        public static void UseIdProvider<TIdProvider>(this CorrelationOptionsBuilder builder) where TIdProvider : class, ICorrelationIdProvider
        {
            builder.Services.AddSingleton<ICorrelationIdProvider, TIdProvider>();
        }
    }
}
