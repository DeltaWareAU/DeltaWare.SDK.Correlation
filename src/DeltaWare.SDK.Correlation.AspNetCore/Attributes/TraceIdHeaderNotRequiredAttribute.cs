using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace DeltaWare.SDK.Correlation.AspNetCore.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class TraceIdHeaderNotRequiredAttribute : ActionFilterAttribute
    {
    }
}
