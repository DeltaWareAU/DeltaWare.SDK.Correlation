using System.Net.Http.Headers;
using DeltaWare.SDK.Correlation.Context.Accessors;
using DeltaWare.SDK.Correlation.Options;

namespace DeltaWare.SDK.Correlation.AspNetCore.Handler
{
    internal sealed class CorrelationIdHandler : DelegatingHandler
    {
        private readonly ICorrelationOptions _options;

        private readonly ICorrelationContextAccessor _contextAccessor;

        public CorrelationIdHandler(ICorrelationOptions options, ICorrelationContextAccessor contextAccessor)
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