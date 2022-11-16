using DeltaWare.SDK.Correlation.Context;
using DeltaWare.SDK.Correlation.Forwarder;
using DeltaWare.SDK.Correlation.NServiceBus.Behaviors;
using DeltaWare.SDK.Correlation.Options;
using Moq;
using NServiceBus.Testing;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DeltaWare.SDK.Correlation.NServiceBus.Tests
{
    public class AttachCorrelationIdBehaviorShould
    {
        [Fact]
        public async Task Add_CorrelationId_ToHeader()
        {
            string correlationId = Guid.NewGuid().ToString();
            string key = "my-test-key";

            Mock<IIdForwarder<CorrelationContext>> mockIdProvider = new Mock<IIdForwarder<CorrelationContext>>();

            mockIdProvider
                .Setup(m => m.GetForwardingId())
                .Returns(correlationId);

            Mock<IOptions<CorrelationContext>> mockOptions = new Mock<IOptions<CorrelationContext>>();

            mockOptions
                .Setup(p => p.Key)
                .Returns(key);

            AttachContextIdBehavior behavior = new AttachCorrelationIdBehavior(mockIdProvider.Object, mockOptions.Object);

            TestableOutgoingPhysicalMessageContext context = new TestableOutgoingPhysicalMessageContext();

            await behavior.Invoke(context, () => Task.CompletedTask);

            context.Headers.Keys.ShouldContain(key);
            context.Headers[key].ShouldBe(correlationId);

            mockIdProvider.Verify(m => m.GetForwardingId(), Times.Once);
        }

        [Fact]
        public async Task Not_Add_CorrelationId_ToHeader()
        {
            string existingCorrelationId = Guid.NewGuid().ToString();
            string correlation = Guid.NewGuid().ToString();
            string key = "my-test-key";

            Mock<IIdForwarder<CorrelationContext>> mockIdForwarder = new Mock<IIdForwarder<CorrelationContext>>();

            mockIdForwarder.Setup(m => m.GetForwardingId()).Returns(correlation);

            Mock<IOptions<CorrelationContext>> mockOptions = new Mock<IOptions<CorrelationContext>>();

            mockOptions.Setup(p => p.Key).Returns(key);

            AttachContextIdBehavior behavior = new AttachCorrelationIdBehavior(mockIdForwarder.Object, mockOptions.Object);

            TestableOutgoingPhysicalMessageContext context = new TestableOutgoingPhysicalMessageContext
            {
                Headers = new Dictionary<string, string>
                {
                    { key, existingCorrelationId }
                }
            };

            await behavior.Invoke(context, () => Task.CompletedTask);

            context.Headers.Keys.ShouldContain(key);
            context.Headers[key].ShouldNotBe(correlation);

            mockIdForwarder.Verify(m => m.GetForwardingId(), Times.Never);
        }
    }
}
