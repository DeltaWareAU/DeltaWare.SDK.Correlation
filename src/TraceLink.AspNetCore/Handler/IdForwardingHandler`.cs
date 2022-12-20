using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using TraceLink.Abstractions.Forwarder;
using TraceLink.Abstractions.Options;

namespace TraceLink.AspNetCore.Handler
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

#if NETCOREAPP
        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            AttachCorrelationId(request.Headers);

            return base.Send(request, cancellationToken);
        }
#endif

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            AttachCorrelationId(request.Headers);

            return base.SendAsync(request, cancellationToken);
        }

        private void AttachCorrelationId(HttpRequestHeaders headers)
        {
            if (headers.Contains(_options.Key))
            {
                return;
            }

            string correlationId = _idForwarder.GetForwardingId();

            headers.Add(_options.Key, correlationId);
        }
    }
}