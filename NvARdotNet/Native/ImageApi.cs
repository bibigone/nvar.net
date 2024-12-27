using System;
using System.Runtime.InteropServices;

namespace NvARdotNet.Native;

/// <summary>See nvCVImage.h from native SDK.</summary>
internal static unsafe class ImageApi
{
    public const string LIB_NAME = "NVCVImage";
    public const string PREFIX = "NvCVImage_";
    public const CallingConvention CALLING_CONVENTION = CallingConvention.Cdecl;

    // const char* NvCV_GetErrorStringFromCode(NvCV_Status code);
    /// <summary>Get an error string corresponding to the given status code.</summary>
    /// <param name="status">the NvCV status code.</param>
    /// <returns>the corresponding string.</returns>
    [DllImport(LIB_NAME, EntryPoint = "NvCV_" + nameof(GetErrorStringFromCode), CallingConvention = CALLING_CONVENTION)]
    public static extern IntPtr GetErrorStringFromCode(Status status);

    // NvCV_Status NvCV_API NvCVImage_Init(NvCVImage* im, unsigned width, unsigned height, int pitch, void* pixels,
    //   NvCVImage_PixelFormat format, NvCVImage_ComponentType type, unsigned layout, unsigned memSpace);
    /// <summary>Initialize an image.</summary>
    /// <param name="im">the image to initialize.</param>
    /// <param name="width">the desired width  of the image, in pixels.</param>
    /// <param name="height">the desired height of the image, in pixels.</param>
    /// <param name="pitch">the byte stride between pixels vertically.</param>
    /// <param name="pixels">a pointer to the pixel buffer.</param>
    /// <param name="format">the format of the pixels.</param>
    /// <param name="type">the type of the components of the pixels.</param>
    /// <param name="layout">One of { NVCV_CHUNKY, NVCV_PLANAR } or one of the YUV layouts.</param>
    /// <param name="memSpace">Location of the buffer: one of { NVCV_CPU, NVCV_CPU_PINNED, NVCV_GPU, NVCV_CUDA }</param>
    /// <returns>NVCV_SUCCESS if successful. NVCV_ERR_PIXELFORMAT if the pixel format is not yet accommodated.</returns>
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(Init), CallingConvention = CALLING_CONVENTION)]
    public static extern Status Init(ImageStruct* im, int width, int height, int pitch, IntPtr pixels,
        ImagePixelFormat format, ImageComponentType type, ImageLayout layout, ImageMemorySpace memSpace);

    // void NvCV_API NvCVImage_InitView(NvCVImage* subImg, NvCVImage* fullImg, int x, int y, unsigned width, unsigned height);
    /// <summary>
    /// Initialize a view into a subset of an existing image.
    /// No memory is allocated -- the fullImg buffer is used.
    /// </summary>
    /// <param name="subImg">the sub-image view into the existing full image.</param>
    /// <param name="fullImage">the existing full image.</param>
    /// <param name="x">the left edge of the sub-image, as coordinate of the full image.</param>
    /// <param name="y">the top  edge of the sub-image, as coordinate of the full image.</param>
    /// <param name="width">the desired width  of the subImage, in pixels.</param>
    /// <param name="height">the desired height of the subImage, in pixels.</param>
    /// <remarks><para>
    /// This does not work in general for planar or semi-planar formats, neither RGB nor YUV.
    /// However, it does work for all formats with the full image, to make a shallow copy, e.g.
    /// NvCVImage_InitView(&subImg, &fullImg, 0, 0, fullImage.width, fullImage.height).
    /// Cropping a planar or semi-planar image can be accomplished with NvCVImage_TransferRect().
    /// </para><para>
    /// This does work for all chunky formats, including UYVY, VYUY, YUYV, YVYU.
    /// </para></remarks>
    /// <seealso cref="TransferRect"/>
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(InitView), CallingConvention = CALLING_CONVENTION)]
    public static extern void InitView(ImageStruct* subImg, ImageStruct* fullImage, int x, int y, int width, int height);

    // NvCV_Status NvCV_API NvCVImage_Alloc(NvCVImage* im, unsigned width, unsigned height, NvCVImage_PixelFormat format,
    //   NvCVImage_ComponentType type, unsigned layout, unsigned memSpace, unsigned alignment);
    /// <summary>
    ///  Allocate memory for, and initialize an image. This assumes that the image data structure has nothing meaningful in it.
    /// </summary>
    /// <param name="im">the image to initialize.</param>
    /// <param name="width">the desired width  of the image, in pixels.</param>
    /// <param name="height">the desired height of the image, in pixels.</param>
    /// <param name="format">the format of the pixels.</param>
    /// <param name="type">the type of the components of the pixels.</param>
    /// <param name="layout">One of { NVCV_CHUNKY, NVCV_PLANAR } or one of the YUV layouts.</param>
    /// <param name="memSpace">Location of the buffer: one of { NVCV_CPU, NVCV_CPU_PINNED, NVCV_GPU, NVCV_CUDA }</param>
    /// <param name="alignment">
    /// row byte alignment. Choose 0 or a power of 2.
    /// 1: yields no gap whatsoever between scanlines;
    /// 0: default alignment: 4 on CPU, and cudaMallocPitch's choice on GPU.
    /// Other common values are 16 or 32 for cache line size.
    /// </param>
    /// <returns>
    /// NVCV_SUCCESS         if the operation was successful.
    /// NVCV_ERR_PIXELFORMAT if the pixel format is not accommodated.
    /// NVCV_ERR_MEMORY      if there is not enough memory to allocate the buffer.
    /// </returns>
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(Alloc), CallingConvention = CALLING_CONVENTION)]
    public static extern Status Alloc(ImageStruct* im, int width, int height, ImagePixelFormat format,
        ImageComponentType type, ImageLayout layout, ImageMemorySpace memSpace, int alignment);

    // NvCV_Status NvCV_API NvCVImage_Realloc(NvCVImage* im, unsigned width, unsigned height, NvCVImage_PixelFormat format,
    //   NvCVImage_ComponentType type, unsigned layout, unsigned memSpace, unsigned alignment);
    /// <summary>
    /// Reallocate memory for, and initialize an image. This assumes that the image is valid.
    /// It will check bufferBytes to see if enough memory is already available, and will reshape rather than realloc if true.
    /// Otherwise, it will free the previous buffer and reallocate a new one.
    /// </summary>
    /// <param name="im">the image to initialize.</param>
    /// <param name="width">the desired width  of the image, in pixels.</param>
    /// <param name="height">the desired height of the image, in pixels.</param>
    /// <param name="format">the format of the pixels.</param>
    /// <param name="type">the type of the components of the pixels.</param>
    /// <param name="layout">One of { NVCV_CHUNKY, NVCV_PLANAR } or one of the YUV layouts.</param>
    /// <param name="memSpace">Location of the buffer: one of { NVCV_CPU, NVCV_CPU_PINNED, NVCV_GPU, NVCV_CUDA }</param>
    /// <param name="alignment">
    /// row byte alignment. Choose 0 or a power of 2.
    /// 1: yields no gap whatsoever between scanlines;
    /// 0: default alignment: 4 on CPU, and cudaMallocPitch's choice on GPU.
    /// Other common values are 16 or 32 for cache line size.
    /// </param>
    /// <returns>
    /// NVCV_SUCCESS         if the operation was successful.
    /// NVCV_ERR_PIXELFORMAT if the pixel format is not accommodated.
    /// NVCV_ERR_MEMORY      if there is not enough memory to allocate the buffer.
    /// </returns>
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(Realloc), CallingConvention = CALLING_CONVENTION)]
    public static extern Status Realloc(ImageStruct* im, int width, int height, ImagePixelFormat format,
        ImageComponentType type, ImageLayout layout, ImageMemorySpace memSpace, int alignment);

    // void NvCV_API NvCVImage_Dealloc(NvCVImage* im);
    /// <summary>Deallocate the image buffer from the image. The image is not deallocated.</summary>
    /// <param name="im">the image whose buffer is to be deallocated.</param>
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(Dealloc), CallingConvention = CALLING_CONVENTION)]
    public static extern void Dealloc(ImageStruct* im);

    // void NvCV_API NvCVImage_DeallocAsync(NvCVImage* im, struct CUstream_st *stream);
    /// <summary>
    /// Deallocate the image buffer from the image asynchronously on the specified stream.
    /// The image is not deallocated.
    /// </summary>
    /// <param name="im">the image whose buffer is to be deallocated.</param>
    /// <param name="stream">the CUDA stream on which the image buffer is to be deallocated.</param>
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(DeallocAsync), CallingConvention = CALLING_CONVENTION)]
    public static extern void DeallocAsync(ImageStruct* im, CudaStreamPointer stream);

    // NvCV_Status NvCV_API NvCVImage_Create(unsigned width, unsigned height, NvCVImage_PixelFormat format,
    //    NvCVImage_ComponentType type, unsigned layout, unsigned memSpace, unsigned alignment, NvCVImage**out);
    /// <summary>Allocate a new image, with storage (C-style constructor).</summary>
    /// <param name="width">the desired width  of the image, in pixels.</param>
    /// <param name="height">the desired height of the image, in pixels.</param>
    /// <param name="format">the format of the pixels.</param>
    /// <param name="type">the type of the components of the pixels.</param>
    /// <param name="layout">One of { NVCV_CHUNKY, NVCV_PLANAR } or one of the YUV layouts.</param>
    /// <param name="memSpace">Location of the buffer: one of { NVCV_CPU, NVCV_CPU_PINNED, NVCV_GPU, NVCV_CUDA }</param>
    /// <param name="alignment">
    /// row byte alignment. Choose 0 or a power of 2.
    /// 1: yields no gap whatsoever between scanlines;
    /// 0: default alignment: 4 on CPU, and cudaMallocPitch's choice on GPU.
    /// Other common values are 16 or 32 for cache line size.
    /// </param>
    /// <param name="im">will be a pointer to the new image if successful; otherwise NULL.</param>
    /// <returns>
    /// NVCV_SUCCESS         if the operation was successful.
    /// NVCV_ERR_PIXELFORMAT if the pixel format is not accommodated.
    /// NVCV_ERR_MEMORY      if there is not enough memory to allocate the buffer.
    /// </returns>
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(Create), CallingConvention = CALLING_CONVENTION)]
    public static extern Status Create(int width, int height, ImagePixelFormat format,
        ImageComponentType type, ImageLayout layout, ImageMemorySpace memSpace, int alignment, out ImageStruct* im);

    // void NvCV_API NvCVImage_Destroy(NvCVImage* im);
    /// <summary>Deallocate the image allocated with NvCVImage_Create() (C-style destructor).</summary>
    /// <param name="im"></param>
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(Destroy), CallingConvention = CALLING_CONVENTION)]
    public static extern void Destroy(ImageStruct* im);

    // void NvCV_API NvCVImage_ComponentOffsets(NvCVImage_PixelFormat format, int* rOff, int* gOff, int* bOff, int* aOff, int* yOff);
    /// <summary>
    /// Get offsets for the components of a pixel format.
    /// These are not byte offsets, but component offsets.
    /// </summary>
    /// <param name="format">the pixel format to be interrogated.</param>
    /// <param name="rOff">a place to store the offset for the red       channel</param>
    /// <param name="gOff">a place to store the offset for the green     channel</param>
    /// <param name="bOff">a place to store the offset for the blue      channel</param>
    /// <param name="aOff">a place to store the offset for the alpha     channel</param>
    /// <param name="yOff">a place to store the offset for the luminance channel</param>
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(ComponentOffsets), CallingConvention = CALLING_CONVENTION)]
    public static extern void ComponentOffsets(ImagePixelFormat format, out int rOff, out int gOff, out int bOff, out int aOff, out int yOff);

    // NvCV_Status NvCV_API NvCVImage_Transfer(
    //         const NvCVImage* src, NvCVImage *dst, float scale, struct CUstream_st *stream, NvCVImage* tmp);
    /// <summary>
    /// Transfer one image to another, with a limited set of conversions.
    /// </summary>
    /// <param name="src">the source image.</param>
    /// <param name="dst">the destination image.</param>
    /// <param name="scale">
    /// a scale factor that can be applied when one (but not both) of the images
    /// is based on floating-point components; this parameter is ignored when all image components
    /// are represented with integer data types, or all image components are represented with
    /// floating-point data types.
    /// </param>
    /// <param name="stream">the stream on which to perform the copy. This is ignored if both images reside on the CPU.</param>
    /// <param name="tmp">
    /// a temporary buffer that is sometimes needed when transferring images
    /// between the CPU and GPU in either direction (can be empty or NULL).
    /// It has the same characteristics as the CPU image, but it resides on the GPU.
    /// </param>
    /// <returns>
    /// NVCV_SUCCESS           if successful,
    /// NVCV_ERR_PIXELFORMAT   if one of the pixel formats is not accommodated.
    /// NVCV_ERR_CUDA          if a CUDA error has occurred.
    /// NVCV_ERR_GENERAL       if an otherwise unspecified error has occurred.
    /// </returns>
    /// <remarks>
    /// When there is some kind of conversion AND the src and dst reside on different processors (CPU, GPU),
    /// it is necessary to have a temporary GPU buffer, which is reshaped as needed to match the characteristics
    /// of the CPU image. The same temporary image can be used in subsequent calls to NvCVImage_Transfer(),
    /// regardless of the shape, format or component type, as it will grow as needed to accommodate
    /// the largest memory requirement. The recommended usage for most cases is to supply an empty image
    /// as the temporary; if it is not needed, no buffer is allocated. NULL can be supplied as the tmp
    /// image, in which case an ephemeral buffer is allocated if needed, with resultant
    /// performance degradation for image sequences.
    /// </remarks>
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(Transfer), CallingConvention = CALLING_CONVENTION)]
    public static extern Status Transfer(ImageStruct* src, ImageStruct* dst, float scale, CudaStreamPointer stream, ImageStruct* tmp);

    // NvCV_Status NvCV_API NvCVImage_TransferRect(
    //     const NvCVImage* src, const NvCVRect2i* srcRect, NvCVImage *dst, const NvCVPoint2i* dstPt,
    //     float scale, struct CUstream_st *stream, NvCVImage* tmp);
    /// <summary>
    /// Transfer a rectangular portion of an image.
    /// </summary>
    /// <param name="src">the source image.</param>
    /// <param name="srcRect">the subRect of the src to be transferred (NULL implies the whole image).</param>
    /// <param name="dst">the destination image.</param>
    /// <param name="dstPt">location to which the srcRect is to be copied (NULL implies (0,0)).</param>
    /// <param name="scale">scale factor applied to the magnitude during transfer, typically 1, 255 or 1/255.</param>
    /// <param name="stream">the CUDA stream.</param>
    /// <param name="tmp">a staging image.</param>
    /// <returns>NVCV_SUCCESS  if the operation was completed successfully.</returns>
    /// <remarks>
    /// The actual transfer region may be smaller, because the rects are clipped against the images.
    /// </remarks>
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(TransferRect), CallingConvention = CALLING_CONVENTION)]
    public static extern Status TransferRect(ImageStruct* src, in Rect2i srcRect, ImageStruct* dst, in Point2i dstPt,
        float scale, CudaStreamPointer stream, ImageStruct* tmp);

    //! Transfer from a YUV image.
    //! YUVu8 --> RGBu8 and YUVu8 --> RGBf32 are currently available.
    //! \param[in]  y             pointer to pixel(0,0) of the luminance channel.
    //! \param[in]  yPixBytes     the byte stride between y pixels horizontally.
    //! \param[in]  yPitch        the byte stride between y pixels vertically.
    //! \param[in]  u             pointer to pixel(0,0) of the u (Cb) chrominance channel.
    //! \param[in]  v             pointer to pixel(0,0) of the v (Cr) chrominance channel.
    //! \param[in]  uvPixBytes    the byte stride between u or v pixels horizontally.
    //! \param[in]  uvPitch       the byte stride between u or v pixels vertically.
    //! \param[in]  yuvColorSpace the yuv colorspace, specifying range, chromaticities, and chrominance phase.
    //! \param[in]  yuvMemSpace   the memory space where the pixel buffers reside.
    //! \param[out] dst           the destination image.
    //! \param[in]  dstRect       the destination rectangle (NULL implies the whole image).
    //! \param[in]  scale         scale factor applied to the magnitude during transfer, typically 1, 255 or 1/255.
    //! \param[in]  stream        the CUDA stream.
    //! \param[in]  tmp           a staging image.
    //! \return     NVCV_SUCCESS  if the operation was completed successfully.
    //! \note       The actual transfer region may be smaller, because the rects are clipped against the images.
    //! \note       This is supplied for use with YUV buffers that do not have the standard structure
    //!             that are expected for NvCVImage_Transfer() and NvCVImage_TransferRect.
    // NvCV_Status NvCV_API NvCVImage_TransferFromYUV(
    //     const void* y,                int yPixBytes,  int yPitch,
    //     const void* u, const void* v, int uvPixBytes, int uvPitch,
    //     NvCVImage_PixelFormat yuvFormat, NvCVImage_ComponentType yuvType,
    //     unsigned yuvColorSpace, unsigned yuvMemSpace,
    //     NvCVImage *dst, const NvCVRect2i* dstRect, float scale, struct CUstream_st *stream, NvCVImage* tmp);

    //! Transfer to a YUV image.
    //! RGBu8 --> YUVu8 and RGBf32 --> YUVu8 are currently available.
    //! \param[in]  src           the source image.
    //! \param[in]  srcRect       the destination rectangle (NULL implies the whole image).
    //! \param[out] y             pointer to pixel(0,0) of the luminance channel.
    //! \param[in]  yPixBytes     the byte stride between y pixels horizontally.
    //! \param[in]  yPitch        the byte stride between y pixels vertically.
    //! \param[out] u             pointer to pixel(0,0) of the u (Cb) chrominance channel.
    //! \param[out] v             pointer to pixel(0,0) of the v (Cr) chrominance channel.
    //! \param[in]  uvPixBytes    the byte stride between u or v pixels horizontally.
    //! \param[in]  uvPitch       the byte stride between u or v pixels vertically.
    //! \param[in]  yuvColorSpace the yuv colorspace, specifying range, chromaticities, and chrominance phase.
    //! \param[in]  yuvMemSpace   the memory space where the pixel buffers reside.
    //! \param[in]  scale         scale factor applied to the magnitude during transfer, typically 1, 255 or 1/255.
    //! \param[in]  stream        the CUDA stream.
    //! \param[in]  tmp           a staging image.
    //! \return     NVCV_SUCCESS  if the operation was completed successfully.
    //! \note       The actual transfer region may be smaller, because the rects are clipped against the images.
    //! \note       This is supplied for use with YUV buffers that do not have the standard structure
    //!             that are expected for NvCVImage_Transfer() and NvCVImage_TransferRect.
    // NvCV_Status NvCV_API NvCVImage_TransferToYUV(
    //     const NvCVImage* src, const NvCVRect2i* srcRect,
    //     const void* y,                int yPixBytes,  int yPitch,
    //     const void* u, const void* v, int uvPixBytes, int uvPitch,
    //     NvCVImage_PixelFormat yuvFormat, NvCVImage_ComponentType yuvType,
    //     unsigned yuvColorSpace, unsigned yuvMemSpace,
    //     float scale, struct CUstream_st *stream, NvCVImage* tmp);

    // NvCV_Status NvCV_API NvCVImage_MapResource(NvCVImage *im, struct CUstream_st *stream);
    /// <summary>
    /// Between rendering by a graphics system and Transfer by CUDA, it is necessary to map the texture resource.
    /// There is a fair amount of overhead, so its use should be minimized.
    /// Every call to NvCVImage_MapResource() should be matched by a subsequent call to NvCVImage_UnmapResource().
    /// </summary>
    /// <param name="im">the image to be mapped.</param>
    /// <param name="stream">the stream on which the mapping is to be performed.</param>
    /// <returns>NVCV_SUCCESS is the operation was completed successfully.</returns>
    /// <remarks>
    /// This is an experimental API. If you find it useful, please respond to XXX@YYY.com,
    /// otherwise we may drop support.
    /// </remarks>
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(MapResource), CallingConvention = CALLING_CONVENTION)]
    public static extern Status MapResource(ImageStruct* im, CudaStreamPointer stream);

    // NvCV_Status NvCV_API NvCVImage_UnmapResource(NvCVImage *im, struct CUstream_st *stream);
    /// <summary>
    /// After transfer by CUDA, the texture resource must be unmapped in order to be used by the graphics system again.
    /// There is a fair amount of overhead, so its use should be minimized.
    /// Every call to NvCVImage_UnmapResource() should correspond to a preceding call to NvCVImage_MapResource().
    /// </summary>
    /// <param name="im">the image to be mapped.</param>
    /// <param name="stream">the CUDA stream on which the mapping is to be performed.</param>
    /// <returns>NVCV_SUCCESS is the operation was completed successfully.</returns>
    /// <remarks>
    /// This is an experimental API. If you find it useful, please respond to XXX@YYY.com,
    /// otherwise we may drop support.
    /// </remarks>
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(UnmapResource), CallingConvention = CALLING_CONVENTION)]
    public static extern Status UnmapResource(ImageStruct* im, CudaStreamPointer stream);

    //! Composite one source image over another using the given matte.
    //! This accommodates all RGB and RGBA formats, with u8 and f32 components.
    //! If the bg has alpha, then the dst alpha is updated for use in subsequent composition.
    //! \param[in]  fg      the foreground source image.
    //! \param[in]  bg      the background source image.
    //! \param[in]  mat     the matte  Yu8   (or Au8)   image, indicating where the src should come through.
    //! \param[out] dst     the destination image. This can be the same as fg or bg.
    //! \param[in]  stream  the CUDA stream on which the composition is to be performed.
    //! \return NVCV_SUCCESS         if the operation was successful.
    //! \return NVCV_ERR_PIXELFORMAT if the pixel format is not accommodated.
    //! \return NVCV_ERR_MISMATCH    if either the fg & bg & dst formats do not match, or if fg & bg & dst & mat are not
    //!                              in the same address space (CPU or GPU).
    // NvCV_Status NvCV_API NvCVImage_Composite(const NvCVImage* fg, const NvCVImage* bg, const NvCVImage* mat, NvCVImage *dst,
    //             struct CUstream_st *stream);

    //! Composite one source image rectangular region over another using the given matte.
    //! This accommodates all RGB and RGBA formats, with u8 and f32 components.
    //! If the bg has alpha, then the dst alpha is updated for use in subsequent composition.
    //! If the background is not opaque, it is recommended that all images be premultiplied by alpha,
    //! and mode 1 composition be used, to yield the most meaningful composite matte.
    //! \param[in]      fg      the foreground source image.
    //! \param[in]      fgOrg   the upper-left corner of the fg image to be composited (NULL implies (0,0)).
    //! \param[in]      bg      the background source image.
    //! \param[in]      bgOrg   the upper-left corner of the bg image to be composited (NULL implies (0,0)).
    //! \param[in]      mat     the matte image, indicating where the src should come through.
    //!                         This determines the size of the rectangle to be composited.
    //!                         If this is multi-channel, the alpha channel is used as the matte.
    //! \param[in]      mode    the composition mode: 0 (straight alpha over) or 1 (premultiplied alpha over).
    //! \param[out]     dst     the destination image. This can be the same as fg or bg.
    //! \param[in]      dstOrg  the upper-left corner of the dst image to be updated (NULL implies (0,0)).
    //! \param[in]      stream  the CUDA stream on which the composition is to be performed.
    //! \note   If a smaller region of a matte is desired, a window can be created using
    //!         NvCVImage_InitView() for chunky pixels, as illustrated below in NvCVImage_CompositeRectA().
    //! \return NVCV_SUCCESS         if the operation was successful.
    //! \return NVCV_ERR_PIXELFORMAT if the pixel format is not accommodated.
    //! \return NVCV_ERR_MISMATCH    if either the fg & bg & dst formats do not match, or if fg & bg & dst & mat are not
    //!                              in the same address space (CPU or GPU).
    // NvCV_Status NvCV_API NvCVImage_CompositeRect(
    //     const NvCVImage* fg,  const NvCVPoint2i* fgOrg,
    //     const NvCVImage* bg,  const NvCVPoint2i* bgOrg,
    //     const NvCVImage* mat, unsigned mode,
    //     NvCVImage* dst, const NvCVPoint2i* dstOrg,
    //     struct CUstream_st *stream);

    //! Composite a source image over a constant color field using the given matte.
    //! \param[in]      src     the source image.
    //! \param[in]      mat     the matte  image, indicating where the src should come through.
    //! \param[in]      bgColor pointer to a location holding the desired flat background color, with the same format
    //!                         and component ordering as the dst. This acts as a 1x1 background pixel buffer,
    //!                         so should reside in the same memory space (CUDA or CPU) as the other buffers.
    //! \param[in,out]  dst     the destination image. May be the same as src.
    //! \return NVCV_SUCCESS         if the operation was successful.
    //! \return NVCV_ERR_PIXELFORMAT if the pixel format is not accommodated.
    //! \return NVCV_ERR_MISMATCH    if fg & mat & dst & bgColor are not in the same address space (CPU or GPU).
    //! \note   The bgColor must remain valid until complete; this is an important consideration especially if
    //!         the buffers are on the GPU and NvCVImage_CompositeOverConstant() runs asynchronously.
    // NvCV_Status NvCV_API NvCVImage_CompositeOverConstant(
    //     const NvCVImage *src, const NvCVImage *mat, const void *bgColor, NvCVImage *dst, struct CUstream_st *stream);

    //! Flip the image vertically.
    //! No actual pixels are moved: it is just an accounting procedure.
    //! This is extremely low overhead, but requires appropriate interpretation of the pitch.
    //! Flipping twice yields the original orientation.
    //! \param[in]  src  the source image (NULL implies src == dst).
    //! \param[out] dst  the flipped image (can be the same as the src).
    //! \return     NVCV_SUCCESS         if successful.
    //! \return     NVCV_ERR_PIXELFORMAT for most planar formats.
    //! \bug        This does not work for planar or semi-planar formats, neither RGB nor YUV.
    //! \note       This does work for all chunky formats, including UYVY, VYUY, YUYV, YVYU.
    // NvCV_Status NvCV_API NvCVImage_FlipY(const NvCVImage *src, NvCVImage *dst);

    //! Get the pointers for YUV, based on the format.
    //! \param[in]  im          The image to be deconstructed.
    //! \param[out] y           A place to store the pointer to y(0,0).
    //! \param[out] u           A place to store the pointer to u(0,0).
    //! \param[out] v           A place to store the pointer to v(0,0).
    //! \param[out] yPixBytes   A place to store the byte stride between  luma  samples horizontally.
    //! \param[out] cPixBytes   A place to store the byte stride between chroma samples horizontally.
    //! \param[out] yRowBytes   A place to store the byte stride between  luma  samples vertically.
    //! \param[out] cRowBytes   A place to store the byte stride between chroma samples vertically.
    //! \return     NVCV_SUCCESS           If the information was gathered successfully.
    //!             NVCV_ERR_PIXELFORMAT   Otherwise.
    // NvCV_Status NvCV_API NvCVImage_GetYUVPointers(NvCVImage *im,
    //   unsigned char **y, unsigned char **u, unsigned char **v,
    //   int *yPixBytes, int *cPixBytes, int *yRowBytes, int *cRowBytes);

    //! Sharpen an image.
    //! The src and dst should be the same type - conversions are not performed.
    //! This function is only implemented for NVCV_CHUNKY NVCV_U8 pixels, of format NVCV_RGB or NVCV_BGR.
    //! \param[in]  sharpness the sharpness strength, calibrated so that 1 and 2 yields Adobe's Sharpen and Sharpen More.
    //! \param[in]  src       the source image to be sharpened.
    //! \param[out] dst       the resultant image (may be the same as the src).
    //! \param[in]  stream    the CUDA stream on which to perform the computations.
    //! \param[in]  tmp       a temporary working image. This can be NULL, but may result in lower performance.
    //!                       It is best if it resides on the same processor (CPU or GPU) as the destination.
    //! @return     NVCV_SUCCESS          if the operation completed successfully.
    //!             NVCV_ERR_MISMATCH     if the source and destination formats are different.
    //!             NVCV_ERR_PIXELFORMAT  if the function has not been implemented for the chosen pixel type.
    // NvCV_Status NvCV_API NvCVImage_Sharpen(float sharpness, const NvCVImage *src, NvCVImage *dst,
    //   struct CUstream_st *stream, NvCVImage* tmp);

    // NvCV_Status NvCV_API NvCVImage_InitFromD3D11Texture(NvCVImage* im, struct ID3D11Texture2D *tx);
    /// <summary>
    /// Initialize an NvCVImage from a D3D11 texture.
    /// The pixelFormat and component types with be transferred over, and a cudaGraphicsResource will be registered;
    /// the NvCVImage destructor will unregister the resource.
    /// It is necessary to call NvCVImage_MapResource() after rendering D3D and before calling  NvCVImage_Transfer(),
    /// and to call NvCVImage_UnmapResource() before rendering in D3D again.
    /// </summary>
    /// <param name="im">the image to be initialized.</param>
    /// <param name="d3d11Texture2D">the texture to be used for initialization. Pointer to ID3D11Texture2D.</param>
    /// <returns>NVCV_SUCCESS if successful.</returns>
    /// <remarks>
    /// This is an experimental API. If you find it useful, please respond to XXX@YYY.com,
    /// otherwise we may drop support.
    /// </remarks>
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(InitFromD3D11Texture), CallingConvention = CALLING_CONVENTION)]
    public static extern Status InitFromD3D11Texture(ImageStruct* im, IntPtr d3d11Texture2D);

    // NvCV_Status NvCV_API NvCVImage_ToD3DFormat(NvCVImage_PixelFormat format, NvCVImage_ComponentType type, unsigned layout, DXGI_FORMAT* d3dFormat);
    /// <summary>
    /// Utility to determine the D3D format from the NvCVImage format, type and layout.
    /// </summary>
    /// <param name="pixelFormat">the pixel format.</param>
    /// <param name="componentType">the component type.</param>
    /// <param name="imageLayout">the layout.</param>
    /// <param name="dxgiFormat">a place to store the corresponding D3D format (the actual type is DXGI_FORMAT enumeration).</param>
    /// <returns>NVCV_SUCCESS if successful.</returns>
    /// <remarks>
    /// This is an experimental API. If you find it useful, please respond to XXX@YYY.com, otherwise we may drop support.
    /// </remarks>
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(ToD3DFormat), CallingConvention = CALLING_CONVENTION)]
    public static extern Status ToD3DFormat(ImagePixelFormat pixelFormat, ImageComponentType componentType, ImageLayout imageLayout,
        out int dxgiFormat);

    // NvCV_Status NvCV_API NvCVImage_FromD3DFormat(DXGI_FORMAT d3dFormat, NvCVImage_PixelFormat* format, NvCVImage_ComponentType* type, unsigned char* layout);
    /// <summary>
    /// Utility to determine the NvCVImage format, component type and layout from a D3D format.
    /// </summary>
    /// <param name="dxgiFormat">the D3D format to translate (the actual type is DXGI_FORMAT enumeration).</param>
    /// <param name="pixelFormat">a place to store the NvCVImage pixel format.</param>
    /// <param name="componentType">a place to store the NvCVImage component type.</param>
    /// <param name="imageLayout">a place to store the NvCVImage layout.</param>
    /// <returns></returns>
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(FromD3DFormat), CallingConvention = CALLING_CONVENTION)]
    public static extern Status FromD3DFormat(int dxgiFormat,
        out ImagePixelFormat pixelFormat, out ImageComponentType componentType, out ImageLayout imageLayout);
}
