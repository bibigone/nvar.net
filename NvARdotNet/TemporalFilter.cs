using System;

namespace NvARdotNet;

[Flags]
public enum TemporalFilter : uint
{
    // #define NVAR_TEMPORAL_FILTER_FACE_BOX                 (1U << 0)  // 0x001
    FaceBox = 1U << 0,

    // #define NVAR_TEMPORAL_FILTER_FACIAL_LANDMARKS         (1U << 1)  // 0x002
    FacialLandmarks = 1U << 1,

    // #define NVAR_TEMPORAL_FILTER_FACE_ROTATIONAL_POSE     (1U << 2)  // 0x004
    FaceRotationalPose = 1U << 2,

    // #define NVAR_TEMPORAL_FILTER_FACIAL_EXPRESSIONS       (1U << 4)  // 0x010
    FacialExpressions = 1U << 4,

    // #define NVAR_TEMPORAL_FILTER_FACIAL_GAZE              (1U << 5)  // 0x020
    FacialGaze = 1U << 5,

    // #define NVAR_TEMPORAL_FILTER_ENHANCE_EXPRESSIONS      (1U << 8)  // 0x100
    EnhanceExpressions = 1U << 8,
}
