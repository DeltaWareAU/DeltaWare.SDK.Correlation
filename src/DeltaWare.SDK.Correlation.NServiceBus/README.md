# Getting Started

Install Nuget Package ```DeltaWare.SDK.Correlation.NServiceBus```

### Configuration

>**NOTE:** Currently only supported when NServiceBus is running in AspNet and you've called `services.AddCorrelation` and/or `services.AddTracing`

```csharp

var builder = WebApplication.CreateBuilder();

builder.Host.UseNServiceBus(context => 
{
    var endpointConfiguration = new EndpointConfiguration("Samples.ASPNETCore.Sender");

    // To use Correlation ID
    endpointConfiguration.UseCorrelation();

    // To use Tracing ID
    endpointConfiguration.UseTracing();

    // The rest of my NServiceBus configuration
});

```