using NvARdotNet.Native;

namespace NvARdotNet;

partial class Feature
{
    public sealed class BodyDetection : BoxDetectionBase
    {
        public BodyDetection(int maxBoundingBoxCount = DEFAULT_MAX_BOUNDING_BOX_COUNT)
            : base(FeatureIds.BodyDetection, maxBoundingBoxCount)
        {
            if(maxBoundingBoxCount > 1)
            {
                throw new System.ArgumentException("NvAR body detector always returns single box due to unknown reason"); 
            }
        }
    }
}
