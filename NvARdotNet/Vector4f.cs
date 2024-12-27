using System.Runtime.InteropServices;

namespace NvARdotNet;

// typedef struct NvAR_Quaternion {
//   float x, y, z, w;
// } NvAR_Quaternion;

/// <summary>This structure represents the coefficients in the quaternion.</summary>
[StructLayout(LayoutKind.Sequential)]
public struct Vector4f
{
    /// <summary>The first coefficient of the complex part of the quaternion.</summary>
    public float X;

    /// <summary>The second coefficient of the complex part of the quaternion.</summary>
    public float Y;

    /// <summary>The third coefficient of the complex part of the quaternion.</summary>
    public float Z;

    /// <summary>The scalar coefficient of the quaternion.</summary>
    public float W;
}