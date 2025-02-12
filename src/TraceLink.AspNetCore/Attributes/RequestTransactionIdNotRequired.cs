using System;

namespace TraceLink.AspNetCore.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class RequestTransactionIdNotRequired : Attribute
    {
    }
}
