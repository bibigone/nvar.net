using System.Runtime.InteropServices;

namespace NvARdotNet;

// typedef struct NvAR_Point2f {
//   float x, y;
// } NvAR_Point2f;

/// <summary>This structure represents the X and Y coordinates of one point in 2D space.</summary>
[StructLayout(LayoutKind.Sequential)]
public struct Point2f
{
    /// <summary>The X coordinate of the point in pixels.</summary>
    public float X;

    /// <summary>The Y coordinate of the point in pixels.</summary>
    public float Y;
}
