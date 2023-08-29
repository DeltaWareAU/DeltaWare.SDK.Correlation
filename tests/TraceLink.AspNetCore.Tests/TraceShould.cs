using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Forwarder;
using TraceLink.Abstractions.Options;
using TraceLink.Abstractions.Providers;
using TraceLink.AspNetCore.Context.Scopes;
using Xunit;

namespace TraceLink.AspNetCore.Tests
{
    public class TraceShould
    {
        [Fact]
        public async Task ReturnBadRequest_WhenIsRequired_AndNoHeaderIsProvided()
        {
            var builder = new WebHostBuilder()
                .Configure(app => app.UseTracing())
                .ConfigureServices(sc => sc.AddTracing(options => options.IsRequired = true));

            using var server = new TestServer(builder);

            var response = await server.CreateClient().GetAsync("");

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task NotReturnBadRequest_WhenIsRequired_AndHeaderIsProvided()
        {
            var builder = new WebHostBuilder()
                .Configure(app => app.UseTracing())
                .ConfigureServices(sc => sc.AddTracing(options => options.IsRequired = true));

            using var server = new TestServer(builder);

            var traceIdProvider = server.Services.GetRequiredService<IIdProvider<TraceContext>>();
            var options = server.Services.GetRequiredService<ITracingOptions<TraceContext>>();

            using var client = server.CreateClient();

            client.DefaultRequestHeaders.Add(options.Key, traceIdProvider.GenerateId());

            var response = await client.GetAsync("");

            response.Headers.TryGetValues(options.Key, out _).ShouldBeFalse();
            response.StatusCode.ShouldNotBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task NotReturnBadRequest_WhenIsNotRequired_AndNoHeaderIsProvided()
        {
            var builder = new WebHostBuilder()
                .Configure(app => app.UseTracing())
                .ConfigureServices(sc => sc.AddTracing(options => options.IsRequired = false));

            using var server = new TestServer(builder);

            var response = await server.CreateClient().GetAsync("");

            response.StatusCode.ShouldNotBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ReturnId_WhenAttachToResponseIsEnabled()
        {
            var builder = new WebHostBuilder()
                .Configure(app => app.UseTracing())
                .ConfigureServices(sc => sc.AddTracing(options => options.AttachToResponse = true));

            using var server = new TestServer(builder);

            var traceId = server.Services.GetRequiredService<IIdProvider<TraceContext>>().GenerateId();
            var options = server.Services.GetRequiredService<ITracingOptions<TraceContext>>();

            var request = new HttpRequestMessage();
            request.Headers.Add(options.Key, traceId);

            var response = await server.CreateClient().SendAsync(request);

            response.Headers.TryGetValues(options.Key, out var headerValues).ShouldBeTrue();

            headerValues!.Single().ShouldBe(traceId);

            response.StatusCode.ShouldNotBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UseSpecifiedKey()
        {
            string headerValue = "x-testing-header-changed";

            var builder = new WebHostBuilder()
                .Configure(app => app.UseTracing())
                .ConfigureServices(sc => sc.AddTracing(options =>
                {
                    options.Key = headerValue;
                    options.AttachToResponse = true;
                }));

            using var server = new TestServer(builder);

            var traceId = server.Services.GetRequiredService<IIdProvider<TraceContext>>().GenerateId();
            var options = server.Services.GetRequiredService<ITracingOptions<TraceContext>>();

            var request = new HttpRequestMessage();
            request.Headers.Add(options.Key, traceId);

            var response = await server.CreateClient().SendAsync(request);

            options.Key.ShouldBe(headerValue);

            response.Headers.TryGetValues(options.Key, out var headerValues).ShouldBeTrue();

            headerValues!.Single().ShouldBe(traceId);

            response.StatusCode.ShouldNotBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void GetForwardingId()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddTracing();

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            string headerKey = serviceProvider.GetRequiredService<ITracingOptions<TraceContext>>().Key;
            string traceId = serviceProvider.GetRequiredService<IIdProvider<TraceContext>>().GenerateId();
            IHttpContextAccessor httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();

            httpContextAccessor.HttpContext = new DefaultHttpContext();
            httpContextAccessor.HttpContext.Request.Headers.Add(headerKey, traceId);

            serviceProvider
                .GetRequiredService<IAspNetTracingScope<TraceContext>>()
                .TrySetId(true);

            serviceProvider
                .GetRequiredService<IIdForwarder<TraceContext>>()
                .GetForwardingId()
                .ShouldNotBe(traceId);
        }
    }
}
