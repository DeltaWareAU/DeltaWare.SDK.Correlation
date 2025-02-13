using Microsoft.AspNetCore.Http;
using System;
using TraceLink.Abstractions.Context;
using TraceLink.AspNetCore.Enum;

namespace TraceLink.AspNetCore.Validation
{
    public interface IHeaderValidationManager
    {
        HeaderValidationRequirements GetHeaderValidationRequirements<TTracingContext>(HttpContext httpContext) where TTracingContext : struct, ITracingContext;
    }
}
