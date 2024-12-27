using System.Runtime.InteropServices;

namespace NvARdotNet;

// typedef struct NvCVRect2i   {
//   int x;      //!< The left edge of the rectangle.
//   int y;      //!< The top  edge of the rectangle.
//   int width;  //!< The width  of the rectangle.
//   int height; //!< The height of the rectangle.
// } NvCVRect2i;
[StructLayout(LayoutKind.Sequential)]
public struct Rect2i
{
    public int X;
    public int Y;
    public int Width;
    public int Height;
}
