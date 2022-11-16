using DeltaWare.SDK.Correlation.Forwarder;
using DeltaWare.SDK.Correlation.NServiceBus.Behaviors;
using DeltaWare.SDK.Correlation.Options;
using Moq;
using NServiceBus.Testing;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeltaWare.SDK.Correlation.Context;
using Xunit;

namespace DeltaWare.SDK.Correlation.NServiceBus.Tests
{
    public class AttachTraceIdBehaviorShould
    {
        [Fact]
        public async Task Add_TraceId_ToHeader()
        {
            string traceId = Guid.NewGuid().ToString();
            string key = "my-test-key";

            Mock<IIdForwarder<TraceContext>> mockIdProvider = new Mock<IIdForwarder<TraceContext>>();

            mockIdProvider
                .Setup(m => m.GetForwardingId())
                .Returns(traceId);

            Mock<IOptions<TraceContext>> mockOptions = new Mock<IOptions<TraceContext>>();

            mockOptions
                .Setup(p => p.Key)
                .Returns(key);

            AttachContextIdBehavior behavior = new AttachTraceIdBehavior(mockIdProvider.Object, mockOptions.Object);

            TestableOutgoingPhysicalMessageContext context = new TestableOutgoingPhysicalMessageContext();

            await behavior.Invoke(context, () => Task.CompletedTask);

            context.Headers.Keys.ShouldContain(key);
            context.Headers[key].ShouldBe(traceId);

            mockIdProvider.Verify(m => m.GetForwardingId(), Times.Once);
        }
        
        [Fact]
        public async Task Not_Add_TraceId_ToHeader()
        {
            string existingCorrelationId = Guid.NewGuid().ToString();
            string traceId = Guid.NewGuid().ToString();
            string key = "my-test-key";

            Mock<IIdForwarder<TraceContext>> mockIdForwarder = new Mock<IIdForwarder<TraceContext>>();

            mockIdForwarder.Setup(m => m.GetForwardingId()).Returns(traceId);

            Mock<IOptions<TraceContext>> mockOptions = new Mock<IOptions<TraceContext>>();

            mockOptions.Setup(p => p.Key).Returns(key);

            AttachContextIdBehavior behavior = new AttachTraceIdBehavior(mockIdForwarder.Object, mockOptions.Object);

            TestableOutgoingPhysicalMessageContext context = new TestableOutgoingPhysicalMessageContext
            {
                Headers = new Dictionary<string, string>
                {
                    { key, existingCorrelationId }
                }
            };

            await behavior.Invoke(context, () => Task.CompletedTask);

            context.Headers.Keys.ShouldContain(key);
            context.Headers[key].ShouldNotBe(traceId);

            mockIdForwarder.Verify(m => m.GetForwardingId(), Times.Never);
        }
    }
}
