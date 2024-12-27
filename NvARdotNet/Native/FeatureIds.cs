using System;

namespace NvARdotNet.Native;

internal static class FeatureIds
{
    // #define NvAR_Feature_FaceBoxDetection     "FaceBoxDetection"
    public static readonly FeatureId FaceBoxDetection = new("FaceBoxDetection");

    // #define NvAR_Feature_FaceDetection        "FaceDetection" // deprecated in favor of FaceBox
    [Obsolete("deprecated in favor of FaceBox")]
    public static readonly FeatureId FaceDetection = new("FaceDetection");

    // #define NvAR_Feature_LandmarkDetection    "LandmarkDetection"
    public static readonly FeatureId LandmarkDetection = new("LandmarkDetection");

    // #define NvAR_Feature_Face3DReconstruction "Face3DReconstruction"
    public static readonly FeatureId Face3DReconstruction = new("Face3DReconstruction");

    // #define NvAR_Feature_BodyDetection        "BodyDetection"
    public static readonly FeatureId BodyDetection = new("BodyDetection");

    // #define NvAR_Feature_BodyPoseEstimation   "BodyPoseEstimation"
    public static readonly FeatureId BodyPoseEstimation = new("BodyPoseEstimation");

    // #define NvAR_Feature_GazeRedirection      "GazeRedirection"
    public static readonly FeatureId GazeRedirection = new("GazeRedirection");

    // #define NvAR_Feature_FaceExpressions      "FaceExpressions"
    public static readonly FeatureId FaceExpressions = new("FaceExpressions");
}
