using NvARdotNet.Native;

namespace NvARdotNet;

partial class Feature
{
    public sealed class FaceBoxDetection : BoxDetectionBase
    {
        public FaceBoxDetection(int maxBoundingBoxCount = DEFAULT_MAX_BOUNDING_BOX_COUNT)
            : base(FeatureIds.FaceBoxDetection, maxBoundingBoxCount)
        { }
    }
}
