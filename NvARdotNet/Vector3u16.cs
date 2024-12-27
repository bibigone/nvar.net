using System.Runtime.InteropServices;

namespace NvARdotNet;

// typedef struct NvAR_Vector3u16 {
//   unsigned short vec[3];
// } NvAR_Vector3u16;

/// <summary>This structure represents a 3D vector.</summary>
[StructLayout(LayoutKind.Sequential)]
public struct Vector3u16
{
    /// <summary>The X component of the 3D vector.</summary>
    public ushort X;

    /// <summary>The Y component of the 3D vector.</summary>
    public ushort Y;

    /// <summary>The Z component of the 3D vector.</summary>
    public ushort Z;
}
