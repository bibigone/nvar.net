using NvARdotNet.Native;
using System;

namespace NvARdotNet;

partial class Feature
{
    public sealed class SingleBodyPoseEstimation : BodyPoseEstimationBase
    {
        private readonly NativeBuffer.Array<Rect> outputBoundingBoxesArrayBuffer;
        private readonly NativeBuffer.Struct<BBoxes> outputBBoxesStructBuffer;

        public SingleBodyPoseEstimation()
            : base(batchSize: 1)
        {
            // For output
            outputBoundingBoxesArrayBuffer = ToBeDisposed(new NativeBuffer.Array<Rect>(batchSize));
            var bboxes = new BBoxes
            {
                MaxBoxCount = batchSize,
                BoxCount = 0,
                BoxesIntPtr = outputBoundingBoxesArrayBuffer.Pointer,
            };
            outputBBoxesStructBuffer = ToBeDisposed(new NativeBuffer.Struct<BBoxes>(bboxes));
        }

        #region Additional Input Parameters

        /// <summary>
        /// Input bounding box.
        /// If not specified as an input property, body detection is automatically run on the input image.
        /// </summary>
        public Rect? InputBoundingBox
        {
            get => inputBBoxesStructBuffer.Value.BoxCount > 0
                ? inputBoundingBoxesArrayBuffer[0]
                : null;

            set
            {
                CheckLoaded();
                CheckNotDisposed();

                var oldValue = InputBoundingBox;

                if (value is null)
                {
                    if (oldValue is not null)
                        throw new InvalidOperationException("Input bounding box has been already specified and cannot be cleared to null.");
                    return;
                }

                inputBoundingBoxesArrayBuffer[0] = value.Value;

                if (oldValue is null)
                {
                    var bboxes = inputBBoxesStructBuffer.Value;
                    bboxes.BoxCount = 1;
                    inputBBoxesStructBuffer.Value = bboxes;

                    try
                    {
                        SetParameterObject(ParameterNames.Input.BoundingBoxes, inputBBoxesStructBuffer);
                    }
                    catch
                    {
                        bboxes.BoxCount = 0;
                        inputBBoxesStructBuffer.Value = bboxes;
                        throw;
                    }
                }
            }
        }

        #endregion

        #region Additional Output Parameters

        public override int OutputBodyCount => outputBBoxesStructBuffer.Value.BoxCount;

        public Rect? OutputBoundingBox
            => OutputBodyCount > 0
                ? outputBoundingBoxesArrayBuffer[0]
                : null;

        protected override void AfterLoad()
        {
            base.AfterLoad();
            SetParameterObject(ParameterNames.Output.BoundingBoxes, outputBBoxesStructBuffer);
        }

        #endregion

        protected override void BeforeRun()
        {
            base.BeforeRun();

            var bb = InputBoundingBox;

            var bboxes = outputBBoxesStructBuffer.Value;
            bboxes.BoxCount = bb.HasValue ? 1 : 0;
            outputBBoxesStructBuffer.Value = bboxes;

            if (bb.HasValue)
                outputBoundingBoxesArrayBuffer[0] = bb.Value;
        }

        protected override void AfterRun()
        {
            var bb = InputBoundingBox;

            if (bb.HasValue)
                outputBoundingBoxesArrayBuffer[0] = bb.Value;

            base.AfterRun();
        }
    }
}

