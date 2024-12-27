using System.Runtime.InteropServices;

namespace NvARdotNet;

// typedef struct NvAR_TrackingBBox {
//   NvAR_Rect bbox;
//   uint16_t tracking_id;
// } NvAR_TrackingBBox;
/// <summary>This structure is returned as the output of the body pose feature when multi-person tracking is enabled.</summary>
[StructLayout(LayoutKind.Sequential)]
public struct TrackingBoundingBox
{
    /// <summary>Bounding box that is allocated by the user.</summary>
    public Rect BoundingBox;

    /// <summary>The Tracking ID assigned to the bounding box by Multi-Person Tracking.</summary>
    public ushort TrackingId;
}
