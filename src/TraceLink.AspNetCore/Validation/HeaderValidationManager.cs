using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Concurrent;
using System.Reflection;
using TraceLink.Abstractions.Context;
using TraceLink.AspNetCore.Attributes;
using TraceLink.AspNetCore.Enum;

namespace TraceLink.AspNetCore.Validation
{
    internal sealed class HeaderValidationManager : IHeaderValidationManager
    {
        private readonly ConcurrentDictionary<string, HeaderValidationRequirements> _requirementsCache = new();

        public HeaderValidationRequirements GetHeaderValidationRequirements<TTracingContext>(HttpContext httpContext) where TTracingContext : struct, ITracingContext
        {
            var endpoint = httpContext.GetEndpoint();

            if (endpoint == null)
            {
                return HeaderValidationRequirements.Default;
            }

            var actionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();

            if (actionDescriptor == null)
            {
                return HeaderValidationRequirements.Default;
            }

            string cacheKey = $"{actionDescriptor.ControllerTypeInfo.FullName}.{actionDescriptor.MethodInfo.Name}";

            if (_requirementsCache.TryGetValue(cacheKey, out var requirements))
            {
                return requirements;
            }

            requirements = DetermineHeaderValidationRequirements<TracingIdRequired<TTracingContext>, TracingIdNotRequired<TTracingContext>>(actionDescriptor);

            _requirementsCache.TryAdd(cacheKey, requirements);

            return requirements;
        }

        private HeaderValidationRequirements DetermineHeaderValidationRequirements<TRequired, TNotRequired>(ControllerActionDescriptor actionDescriptor) where TRequired : Attribute where TNotRequired : Attribute
        {
            if (actionDescriptor.MethodInfo.GetCustomAttribute<TRequired>() != null)
            {
                return HeaderValidationRequirements.Required;
            }

            if (actionDescriptor.MethodInfo.GetCustomAttribute<TNotRequired>() != null)
            {
                return HeaderValidationRequirements.Optional;
            }

            if (actionDescriptor.ControllerTypeInfo.GetCustomAttribute<TRequired>() != null)
            {
                return HeaderValidationRequirements.Required;
            }

            if (actionDescriptor.ControllerTypeInfo.GetCustomAttribute<TNotRequired>() != null)
            {
                return HeaderValidationRequirements.Optional;
            }

            return HeaderValidationRequirements.Default;
        }
    }
}
