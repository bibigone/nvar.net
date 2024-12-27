namespace NvARdotNet;

// typedef enum NvCVImage_ComponentType {
//   ...
// } NvCVImage_ComponentType;
/// <summary>
/// The data type used to represent each component of an image.
/// </summary>
public enum ImageComponentType : int
{
    /// <summary>Unknown type of component.</summary>
    Unknown = 0,

    /// <summary>Unsigned 8-bit integer.</summary>
    U8 = 1,

    /// <summary>Unsigned 16-bit integer.</summary>
    U16 = 2,

    /// <summary>Signed 16-bit integer.</summary>
    S16 = 3,

    /// <summary>16-bit floating-point.</summary>
    F16 = 4,

    /// <summary>Unsigned 32-bit integer.</summary>
    U32 = 5,

    /// <summary>Signed 32-bit integer.</summary>
    S32 = 6,

    /// <summary>32-bit floating-point (float).</summary>
    F32 = 7,

    /// <summary>Unsigned 64-bit integer.</summary>
    U64 = 8,

    /// <summary>Signed 64-bit integer.</summary>
    S64 = 9,

    /// <summary>64-bit floating-point (double).</summary>
    F64 = 10,
}
