using DeltaWare.SDK.Correlation.Options;
using DeltaWare.SDK.Correlation.Providers;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace DeltaWare.SDK.Correlation.AspNetCore.Handler
{
    internal sealed class TraceIdForwardingHandler : DelegatingHandler
    {
        private readonly ITraceOptions _options;
        private readonly ITraceIdProvider _idProvider;

        public TraceIdForwardingHandler(ITraceOptions options, ITraceIdProvider idProvider)
        {
            _options = options;
            _idProvider = idProvider;
        }

        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            AttachTraceId(request.Headers);

            return base.Send(request, cancellationToken);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            AttachTraceId(request.Headers);

            return base.SendAsync(request, cancellationToken);
        }

        private void AttachTraceId(HttpRequestHeaders headers)
        {
            string traceId = _idProvider.GenerateId();

            headers.Add(_options.Header, traceId);
        }
    }
}