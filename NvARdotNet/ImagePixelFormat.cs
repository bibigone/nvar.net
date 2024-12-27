namespace NvARdotNet;

// typedef enum NvCVImage_PixelFormat {
//   ...
// } NvCVImage_PixelFormat;
/// <summary>The format of pixels in an image.</summary>
public enum ImagePixelFormat : int
{
    /// <summary>Unknown pixel format.</summary>
    Unknown = 0,

    /// <summary>Luminance (gray).</summary>
    Y = 1,

    /// <summary>Alpha (opacity).</summary>
    A = 2,

    /// <summary>{ Luminance, Alpha }</summary>
    YA = 3,

    /// <summary>{ Red, Green, Blue }</summary>
    RGB = 4,

    /// <summary>{ Red, Green, Blue }</summary>
    BGR = 5,

    /// <summary>{ Red, Green, Blue, Alpha }</summary>
    RGBA = 6,

    /// <summary>{ Red, Green, Blue, Alpha }</summary>
    BGRA = 7,

    /// <summary>{ Red, Green, Blue, Alpha }</summary>
    ARGB = 8,

    /// <summary>{ Red, Green, Blue, Alpha }</summary>
    ABGR = 9,

    /// <summary>Luminance and subsampled Chrominance { Y, Cb, Cr }</summary>
    YUV420 = 10,

    /// <summary>Luminance and subsampled Chrominance { Y, Cb, Cr }</summary>
    YUV422 = 11,

    /// <summary>Luminance and full bandwidth Chrominance { Y, Cb, Cr }</summary>
    YUV444 = 12,
}
