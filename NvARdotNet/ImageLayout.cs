namespace NvARdotNet;

/// <summary>
/// Value for the planar field or layout argument. Two values are currently accommodated for RGB:
/// Interleaved or chunky storage locates all components of a pixel adjacent in memory,
/// e.g. RGBRGBRGB... (denoted [RGB]).
/// Planar storage locates the same component of all pixels adjacent in memory,
/// e.g. RRRRR...GGGGG...BBBBB... (denoted [R][G][B]).
/// YUV has many more variants.
/// 4:2:2 can be chunky, planar or semi-planar, with different orderings.
/// 4:2:0 can be planar or semi-planar, with different orderings.
/// Aliases are provided for FOURCCs defined at fourcc.org.
/// Note: the LSB can be used to distinguish between chunky and planar formats.
/// </summary>
public enum ImageLayout : int
{
    /// <summary>All components of pixel(x,y) are adjacent (same as chunky) (default for non-YUV).</summary>
    Interleaved = 0,

    /// <summary>All components of pixel(x,y) are adjacent (same as interleaved).</summary>
    Chunky = Interleaved,

    /// <summary>The same component of all pixels are adjacent.</summary>
    Planar = 1,

    /// <summary>[UYVY] Chunky 4:2:2 (default for 4:2:2).</summary>
    UYVY = 2,

    /// <summary>[VYUY] Chunky 4:2:2.</summary>
    VYUY = 4,

    /// <summary>[YUYV] Chunky 4:2:2.</summary>
    YUYV = 6,

    /// <summary>[YVYU] Chunky 4:2:2.</summary>
    YVYU = 8,

    /// <summary>[YUV] Chunky 4:4:4.</summary>
    CYUV = 10,

    /// <summary>[YVU] Chunky 4:4:4.</summary>
    CYVU = 12,

    /// <summary>[Y][U][V] Planar 4:2:2 or 4:2:0 or 4:4:4.</summary>
    YUV = 3,

    /// <summary>[Y][V][U] Planar 4:2:2 or 4:2:0 or 4:4:4.</summary>
    YVU = 5,

    /// <summary>[Y][UV] Semi-planar 4:2:2 or 4:2:0 (default for 4:2:0).</summary>
    YCUV = 7,

    /// <summary>[Y][VU] Semi-planar 4:2:2 or 4:2:0.</summary>
    YCVU = 9,

    // The following are FOURCC aliases for specific layouts. Note that it is still required to specify the format as well
    // as the layout, e.g. NVCV_YUV420 and NVCV_NV12, even though the NV12 layout is only associated with YUV420 sampling.
    #region FOURCC aliases for specific layouts

    /// <summary>[Y][U][V] Planar 4:2:0.</summary>
    I420 = YUV,

    /// <summary>[Y][U][V] Planar 4:2:0.</summary>
    IYUV = YUV,

    /// <summary>[Y][V][U] Planar 4:2:0.</summary>
    YV12 = YVU,

    /// <summary>[Y][UV] Semi-planar 4:2:0 (default for 4:2:0).</summary>
    NV12 = YCUV,

    /// <summary>[Y][VU] Semi-planar 4:2:0.</summary>
    NV21 = YCVU,

    /// <summary>[YUYV] Chunky 4:2:2.</summary>
    YUY2 = YUYV,

    /// <summary>[Y][U][V] Planar 4:4:4.</summary>
    I444 = YUV,

    /// <summary>[Y][U][V] Planar 4:4:4.</summary>
    YM24 = YUV,

    /// <summary>[Y][V][U] Planar 4:4:4.</summary>
    YM42 = YVU,

    /// <summary>[Y][UV] Semi-planar 4:4:4.</summary>
    NV24 = YCUV,

    /// <summary>[Y][VU] Semi-planar 4:4:4.</summary>
    NV42 = YCVU,

    #endregion
}
