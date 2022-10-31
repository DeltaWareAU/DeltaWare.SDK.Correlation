using Microsoft.AspNetCore.Http.Features;

namespace DeltaWare.SDK.Correlation.AspNetCore.Extensions
{
    internal static class FeatureCollectionExtensions
    {
        public static bool HasFeature<T>(this IFeatureCollection features) where T : class
        {
            IEndpointFeature? endpointFeature = features.Get<IEndpointFeature>();

            if (endpointFeature == null)
            {
                return false;
            }

            T? metadata = endpointFeature.Endpoint.Metadata.GetMetadata<T>();

            return metadata != null;
        }
    }
}
