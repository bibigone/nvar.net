using System.Runtime.InteropServices;

namespace NvARdotNet;

// typedef struct NvAR_RenderingParams {
//   NvAR_Frustum frustum;
//   NvAR_Quaternion rotation;
//   NvAR_Vector3f translation;
// } NvAR_RenderingParams;

/// <summary>
/// This structure defines the parameters that are used to draw a 3D face mesh in a window on the computer screen
/// so that the face mesh is aligned with the corresponding video frame.
/// The projection matrix is constructed from the frustum parameter,
/// and the model view matrix is constructed from the rotation and translation parameters.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct RenderingParams
{
    /// <summary>The camera viewing frustum for an orthographic camera.</summary>
    public Frustum Frustum;

    /// <summary>The rotation of the camera relative to the mesh.</summary>
    public Vector4f Rotation;

    /// <summary>The translation of the camera relative to the mesh.</summary>
    public Vector3f Translation;
}
