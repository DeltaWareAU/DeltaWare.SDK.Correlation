using DeltaWare.SDK.Correlation.AspNetCore.Options.Builder;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DeltaWare.SDK.Correlation.AspNetCore.Tests
{
    public class CorrelationOptionsBuilderShould
    {
        [Fact]
        public void Test1()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            CorrelationOptionsBuilder optionsBuilder = new CorrelationOptionsBuilder(serviceCollection);

            optionsBuilder.Build();

            //mockServiceCollection.Verify(s => s.Add(ServiceDescriptor.Scoped(typeof(AspNetCorrelationContextScope), typeof(AspNetCorrelationContextScope))), Times.Exactly(1));
        }
    }
}