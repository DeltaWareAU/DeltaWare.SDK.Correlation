using Microsoft.AspNetCore.Http;
using System;
using TraceLink.AspNetCore.Enum;

namespace TraceLink.AspNetCore.Validation
{
    public interface IHeaderValidationManager
    {
        HeaderValidationRequirements GetHeaderValidationRequirements<TRequired, TNotRequired>(HttpContext httpContext) where TRequired : Attribute where TNotRequired : Attribute;
    }
}
