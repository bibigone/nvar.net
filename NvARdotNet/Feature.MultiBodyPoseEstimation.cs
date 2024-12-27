using NvARdotNet.Native;
using System;
using System.Collections.Generic;

namespace NvARdotNet;

partial class Feature
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Available only on Windows.</remarks>
    public sealed class MultiBodyPoseEstimation : BodyPoseEstimationBase
    {
        public const int DEFAULT_BATCH_SIZE = 8;

        private readonly NativeBuffer.Array<TrackingBoundingBox> outputTrackingBoundingBoxesArrayBuffer;
        private readonly NativeBuffer.Struct<TrackingBBoxes> outputTrackingBBoxesStructBuffer;

        public MultiBodyPoseEstimation(int batchSize = DEFAULT_BATCH_SIZE)
            : base(batchSize)
        {
            // For output
            outputTrackingBoundingBoxesArrayBuffer = ToBeDisposed(new NativeBuffer.Array<TrackingBoundingBox>(batchSize));
            var bboxes = new TrackingBBoxes
            {
                MaxBoxCount = batchSize,
                BoxCount = 0,
                BoxesIntPtr = outputTrackingBoundingBoxesArrayBuffer.Pointer,
            };
            outputTrackingBBoxesStructBuffer = ToBeDisposed(new NativeBuffer.Struct<TrackingBBoxes>(bboxes));
        }

        /// <summary>The number of inferences to be run at one time on the GPU.</summary>
        public int BatchSize => batchSize;

        #region Additional Configuration Parameters

        /// <summary>
        /// Specifies the period after which the multi-person tracker stops tracking the object in shadow mode.
        /// This value is measured in the number of frames.
        /// Set by the user, and the default value is 90.
        /// </summary>
        public int ShadowTrackingAge
        {
            get => GetConfigValue(shadowTrackingAge);
            set => SetConfigValue(ref shadowTrackingAge, value);
        }
        private int? shadowTrackingAge = 90;

        /// <summary>
        /// Specifies the period after which the multi-person tracker marks the object valid and assigns an ID for tracking.
        /// This value is measured in the number of frames.
        /// Set by the user, and the default value is 10.
        /// </summary>
        public int ProbationAge
        {
            get => GetConfigValue(probationAge);
            set => SetConfigValue(ref probationAge, value);
        }
        private int? probationAge = 10;

        protected override void BeforeLoad()
        {
            if (Temporal)
                throw new NotSupportedException($"Current version of NvAR toolkit does not support temporal filtering for multi-person tracking.");

            base.BeforeLoad();

            var status = PoseApi.SetU32(Handle, ParameterNames.Config.TrackPeople, uint.MaxValue);
            NvarException.ThrowIfNotSuccess(status, PoseApi.PREFIX + nameof(PoseApi.SetU32));

            status = PoseApi.SetU32(Handle, ParameterNames.Config.ShadowTrackingAge, (uint)ShadowTrackingAge);
            NvarException.ThrowIfNotSuccess(status, PoseApi.PREFIX + nameof(PoseApi.SetU32));

            status = PoseApi.SetU32(Handle, ParameterNames.Config.ProbationAge, (uint)ProbationAge);
            NvarException.ThrowIfNotSuccess(status, PoseApi.PREFIX + nameof(PoseApi.SetU32));
        }

        #endregion


        #region Additional Input Parameters

        /// <summary>
        /// Array that contains the number of bounding boxes that are equal to <see cref="BatchSize"/> on which to run 3D Body Pose detection.
        /// If not specified as an input property, body detection is automatically run on the input image.
        /// </summary>
        public Rect[]? InputBoundingBoxes
        {
            get => inputBoundingBoxesWasSpecified
                ? new NativeArrayView<Rect>(inputBoundingBoxesArrayBuffer, inputBBoxesStructBuffer.Value.BoxCount).ToArray()
                : null;

            set
            {
                CheckLoaded();
                CheckNotDisposed();

                if (value is null)
                {
                    if (inputBoundingBoxesWasSpecified)
                        throw new InvalidOperationException("Input bounding boxes have been already specified and cannot be cleared to null.");
                    return;
                }

                if (value.Length != batchSize)
                    throw new ArgumentOutOfRangeException(nameof(value), "Length of input bounding boxes array must be the same as batch size.");

                for (var i = 0; i < value.Length; i++)
                    inputBoundingBoxesArrayBuffer[i] = value[i];

                var bboxes = inputBBoxesStructBuffer.Value;
                bboxes.BoxCount = value.Length;
                inputBBoxesStructBuffer.Value = bboxes;

                if (!inputBoundingBoxesWasSpecified)
                {
                    try
                    {
                        var status = PoseApi.SetObject(Handle, ParameterNames.Input.BoundingBoxes,
                            inputBBoxesStructBuffer.Pointer, (ulong)NativeBuffer.Struct<BBoxes>.SizeOf);
                        NvarException.ThrowIfNotSuccess(status, PoseApi.PREFIX + nameof(PoseApi.SetObject));
                        inputBoundingBoxesWasSpecified = true;
                    }
                    catch
                    {
                        inputBoundingBoxesWasSpecified = false;
                        throw;
                    }
                }
            }
        }
        private bool inputBoundingBoxesWasSpecified;

        #endregion

        #region Additional Output Parameters

        public override int OutputBodyCount => outputTrackingBBoxesStructBuffer.Value.BoxCount;

        public NativeArrayView<TrackingBoundingBox> OutputTrackingBoundingBoxes
            => new(outputTrackingBoundingBoxesArrayBuffer, OutputBodyCount);

        protected override void AfterLoad()
        {
            base.AfterLoad();
            SetParameterObject(ParameterNames.Output.TrackingBoundingBoxes, outputTrackingBBoxesStructBuffer);
        }

        #endregion
    }
}
