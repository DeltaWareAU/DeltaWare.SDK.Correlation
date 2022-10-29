using DeltaWare.SDK.Correlation.Context.Accessors;
using DeltaWare.SDK.Correlation.Options;
using DeltaWare.SDK.MessageBroker.Core.Messages.Properties;

namespace DeltaWare.SDK.Correlation.MessageBroker
{
    public class CorrelationPropertyProvider : IPropertiesProvider
    {
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        private readonly ICorrelationOptions _correlationOptions;

        public CorrelationPropertyProvider(ICorrelationContextAccessor correlationContextAccessor, ICorrelationOptions correlationOptions)
        {
            _correlationContextAccessor = correlationContextAccessor;
            _correlationOptions = correlationOptions;
        }

        public IDictionary<string, object> GetProperties<T>(T message) where T : class
        {
            Dictionary<string, object> properties = new Dictionary<string, object>
            {
                { _correlationOptions.Header, _correlationContextAccessor.Context.CorrelationId }
            };

            return properties;
        }
    }
}