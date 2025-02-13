using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Options;
using TraceLink.Abstractions.Outgoing;

namespace TraceLink.AspNetCore.Handler
{
    internal sealed class OutboundTracingIdHandler<TTracingContext> : DelegatingHandler where TTracingContext : struct, ITracingContext
    {
        private readonly IOutgoingTracingIdProvider<TTracingContext> _idProvider;
        private readonly ITracingOptions<TTracingContext> _options;

        public OutboundTracingIdHandler(IOutgoingTracingIdProvider<TTracingContext> idProvider, ITracingOptions<TTracingContext> options)
        {
            _idProvider = idProvider;
            _options = options;
        }

        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            AttachTracingId(request.Headers);

            return base.Send(request, cancellationToken);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            AttachTracingId(request.Headers);

            return base.SendAsync(request, cancellationToken);
        }

        private void AttachTracingId(HttpRequestHeaders headers)
        {
            if (headers.Contains(_options.Key))
            {
                return;
            }

            headers.Add(_options.Key, _idProvider.GetOutboundTracingId().ToString());
        }
    }
}
