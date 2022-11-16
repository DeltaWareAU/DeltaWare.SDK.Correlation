using DeltaWare.SDK.Correlation.NServiceBus.Behaviors;
using NServiceBus;

// ReSharper disable once CheckNamespace
namespace DeltaWare.SDK.Correlation.NServiceBus.Extensions
{
    public static class TraceEndpointConfiguration
    {
        public static void UseTracing(this EndpointConfiguration configuration)
        {
            configuration.RegisterComponents(components =>
            {
                components.ConfigureComponent<RetrieveTraceIdBehavior>(DependencyLifecycle.SingleInstance);
                components.ConfigureComponent<AttachTraceIdBehavior>(DependencyLifecycle.SingleInstance);
            });

            configuration.Pipeline.Register<RetrieveTraceIdBehavior.Register>();
            configuration.Pipeline.Register<AttachTraceIdBehavior.Register>();
        }
    }
}
