namespace NvARdotNet;

/// <summary>Location of image buffer storage.</summary>
public enum ImageMemorySpace : int
{
    /// <summary>The buffer is stored in CPU memory.</summary>
    CPU = 0,

    /// <summary>The buffer is stored in CUDA memory.</summary>
    GPU = 1,

    /// <summary>The buffer is stored in CUDA memory.</summary>
    CUDA = 1,

    /// <summary>The buffer is stored in pinned CPU memory.</summary>
    CPU_PINNED = 2,

    /// <summary>A CUDA array is used for storage.</summary>
    CUDA_ARRAY = 3,
}
