using DeltaWare.SDK.Correlation.NServiceBus.Behaviors;

// ReSharper disable once CheckNamespace
namespace NServiceBus
{
    public static partial class TraceEndpointConfiguration
    {
        /// <summary>
        /// Adds the Tracing Middleware to NServiceBus.
        /// </summary>
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
