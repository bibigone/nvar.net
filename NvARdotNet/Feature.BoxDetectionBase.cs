using NvARdotNet.Native;

namespace NvARdotNet;

partial class Feature
{
    public abstract class BoxDetectionBase : Feature
    {
        public const int DEFAULT_MAX_BOUNDING_BOX_COUNT = 25;

        private protected readonly NativeBuffer.Array<Rect> bboxesArrayBuffer;
        private protected readonly NativeBuffer.Struct<BBoxes> bboxesStructBuffer;
        private protected readonly NativeBuffer.Array<float> bboxesConfidenceArrayBuffer;

        private protected BoxDetectionBase(FeatureId featureId, int maxBoundingBoxCount = DEFAULT_MAX_BOUNDING_BOX_COUNT)
            : base(featureId)
        {
            bboxesArrayBuffer = ToBeDisposed(new NativeBuffer.Array<Rect>(maxBoundingBoxCount));

            var bboxes = new BBoxes
            {
                BoxCount = 0,
                MaxBoxCount = maxBoundingBoxCount,
                BoxesIntPtr = bboxesArrayBuffer.Pointer,
            };
            bboxesStructBuffer = ToBeDisposed(new NativeBuffer.Struct<BBoxes>(bboxes));

            bboxesConfidenceArrayBuffer = ToBeDisposed(new NativeBuffer.Array<float>(maxBoundingBoxCount));
        }

        public int MaxBoundingBoxCount => bboxesStructBuffer.Value.MaxBoxCount;

        #region Output

        public NativeArrayView<Rect> OutputBoundingBoxes => new(bboxesArrayBuffer, bboxesStructBuffer.Value.BoxCount);

        public NativeArrayView<float> OutputBoundingBoxConfidences => new(bboxesConfidenceArrayBuffer, bboxesStructBuffer.Value.BoxCount);

        protected override void AfterLoad()
        {
            base.AfterLoad();
            SetParameterObject(ParameterNames.Output.BoundingBoxes, bboxesStructBuffer);
            SetParameterF32Array(ParameterNames.Output.BoundingBoxesConfidence, bboxesConfidenceArrayBuffer);
        }

        #endregion
    }
}
