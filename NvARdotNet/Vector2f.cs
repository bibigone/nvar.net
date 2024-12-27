using System.Runtime.InteropServices;

namespace NvARdotNet;

// typedef struct NvAR_Vector2f {
//   float x, y;
// } NvAR_Vector2f;

/// <summary>This structure represents a 2D vector.</summary>
[StructLayout(LayoutKind.Sequential)]
public struct Vector2f
{
    /// <summary>The X component of the 2D vector.</summary>
    public float X;

    /// <summary>The Y component of the 2D vector.</summary>
    public float Y;
}
