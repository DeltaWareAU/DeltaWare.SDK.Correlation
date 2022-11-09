using DeltaWare.SDK.Correlation.Forwarder;
using DeltaWare.SDK.Correlation.Options;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace DeltaWare.SDK.Correlation.AspNetCore.Handler
{
    internal sealed class IdForwardingHandler<TContext> : DelegatingHandler where TContext : class
    {
        private readonly IOptions _options;

        private readonly IIdForwarder _idForwarder;

        public IdForwardingHandler(IOptions<TContext> options, IIdForwarder<TContext> idForwarder)
        {
            _options = options;
            _idForwarder = idForwarder;
        }

        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            AttachCorrelationId(request.Headers);

            return base.Send(request, cancellationToken);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            AttachCorrelationId(request.Headers);

            return base.SendAsync(request, cancellationToken);
        }

        private void AttachCorrelationId(HttpRequestHeaders headers)
        {
            string correlationId = _idForwarder.GetForwardingId();

            headers.Add(_options.Key, correlationId);
        }
    }
}