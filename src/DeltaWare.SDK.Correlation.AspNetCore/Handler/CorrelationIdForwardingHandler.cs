using DeltaWare.SDK.Correlation.Context.Accessors;
using DeltaWare.SDK.Correlation.Options;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace DeltaWare.SDK.Correlation.AspNetCore.Handler
{
    internal sealed class CorrelationIdForwardingHandler : DelegatingHandler
    {
        private readonly ICorrelationOptions _options;

        private readonly ICorrelationContextAccessor _contextAccessor;

        public CorrelationIdForwardingHandler(ICorrelationOptions options, ICorrelationContextAccessor contextAccessor)
        {
            _options = options;
            _contextAccessor = contextAccessor;
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
            string? correlationId = _contextAccessor.Context.CorrelationId;

            headers.Add(_options.Header, correlationId);
        }
    }
}