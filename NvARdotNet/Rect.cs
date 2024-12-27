using System.Runtime.InteropServices;

namespace NvARdotNet;

// typedef struct NvAR_Rect {
//   float x, y, width, height;
// } NvAR_Rect;

/// <summary>This structure represents the position and size of a rectangular 2D bounding box.</summary>
[StructLayout(LayoutKind.Sequential)]
public struct Rect
{
    /// <summary>The X coordinate of the top left corner of the bounding box in pixels.</summary>
    public float X;

    /// <summary>The Y coordinate of the top left corner of the bounding box in pixels.</summary>
    public float Y;

    /// <summary>The width of the bounding box in pixels.</summary>
    public float Width;

    /// <summary>The height of the bounding box in pixels.</summary>
    public float Height;
}
