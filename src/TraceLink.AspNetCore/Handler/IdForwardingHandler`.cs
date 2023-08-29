using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Forwarder;
using TraceLink.Abstractions.Options;

namespace TraceLink.AspNetCore.Handler
{
    internal sealed class IdForwardingHandler<TTracingContext> : DelegatingHandler where TTracingContext : ITracingContext
    {
        private readonly ITracingOptions _options;

        private readonly IIdForwarder _idForwarder;

        public IdForwardingHandler(ITracingOptions<TTracingContext> options, IIdForwarder<TTracingContext> idForwarder)
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