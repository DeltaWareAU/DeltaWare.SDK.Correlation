using DeltaWare.SDK.Correlation.AspNetCore.Filters;
using DeltaWare.SDK.Correlation.AspNetCore.Options.Builder;
using DeltaWare.SDK.Correlation.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Prospa.SDK.Correlation.AspNetCore.Options.Builder;

// ReSharper disable once CheckNamespace
namespace Prospa.SDK.Correlation.AspNetCore.Options
{
    public static partial class TraceOptionsBuilderExtensions
    {
        public static void AttachTraceIdToResponseHeader(this TraceOptionsBuilder builder)
        {
            builder.Services.AddScoped<AttachTraceIdToResponseHeaderFilter>();
            builder.Services.Configure<MvcOptions>(o =>
            {
                o.Filters.AddService<AttachTraceIdToResponseHeaderFilter>();
            });
        }

        public static void TraceIdHeaderRequired(this TraceOptionsBuilder builder)
        {
            builder.Services.AddScoped<TraceIdHeaderRequiredFilter>();
            builder.Services.Configure<MvcOptions>(o =>
            {
                o.Filters.AddService<TraceIdHeaderRequiredFilter>();
            });
        }

        public static void UseIdProvider<TIdProvider>(this CorrelationOptionsBuilder builder) where TIdProvider : class, ITraceIdProvider
        {
            builder.Services.AddSingleton<ITraceIdProvider, TIdProvider>();
        }
    }
}
