using System.Runtime.InteropServices;

namespace NvARdotNet;

// typedef struct NvCVPoint2i {
//   int x;  //!< The horizontal coordinate.
//   int y;  //!< The vertical coordinate
// } NvCVPoint2i;
[StructLayout(LayoutKind.Sequential)]
public struct Point2i
{
    public int X;
    public int Y;
}
