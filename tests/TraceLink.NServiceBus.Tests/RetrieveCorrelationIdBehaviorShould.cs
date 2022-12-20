using Microsoft.Extensions.Logging;
using Moq;
using NServiceBus.Testing;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Options;
using TraceLink.Abstractions.Providers;
using TraceLink.NServiceBus.Behaviors;
using TraceLink.NServiceBus.Tests.Mocking;
using Xunit;

namespace TraceLink.NServiceBus.Tests
{
    public class RetrieveCorrelationIdBehaviorShould
    {
        [Fact]
        public async Task Retrieve_CorrelationId()
        {
            string correlationId = Guid.NewGuid().ToString();
            string key = "my-test-key";

            MockContextScope<CorrelationContext> contextScope = new MockContextScope<CorrelationContext>();

            Mock<IIdProvider<CorrelationContext>> mockIdProvider = new Mock<IIdProvider<CorrelationContext>>();
            Mock<IOptions<CorrelationContext>> mockOptions = new Mock<IOptions<CorrelationContext>>();

            mockOptions.Setup(p => p.Key).Returns(key);
            mockOptions.Setup(p => p.IsRequired).Returns(true);

            RetrieveContextIdBehavior<CorrelationContext> behavior = new RetrieveCorrelationIdBehavior(contextScope, mockIdProvider.Object, mockOptions.Object);

            TestableIncomingPhysicalMessageContext context = new TestableIncomingPhysicalMessageContext
            {
                MessageHeaders = new Dictionary<string, string>
                {
                    { key, correlationId }
                }
            };

            await behavior.Invoke(context, () => Task.CompletedTask);

            contextScope.Context.CorrelationId.ShouldBe(correlationId);

            mockIdProvider.Verify(m => m.GenerateId(), Times.Never);
        }

        [Fact]
        public async Task Generate_CorrelationId()
        {
            string correlationId = Guid.NewGuid().ToString();
            string key = "my-test-key";

            MockContextScope<CorrelationContext> contextScope = new MockContextScope<CorrelationContext>();

            Mock<IIdProvider<CorrelationContext>> mockIdProvider = new Mock<IIdProvider<CorrelationContext>>();
            Mock<IOptions<CorrelationContext>> mockOptions = new Mock<IOptions<CorrelationContext>>();

            mockIdProvider.Setup(m => m.GenerateId()).Returns(correlationId);

            mockOptions.Setup(p => p.Key).Returns(key);
            mockOptions.Setup(p => p.IsRequired).Returns(false);

            RetrieveContextIdBehavior<CorrelationContext> behavior = new RetrieveCorrelationIdBehavior(contextScope, mockIdProvider.Object, mockOptions.Object);

            TestableIncomingPhysicalMessageContext context = new TestableIncomingPhysicalMessageContext();

            await behavior.Invoke(context, () => Task.CompletedTask);

            contextScope.Context.CorrelationId.ShouldBe(correlationId);

            mockIdProvider.Verify(m => m.GenerateId(), Times.Once);
        }

        [Fact]
        public async Task Begin_LoggingScope()
        {
            string correlationId = Guid.NewGuid().ToString();
            string key = "my-test-key";
            string loggingScopeKey = "my-test-logging-scope-key";

            MockContextScope<CorrelationContext> contextScope = new MockContextScope<CorrelationContext>();

            Mock<IIdProvider<CorrelationContext>> mockIdProvider = new Mock<IIdProvider<CorrelationContext>>();
            Mock<IOptions<CorrelationContext>> mockOptions = new Mock<IOptions<CorrelationContext>>();
            Mock<ILogger<CorrelationContext>> mockLogger = new Mock<ILogger<CorrelationContext>>();

            mockOptions.Setup(p => p.Key).Returns(key);
            mockOptions.Setup(p => p.IsRequired).Returns(true);
            mockOptions.Setup(p => p.AttachToLoggingScope).Returns(true);
            mockOptions.Setup(p => p.LoggingScopeKey).Returns(loggingScopeKey);

            mockLogger.Setup(m => m.BeginScope(It.IsAny<Dictionary<string, string>>())).Returns(new MockDisposable());


            RetrieveContextIdBehavior<CorrelationContext> behavior = new RetrieveCorrelationIdBehavior(contextScope, mockIdProvider.Object, mockOptions.Object, mockLogger.Object);

            TestableIncomingPhysicalMessageContext context = new TestableIncomingPhysicalMessageContext
            {
                MessageHeaders = new Dictionary<string, string>
                {
                    { key, correlationId }
                }
            };

            await behavior.Invoke(context, () => Task.CompletedTask);

            contextScope.Context.CorrelationId.ShouldBe(correlationId);

            mockIdProvider.Verify(m => m.GenerateId(), Times.Never);
            mockLogger.Verify(m => m.BeginScope(It.IsAny<Dictionary<string, string>>()), Times.Once);
            mockLogger.Verify(m => m.BeginScope(It.Is<Dictionary<string, string>>(v => v.ContainsKey(loggingScopeKey) && v[loggingScopeKey] == correlationId)), Times.Once);
        }
    }
}
