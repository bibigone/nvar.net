using NvARdotNet.Native;
using System.Runtime.InteropServices;

namespace NvARdotNet;

partial class Feature
{
    public abstract class BodyPoseEstimationBase : Feature
    {
        private protected readonly int batchSize;
        private protected readonly NativeBuffer.Array<Rect> inputBoundingBoxesArrayBuffer;
        private protected readonly NativeBuffer.Struct<BBoxes> inputBBoxesStructBuffer;

        private protected BodyPoseEstimationBase(int batchSize)
            : base(FeatureIds.BodyPoseEstimation)
        {
            this.batchSize = batchSize;

            // Key point count
            var status = PoseApi.GetU32(Handle, ParameterNames.Config.NumKeyPoints, out var keyPointCount);
            NvarException.ThrowIfNotSuccess(status, PoseApi.PREFIX + nameof(PoseApi.GetU32));
            KeyPointCount = (int)keyPointCount;

            // For input bounding boxes
            inputBoundingBoxesArrayBuffer = ToBeDisposed(new NativeBuffer.Array<Rect>(batchSize));
            var bboxes = new BBoxes
            {
                MaxBoxCount = batchSize,
                BoxCount = 0,
                BoxesIntPtr = inputBoundingBoxesArrayBuffer.Pointer,
            };
            inputBBoxesStructBuffer = ToBeDisposed(new NativeBuffer.Struct<BBoxes>(bboxes));

            // For output
            var maxOutputCount = KeyPointCount * batchSize;
            outputKeyPointsArrayBuffer = ToBeDisposed(new NativeBuffer.Array<Point2f>(maxOutputCount));
            outputKeyPoints3DArrayBuffer = ToBeDisposed(new NativeBuffer.Array<Point3f>(maxOutputCount));
            outputJointAnglesArrayBuffer = ToBeDisposed(new NativeBuffer.Array<Vector4f>(maxOutputCount));
            outputConfidencesArrayBuffer = ToBeDisposed(new NativeBuffer.Array<float>(maxOutputCount));
        }

        /// <summary>Specifies the number of keypoints available, which is currently 34.</summary>
        public int KeyPointCount { get; }

        /// <summary>
        /// Array, which contains the reference pose for each of the 34 keypoints.
        /// Specifies the Reference Pose used to compute the joint angles.
        /// </summary>
        public NativeArrayView<Point3f> ReferencePose
        {
            get
            {
                var status = PoseApi.GetObject(Handle, ParameterNames.Config.ReferencePose, out var ptr, (ulong)Marshal.SizeOf<Point3f>());
                NvarException.ThrowIfNotSuccess(status, PoseApi.PREFIX + nameof(PoseApi.GetObject));
                return new(new NativeBuffer.Array<Point3f>(ptr, KeyPointCount, ownsBuffer: false));
            }
        }

        #region Configuration Parameters

        /// <summary>
        /// Selects the High Performance mode or High Quality mode.
        /// Set by the user.
        /// Default value is <see cref="ProcessingMode.HighPerformance"/>.
        /// </summary>
        public ProcessingMode Mode
        {
            get => GetConfigValue(mode);
            set => SetConfigValue(ref mode, value);
        }
        private ProcessingMode? mode = ProcessingMode.HighPerformance;

        /// <summary>
        /// Flag to use CUDA Graphs for optimization.
        /// Set by the user.
        /// Default value is <see langword="true"/>.
        /// </summary>
        public bool UseCudaGraph
        {
            get => GetConfigValue(useCudaGraph);
            set => SetConfigValue(ref useCudaGraph, value);
        }
        private bool? useCudaGraph = true;

        protected override void BeforeLoad()
        {
            base.BeforeLoad();

            var status = PoseApi.SetU32(Handle, ParameterNames.Config.BatchSize, (uint)batchSize);
            NvarException.ThrowIfNotSuccess(status, PoseApi.PREFIX + nameof(PoseApi.SetU32));

            status = PoseApi.SetU32(Handle, ParameterNames.Config.Mode, (uint)(int)Mode);
            NvarException.ThrowIfNotSuccess(status, PoseApi.PREFIX + nameof(PoseApi.SetU32));

            status = PoseApi.SetU32(Handle, ParameterNames.Config.UseCudaGraph, UseCudaGraph ? uint.MaxValue :uint.MinValue);
            NvarException.ThrowIfNotSuccess(status, PoseApi.PREFIX + nameof(PoseApi.SetU32));
        }

        #endregion

        #region Input Parameters

        /// <summary>
        /// Specifies the focal length of the camera to be used for 3D Body Pose.
        /// Set by the user.
        /// Default is 800.79041
        /// </summary>
        public float InputFocalLength
        {
            get
            {
                CheckLoaded();
                var status = PoseApi.GetF32(Handle, ParameterNames.Input.FocalLength, out var value);
                NvarException.ThrowIfNotSuccess(status, PoseApi.PREFIX + nameof(PoseApi.GetF32));
                return value;
            }

            set
            {
                CheckLoaded();
                var status = PoseApi.SetF32(Handle, ParameterNames.Input.FocalLength, value);
                NvarException.ThrowIfNotSuccess(status, PoseApi.PREFIX + nameof(PoseApi.SetF32));
            }
        }


        // TODO: Looks like that in the current version of NvAR, config parameter MaxTargetsTracked has no sense.
        //       This why it is not implemented here
        #if FALSE
        public abstract int MaxTargetsTracked { get; set; }
        #endif

        #endregion

        #region Output Parameters

        public abstract int OutputBodyCount { get; }

        public NativeArrayView<Point2f> OutputKeyPoints => new(outputKeyPointsArrayBuffer, OutputBodyCount * KeyPointCount);
        private protected NativeBuffer.Array<Point2f> outputKeyPointsArrayBuffer;

        public NativeArrayView<Point3f> OutputKeyPoints3D => new(outputKeyPoints3DArrayBuffer, OutputBodyCount * KeyPointCount);
        private protected NativeBuffer.Array<Point3f> outputKeyPoints3DArrayBuffer;

        public NativeArrayView<Vector4f> OutputJointAngles => new(outputJointAnglesArrayBuffer, OutputBodyCount * KeyPointCount);
        private protected NativeBuffer.Array<Vector4f> outputJointAnglesArrayBuffer;

        public NativeArrayView<float> OutputKeyPointConfidences => new(outputConfidencesArrayBuffer, OutputBodyCount * KeyPointCount);
        private protected NativeBuffer.Array<float> outputConfidencesArrayBuffer;

        protected override void AfterLoad()
        {
            base.AfterLoad();
            SetParameterObject(ParameterNames.Output.KeyPoints, outputKeyPointsArrayBuffer);
            SetParameterObject(ParameterNames.Output.KeyPoints3D, outputKeyPoints3DArrayBuffer);
            SetParameterObject(ParameterNames.Output.JointAngles, outputJointAnglesArrayBuffer);
            SetParameterF32Array(ParameterNames.Output.KeyPointsConfidence, outputConfidencesArrayBuffer);
        }

        #endregion
    }
}
