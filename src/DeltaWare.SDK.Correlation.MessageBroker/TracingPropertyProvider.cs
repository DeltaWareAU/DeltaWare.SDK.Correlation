using DeltaWare.SDK.Correlation.Options;
using DeltaWare.SDK.Correlation.Providers;
using DeltaWare.SDK.MessageBroker.Core.Messages.Properties;

namespace DeltaWare.SDK.Correlation.MessageBroker
{
    public class TracingPropertyProvider : IPropertiesProvider
    {
        private readonly ITraceIdProvider _traceIdProvider;

        private readonly ITraceOptions _traceOptions;

        public TracingPropertyProvider(ITraceIdProvider traceIdProvider, ITraceOptions traceOptions)
        {
            _traceIdProvider = traceIdProvider;
            _traceOptions = traceOptions;
        }

        public IDictionary<string, object> GetProperties<T>(T message) where T : class
        {
            Dictionary<string, object> properties = new Dictionary<string, object>
            {
                { _traceOptions.Header, _traceIdProvider.GenerateId() }
            };

            return properties;
        }
    }
}