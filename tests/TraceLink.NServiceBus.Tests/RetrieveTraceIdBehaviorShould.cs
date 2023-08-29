using Microsoft.Extensions.Logging;
using Moq;
using NServiceBus.Testing;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Options;
using TraceLink.NServiceBus.Behaviors;
using TraceLink.NServiceBus.Tests.Mocking;
using Xunit;

namespace TraceLink.NServiceBus.Tests
{
    public class RetrieveTraceIdBehaviorShould
    {
        [Fact]
        public async Task Retrieve_TraceId()
        {
            string traceId = Guid.NewGuid().ToString();
            string key = "my-test-key";

            MockTracingTracingScope<TraceContext> tracingTracingScope = new MockTracingTracingScope<TraceContext>();

            Mock<ITracingOptions<TraceContext>> mockOptions = new Mock<ITracingOptions<TraceContext>>();

            mockOptions.Setup(p => p.Key).Returns(key);
            mockOptions.Setup(p => p.IsRequired).Returns(true);

            RetrieveContextIdBehavior<TraceContext> behavior = new RetrieveTraceIdBehavior(tracingTracingScope, mockOptions.Object);

            TestableIncomingPhysicalMessageContext context = new TestableIncomingPhysicalMessageContext
            {
                MessageHeaders = new Dictionary<string, string>
                {
                    { key, traceId }
                }
            };

            await behavior.Invoke(context, () => Task.CompletedTask);

            tracingTracingScope.Context.Id.ShouldBe(traceId);
        }

        [Fact]
        public async Task Not_Retrieve_Or_Generate_TraceId()
        {
            string key = "my-test-key";

            MockTracingTracingScope<TraceContext> tracingTracingScope = new MockTracingTracingScope<TraceContext>();

            Mock<ITracingOptions<TraceContext>> mockOptions = new Mock<ITracingOptions<TraceContext>>();

            mockOptions.Setup(p => p.Key).Returns(key);
            mockOptions.Setup(p => p.IsRequired).Returns(false);

            RetrieveContextIdBehavior<TraceContext> behavior = new RetrieveTraceIdBehavior(tracingTracingScope, mockOptions.Object);

            TestableIncomingPhysicalMessageContext context = new TestableIncomingPhysicalMessageContext();

            await behavior.Invoke(context, () => Task.CompletedTask);

            tracingTracingScope.Context.ShouldBeNull();
        }

        [Fact]
        public async Task Begin_LoggingScope()
        {
            string traceId = Guid.NewGuid().ToString();
            string key = "my-test-key";
            string loggingScopeKey = "my-test-logging-scope-key";

            MockTracingTracingScope<TraceContext> tracingTracingScope = new MockTracingTracingScope<TraceContext>();

            Mock<ITracingOptions<TraceContext>> mockOptions = new Mock<ITracingOptions<TraceContext>>();
            Mock<ILogger<TraceContext>> mockLogger = new Mock<ILogger<TraceContext>>();

            mockOptions.Setup(p => p.Key).Returns(key);
            mockOptions.Setup(p => p.IsRequired).Returns(true);
            mockOptions.Setup(p => p.AttachToLoggingScope).Returns(true);
            mockOptions.Setup(p => p.LoggingScopeKey).Returns(loggingScopeKey);

            mockLogger.Setup(m => m.BeginScope(It.IsAny<Dictionary<string, string>>())).Returns(new MockDisposable());

            RetrieveContextIdBehavior<TraceContext> behavior = new RetrieveTraceIdBehavior(tracingTracingScope, mockOptions.Object, mockLogger.Object);

            TestableIncomingPhysicalMessageContext context = new TestableIncomingPhysicalMessageContext
            {
                MessageHeaders = new Dictionary<string, string>
                {
                    { key, traceId }
                }
            };

            await behavior.Invoke(context, () => Task.CompletedTask);

            tracingTracingScope.Context.Id.ShouldBe(traceId);

            mockLogger.Verify(m => m.BeginScope(It.IsAny<Dictionary<string, string>>()), Times.Once);
            mockLogger.Verify(m => m.BeginScope(It.Is<Dictionary<string, string>>(v => v.ContainsKey(loggingScopeKey) && v[loggingScopeKey] == traceId)), Times.Once);
        }
    }
}
