using System.Runtime.InteropServices;

namespace NvARdotNet;

// typedef struct NvAR_Point3f {
//   float x, y, z;
// } NvAR_Point3f;

/// <summary>This structure represents the X, Y, Z coordinates of one point in 3D space.</summary>
[StructLayout(LayoutKind.Sequential)]
public struct Point3f
{
    /// <summary>The X coordinate of the point in pixels.</summary>
    public float X;

    /// <summary>The Y coordinate of the point in pixels.</summary>
    public float Y;

    /// <summary>The Z coordinate of the point in pixels.</summary>
    public float Z;
}
