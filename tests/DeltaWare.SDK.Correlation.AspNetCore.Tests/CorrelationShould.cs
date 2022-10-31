using DeltaWare.SDK.Correlation.Options;
using DeltaWare.SDK.Correlation.Providers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DeltaWare.SDK.Correlation.AspNetCore.Tests
{
    public class CorrelationShould
    {
        [Fact]
        public async Task ReturnBadRequest_WhenIsRequired_AndNoHeaderIsProvided()
        {
            var builder = new WebHostBuilder()
                .Configure(app => app.UseCorrelation())
                .ConfigureServices(sc => sc.AddCorrelation(options => options.IsRequired = true));

            using var server = new TestServer(builder);

            var response = await server.CreateClient().GetAsync("");

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task NotReturnBadRequest_WhenIsRequired_AndHeaderIsProvided()
        {
            var builder = new WebHostBuilder()
                .Configure(app => app.UseCorrelation())
                .ConfigureServices(sc => sc.AddCorrelation(options => options.IsRequired = true));

            using var server = new TestServer(builder);

            var correlationIdProvider = server.Services.GetRequiredService<ICorrelationIdProvider>();
            var options = server.Services.GetRequiredService<ICorrelationOptions>();

            using var client = server.CreateClient();

            client.DefaultRequestHeaders.Add(options.Header, correlationIdProvider.GenerateId());

            var response = await client.GetAsync("");

            response.Headers.TryGetValues(options.Header, out _).ShouldBeFalse();
            response.StatusCode.ShouldNotBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task NotReturnBadRequest_WhenIsNotRequired_AndNoHeaderIsProvided()
        {
            var builder = new WebHostBuilder()
                .Configure(app => app.UseCorrelation())
                .ConfigureServices(sc => sc.AddCorrelation(options => options.IsRequired = false));

            using var server = new TestServer(builder);

            var response = await server.CreateClient().GetAsync("");

            response.StatusCode.ShouldNotBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ReturnId_WhenAttachToResponseIsEnabled()
        {
            var builder = new WebHostBuilder()
                .Configure(app => app.UseCorrelation())
                .ConfigureServices(sc => sc.AddCorrelation(options => options.AttachToResponse = true));

            using var server = new TestServer(builder);

            var correlationId = server.Services.GetRequiredService<ICorrelationIdProvider>().GenerateId();
            var options = server.Services.GetRequiredService<ICorrelationOptions>();

            var request = new HttpRequestMessage();
            request.Headers.Add(options.Header, correlationId);

            var response = await server.CreateClient().SendAsync(request);

            response.Headers.TryGetValues(options.Header, out var headerValues).ShouldBeTrue();

            headerValues!.Single().ShouldBe(correlationId);

            response.StatusCode.ShouldNotBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ChangeHeader()
        {
            string headerValue = "x-testing-header-changed";

            var builder = new WebHostBuilder()
                .Configure(app => app.UseCorrelation())
                .ConfigureServices(sc => sc.AddCorrelation(options =>
                {
                    options.Header = headerValue;
                    options.AttachToResponse = true;
                }));

            using var server = new TestServer(builder);

            var correlationId = server.Services.GetRequiredService<ICorrelationIdProvider>().GenerateId();
            var options = server.Services.GetRequiredService<ICorrelationOptions>();

            var request = new HttpRequestMessage();
            request.Headers.Add(options.Header, correlationId);

            var response = await server.CreateClient().SendAsync(request);

            options.Header.ShouldBe(headerValue);

            response.Headers.TryGetValues(options.Header, out var headerValues).ShouldBeTrue();

            headerValues!.Single().ShouldBe(correlationId);

            response.StatusCode.ShouldNotBe(HttpStatusCode.BadRequest);
        }
    }
}
