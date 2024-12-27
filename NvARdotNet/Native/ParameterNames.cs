namespace NvARdotNet.Native;

/// <summary>The key values in the properties of a feature type identify the properties that can be used with each feature type.</summary>
internal static class ParameterNames
{
    /// <summary>Here are the configuration properties in the AR SDK.</summary>
    public static class Config
    {
        private const string PREFIX = "NvAR_Parameter_Config_";

        /// <summary>A description of the feature type.</summary>
        public static readonly ParameterName FeatureDescription = new(PREFIX + "FeatureDescription");

        /// <summary>The CUDA stream in which to run the feature.</summary>
        public static readonly ParameterName CudaStream = new(PREFIX + "CUDAStream");

        /// <summary>
        /// The path to the directory that contains the TensorRT model files that will be used
        /// to run inference for face detection or landmark detection, and the .nvf file that contains the 3D Face model,
        /// excluding the model file name. For details about the format of the .nvf file, Refer to NVIDIA 3DMM File Format.
        /// </summary>
        public static readonly ParameterName ModelDir = new (PREFIX + "ModelDir");

        /// <summary>The number of inferences to be run at one time on the GPU.</summary>
        public static readonly ParameterName BatchSize = new (PREFIX + "BatchSize");

        /// <summary>
        /// The length of the output buffer that contains the X and Y coordinates in pixels of the detected landmarks.
        /// This property applies only to the landmark detection feature.
        /// </summary>
        public static readonly ParameterName Landmarks_Size = new (PREFIX + "Landmarks_Size");

        /// <summary>
        /// The length of the output buffer that contains the confidence values of the detected landmarks.
        /// This property applies only to the landmark detection feature.
        /// </summary>
        public static readonly ParameterName LandmarksConfidence_Size = new (PREFIX + "LandmarksConfidence_Size");

        /// <summary>Flag to enable optimization for temporal input frames. Enable the flag when the input is a video.</summary>
        public static readonly ParameterName Temporal = new (PREFIX + "Temporal");

        /// <summary>
        /// The number of eigenvalues used to describe shape.
        /// In the supplied face_model2.nvf, there are 100 shapes (also known as identity) eigenvalues,
        /// but the ShapeEigenValueCount should be queried when you allocate an array to receive the eigenvalues.
        /// </summary>
        public static readonly ParameterName ShapeEigenValueCount = new (PREFIX + "ShapeEigenValueCount");

        /// <summary>
        /// The number of coefficients used to represent expression.
        /// In the supplied face_model2.nvf, there are 53 expression blendshape coefficients,
        /// but theExpressionCount should be queried when allocating an array to receive the coefficients.
        /// </summary>
        public static readonly ParameterName ExpressionCount = new (PREFIX + "ExpressionCount");

        /// <summary>Flag to enable CUDA Graph optimization. The CUDA graph reduces the overhead of GPU operation submission of 3D body tracking.</summary>
        public static readonly ParameterName UseCudaGraph = new (PREFIX + "UseCudaGraph");

        /// <summary>Mode to select High Performance or High Quality for 3D Body Pose or Facial Landmark Detection.</summary>
        public static readonly ParameterName Mode = new (PREFIX + "Mode");

        /// <summary>Specifies the number of keypoints available, which is currently 34. Unsigned integer.</summary>
        public static readonly ParameterName NumKeyPoints = new (PREFIX + "NumKeyPoints");

        /// <summary>CPU buffer of type NvAR_Point3f to hold the Reference Pose for Joint Rotations for 3D Body Pose.</summary>
        public static readonly ParameterName ReferencePose = new (PREFIX + "ReferencePose");

        /// <summary>Flag to select Multi-Person Tracking for 3D Body Pose Tracking.</summary>
        public static readonly ParameterName TrackPeople = new (PREFIX + "TrackPeople");

        /// <summary>
        /// The age after which the multi-person tracker no longer tracks the object in shadow mode.
        /// This property is measured in the number of frames.
        /// Flag to select Multi-Person Tracking for 3D Body Pose Tracking.
        /// </summary>
        public static readonly ParameterName ShadowTrackingAge = new (PREFIX + "ShadowTrackingAge");

        /// <summary>
        /// The age after which the multi-person tracker marks the object valid and assigns an ID for tracking.
        /// This property is measured in the number of frames.
        /// </summary>
        public static readonly ParameterName ProbationAge = new (PREFIX + "ProbationAge");

        /// <summary>
        /// The maximum number of targets to be tracked by the multi-person tracker.
        /// After the new maximum target tracked limit is met, any new targets will be discarded.
        /// </summary>
        public static readonly ParameterName MaxTargetsTracked = new (PREFIX + "MaxTargetsTracked");
    }

    /// <summary>Here are the input properties in the AR SDK:</summary>
    public static class Input
    {
        private const string PREFIX = "NvAR_Parameter_Input_";

        /// <summary>GPU input image buffer of type NvCVImage</summary>
        public static readonly ParameterName Image = new (PREFIX + "Image");

        /// <summary>The width of the input image buffer in pixels.</summary>
        public static readonly ParameterName Width = new (PREFIX + "Width");

        /// <summary>The height of the input image buffer in pixels.</summary>
        public static readonly ParameterName Height = new (PREFIX + "Height");

