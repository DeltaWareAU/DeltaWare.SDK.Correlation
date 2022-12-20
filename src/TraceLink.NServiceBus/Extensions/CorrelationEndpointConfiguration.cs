using TraceLink.NServiceBus.Behaviors;

// ReSharper disable once CheckNamespace
namespace NServiceBus
{
    public static partial class CorrelationEndpointConfiguration
    {
        /// <summary>
        /// Adds the Correlation Middleware to NServiceBus.
        /// </summary>
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
