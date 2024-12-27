using System.Runtime.InteropServices;

namespace NvARdotNet;

// typedef struct NvAR_Vector3f {
//   float vec[3];
// } NvAR_Vector3f;

/// <summary>This structure represents a 3D vector.</summary>
[StructLayout(LayoutKind.Sequential)]
public struct Vector3f
{
    /// <summary>The X component of the 3D vector.</summary>
    public float X;

    /// <summary>The Y component of the 3D vector.</summary>
    public float Y;

    /// <summary>The Z component of the 3D vector.</summary>
    public float Z;
}