        /// <summary>CPU input array of type NvAR_Point2f that contains the facial landmark points.</summary>
        public static readonly ParameterName Landmarks = new (PREFIX + "Landmarks");

        /// <summary>Bounding boxes that determine the region of interest (ROI) of an input image that contains a face of type NvAR_BBoxes.</summary>
        public static readonly ParameterName BoundingBoxes = new (PREFIX + "BoundingBoxes");

        /// <summary>The focal length of the camera used for 3D Body Pose.</summary>
        public static readonly ParameterName FocalLength = new (PREFIX + "FocalLength");
    }

    /// <summary>Here are the output properties in the AR SDK</summary>
    public static class Output
    {
        private const string PREFIX = "NvAR_Parameter_Output_";

        /// <summary>CPU output bounding boxes of type NvAR_BBoxes.</summary>
        public static readonly ParameterName BoundingBoxes = new (PREFIX + "BoundingBoxes");

        /// <summary>CPU output tracking bounding boxes of type NvAR_TrackingBBoxes.</summary>
        public static readonly ParameterName TrackingBoundingBoxes = new (PREFIX + "TrackingBoundingBoxes");

        /// <summary>Float array of confidence values for each returned bounding box.</summary>
        public static readonly ParameterName BoundingBoxesConfidence = new (PREFIX + "BoundingBoxesConfidence");

        /// <summary>
        /// CPU output buffer of type NvAR_Point2f to hold the output detected landmark key points.
        /// Refer to Facial point annotations for more information.
        /// The order of the points in the CPU buffer follows the order in MultiPIE 68-point markups,
        /// and the 126 points cover more points along the cheeks, the eyes, and the laugh lines.
        /// </summary>
        public static readonly ParameterName Landmarks = new (PREFIX + "Landmarks");

        /// <summary>Float array of confidence values for each detected landmark point.</summary>
        public static readonly ParameterName LandmarksConfidence = new (PREFIX + "LandmarksConfidence");

        /// <summary>CPU array of type NvAR_Quaternion to hold the output-detected pose as an XYZW quaternion.</summary>
        public static readonly ParameterName Pose = new (PREFIX + "Pose");

        /// <summary>CPU 3D face Mesh of type NvAR_FaceMesh.</summary>
        public static readonly ParameterName FaceMesh = new (PREFIX + "FaceMesh");

        /// <summary>CPU output structure of type NvAR_RenderingParams that contains the rendering parameters that might be used to render the 3D face mesh.</summary>
        public static readonly ParameterName RenderingParams = new (PREFIX + "RenderingParams");

        /// <summary>Float array of shape eigenvalues. Get ShapeEigenValueCount to determine how many eigenvalues there are.</summary>
        public static readonly ParameterName ShapeEigenValues = new (PREFIX + "ShapeEigenValues");

        /// <summary>Float array of expression coefficients. Get ExpressionCount to determine how many coefficients there are.</summary>
        public static readonly ParameterName ExpressionCoefficients = new (PREFIX + "ExpressionCoefficients");

        /// <summary>
        /// CPU output buffer of type NvAR_Point2f to hold the output detected 2D Keypoints for Body Pose.
        /// Refer to 3D Body Pose Keypoint Format for information about the Keypoint names and the order of Keypoint output.
        /// </summary>
        public static readonly ParameterName KeyPoints = new (PREFIX + "KeyPoints");

        /// <summary>
        /// CPU output buffer of type NvAR_Point3f to hold the output detected 3D Keypoints for Body Pose.
        /// Refer to 3D Body Pose Keypoint Format for information about the Keypoint names and the order of Keypoint output.
        /// </summary>
        public static readonly ParameterName KeyPoints3D = new (PREFIX + "KeyPoints3D");

        /// <summary>CPU output buffer of type NvAR_Point3f to hold the joint angles in axis-angle format for the Keypoints for Body Pose.</summary>
        public static readonly ParameterName JointAngles = new (PREFIX + "JointAngles");

        /// <summary>Float array of confidence values for each detected keypoints.</summary>
        public static readonly ParameterName KeyPointsConfidence = new (PREFIX + "KeyPointsConfidence");

        /// <summary>Float array of three values that represent the x, y and z values of head translation with respect to the camera for Eye Contact.</summary>
        public static readonly ParameterName OutputHeadTranslation = new (PREFIX + "OutputHeadTranslation");

        /// <summary>Float array of two values that represent the yaw and pitch angles of the estimated gaze for Eye Contact.</summary>
        public static readonly ParameterName OutputGazeVector = new (PREFIX + "OutputGazeVector");

        /// <summary>
        /// CPU array of type NvAR_Quaternion to hold the output-detected head pose as an XYZW quaternion in Eye Contact.
        /// This is an alternative to the head pose that was obtained from the facial landmarks feature.
        /// This head pose is obtained using the PnP algorithm over the landmarks.
        /// </summary>
        public static readonly ParameterName HeadPose = new (PREFIX + "HeadPose");

        /// <summary>Float array of two values that represent the yaw and pitch angles of the estimated gaze for Eye Contact.</summary>
        public static readonly ParameterName GazeDirection = new (PREFIX + "GazeDirection");
    }
}
