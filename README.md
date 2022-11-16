# Getting Started

Install Nuget Package ```DeltaWare.SDK.Correlation.AspNetCore```

[**NServiceBus Documentation**](https://github.com/DeltaWareAU/DeltaWare.SDK.Correlation/tree/main/src/DeltaWare.SDK.Correlation.NServiceBus/README.md)

## Correlation ID

The Correlation ID will remaing static for the lifetime of a process. This enables correlation of a process over multiple services. 

>**Example:** *New ID* -> ServiceA -> *Same ID* -> ServiceB -> *Same ID* -> ServiceC

```csharp
public void ConfigureServices(IServiceCollection services)
{
	// Adds Required Sercies and Configuration
	services.AddCorrelation();
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
	// Enables Correlation Middleware
	app.UseCorrelation();
}
```

### Configuration

```csharp
services.AddCorrelation(o => 
{
	// The default value is x-correlation-id
	o.Key = "my-custom-key";
	o.AttachToResponse = true;
	o.IsRequired = true;
	o.AttachToLoggingScope = false;
	// The default value is correlation-id
	o.LoggingScopeKey = "my-custom-scope"
	o.UseIdProvider<MyCustomIdProvider>();
	o.UseIdForwarder<MyCustomIdForwarder>();
});
```

### Accessing Correlation

```csharp
public class MyClass
{
	private readonly IContextAccessor<CorrelationContext> _contextAccessor;

	public MyClass(IContextAccessor<CorrelationContext> contextAccessor)
	{
		_contextAccessor = contextAccessor
	}

	public void MyMethod()
	{
		string correlationId = _contextAccessor.Context.CorrelationId;
	}
}
```

## Trace ID

A unique Trace ID is generated every time a message transits from service to another, unlike the Correlation ID which remains static for the lifetime of the process. This is helpful for tracing how the current process moves between services. 

**Example:** ServiceA -> *New ID* -> ServiceB -> *New ID* -> ServiceC

```csharp
public void ConfigureServices(IServiceCollection services)
{
	// Adds Required Sercies and Configuration
	services.AddTracing();
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
	// Enables Correlation Middleware
	app.UseTracing();
}
```

### Configuration

```csharp
services.AddTracing(o => 
{
	// The default value is x-tracing-id
	o.Key = "my-custom-key";
	o.AttachToResponse = true;
	o.IsRequired = true;
	o.AttachToLoggingScope = false;
	// The default value is tracing-id
	o.LoggingScopeKey = "my-custom-scope"
	o.UseIdProvider<MyCustomIdProvider>();
	o.UseIdForwarder<MyCustomIdForwarder>();
});
```

### Accessing Correlation

```csharp
public class MyClass
{
	private readonly IContextAccessor<TraceContext> _contextAccessor;

	public MyClass(IContextAccessor<TraceContext> contextAccessor)
	{
		_contextAccessor = contextAccessor
	}

	public void MyMethod()
	{
		string traceId = _contextAccessor.Context.TraceId;
	}
}
```

# Applying Attributes to Controller or Methods
It is also possible to change how a Controller or Method will handle an incoming request by using the following Attributes

## Attach ID to the response headers.

When these attributes are present the Correlation or Trace ID will be attached to the response headers.
* AttachCorrelationIdToResponseHeader
* AttachTraceIdToResponseHeaderAttribute

## Enforce an incoming request to have the ID header

When these attributes are present if an incoming request does not contain the Correlation or Trace ID headers it will respond with 400 (BadRequest). This can be enabled API wide by settings the `IsRequired` property to true when adding Correlation or Trace ID.
* CorrelationIdHeaderRequiredAttribute
* TraceIdHeaderRequiredAttribute

## Make an incoming requests ID header optional.

When these attributes are present the `IsRequiredAttribute` or `IsRequired` option are no longer enforced.
* CorrelationIdHeaderNotRequiredAttribute
* TraceIdHeaderNotRequiredAttribute

### Example

Below is an example of using the IsRequied and IsNotRequired attributes for the Correlation ID.

```csharp
// All calls to this controller must have the Correlation ID header present in the incoming request.
[ApiController]
[CorrelationIdHeaderRequired]
public FooController: ControllerBase
{
	// When this endpoint is called, the Correlation ID does not need to be present in the headers of the incoming request as we've used the Not Required attribute.
	[HttpGet("{fooId}")]
	[CorrelationIdHeaderNotRequired]
	public IActionResult Get(string fooId)
	{
	}

	// When this endpoint is called, the Correlation ID must be present in the headers of the incoming request as the controller has the Is Required attribute.
	// It will also attach the Correlation ID to the headers of the Http Response.
	[HttpPost]
	[AttachCorrelationIdToResponseHeader]
	public IActionResult Get(FooDto newFoo)
	{
	}
}
```

>**NOTE:** This can also be done with the Trace ID.

## Releases

|Package|Downloads|NuGet|
|-|-|-|
|**DeltaWare.SDK.Correlation**|![](https://img.shields.io/nuget/dt/DeltaWare.SDK.Correlation?style=for-the-badge)|[![Nuget](https://img.shields.io/nuget/v/DeltaWare.SDK.Correlation.svg?style=for-the-badge)](https://www.nuget.org/packages/DeltaWare.SDK.Correlation/)|
|**DeltaWare.SDK.Correlation.AspNetCore**|![](https://img.shields.io/nuget/dt/DeltaWare.SDK.Correlation.AspNetCore?style=for-the-badge)|[![Nuget](https://img.shields.io/nuget/v/DeltaWare.SDK.Correlation.AspNetCore.svg?style=for-the-badge)](https://www.nuget.org/packages/DeltaWare.SDK.Correlation.AspNetCore/)|
|**DeltaWare.SDK.Correlation.NServiceBus**|![](https://img.shields.io/nuget/dt/DeltaWare.SDK.Correlation.NServiceBus?style=for-the-badge)|[![Nuget](https://img.shields.io/nuget/v/DeltaWare.SDK.Correlation.NServiceBus.svg?style=for-the-badge)](https://www.nuget.org/packages/DeltaWare.SDK.Correlation.NServiceBus/)|


# Contributors
 
Nuget Icon by Bernd Lakenbrink from [Noun Project](https://thenounproject.com/browse/icons/term/data-visualization/)