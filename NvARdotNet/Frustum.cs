using System.Runtime.InteropServices;

namespace NvARdotNet;

// typedef struct NvAR_Frustum {
//   float left;
//   float right;
//   float bottom;
//   float top;
// } NvAR_Frustum;

/// <summary>
/// This structure represents a camera viewing frustum for an orthographic camera.
/// As a result, it contains only the left, the right, the top, and the bottom coordinates in pixels.
/// It does not contain a near or a far clipping plane.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct Frustum
{
    /// <summary>The X coordinate of the top-left corner of the viewing frustum.</summary>
    public float Left;

    /// <summary>The X coordinate of the bottom-right corner of the viewing frustum.</summary>
    public float Right;

    /// <summary>The Y coordinate of the bottom-right corner of the viewing frustum.</summary>
    public float Bottom;

    /// <summary>The Y coordinate of the top-left corner of the viewing frustum.</summary>
    public float Top;
}