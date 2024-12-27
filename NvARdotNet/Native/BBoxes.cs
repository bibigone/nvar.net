using System;
using System.Runtime.InteropServices;

namespace NvARdotNet.Native;

// typedef struct NvAR_BBoxes {
//   NvAR_Rect *boxes;
//   uint8_t num_boxes;
//   uint8_t max_boxes;
// } NvAR_BBoxes;

/// <summary>This structure is returned as the output of the face detection feature.</summary>
[StructLayout(LayoutKind.Sequential)]
internal unsafe struct BBoxes
{
    /// <summary>Pointer to an array of bounding boxes that are allocated by the user.</summary>
    public Rect* Boxes;

    public IntPtr BoxesIntPtr
    {
        get => new(Boxes);
        set => Boxes = (Rect*)value.ToPointer();
    }

    /// <summary>The number of bounding boxes in the array.</summary>
    public int BoxCount
    {
        get => numBoxes;
        set
        {
            if (value < byte.MinValue || value > byte.MaxValue) throw new ArgumentOutOfRangeException(nameof(value));
            numBoxes = (byte)value;
        }
    }
    private byte numBoxes;

    /// <summary>The maximum number of bounding boxes that can be stored in the array as defined by the user.</summary>
    public int MaxBoxCount
    {
        get => maxBoxes;
        set
        {
            if (value < byte.MinValue || value > byte.MaxValue) throw new ArgumentOutOfRangeException(nameof(value));
            maxBoxes = (byte)value;
        }
    }
    private byte maxBoxes;
}