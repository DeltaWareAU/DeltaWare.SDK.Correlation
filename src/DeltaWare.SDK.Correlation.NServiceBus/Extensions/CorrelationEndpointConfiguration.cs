using DeltaWare.SDK.Correlation.NServiceBus.Behaviors;
using NServiceBus;

// ReSharper disable once CheckNamespace
namespace DeltaWare.SDK.Correlation.NServiceBus.Extensions
{
    public static class CorrelationEndpointConfiguration
    {
        public static void UseCorrelation(this EndpointConfiguration configuration)
        {
            configuration.RegisterComponents(components =>
            {
                components.ConfigureComponent<RetrieveCorrelationIdBehavior>(DependencyLifecycle.SingleInstance);
                components.ConfigureComponent<AttachCorrelationIdBehavior>(DependencyLifecycle.SingleInstance);
            });

            configuration.Pipeline.Register<RetrieveCorrelationIdBehavior.Register>();
            configuration.Pipeline.Register<AttachCorrelationIdBehavior.Register>();
        }
    }
}
