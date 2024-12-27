using NvARdotNet.Native;
using System;
using System.Diagnostics;
using System.Threading;

namespace NvARdotNet
{
    public sealed unsafe partial class Image : IDisposable
    {
        public static void GetComponentOffsets(ImagePixelFormat format,
            out int rOffset, out int gOffset, out int bOffset, out int aOffset, out int yOffset)
            => ImageApi.ComponentOffsets(format, out rOffset, out gOffset, out bOffset, out aOffset, out yOffset);

        public static void Transfer(Image src, Image dst, float scale, CudaStream stream, Image? tmp)
        {
            var status = ImageApi.Transfer(
                src.pImageStruct, dst.pImageStruct,
                scale, stream.NativePointer,
                tmp is not null ? tmp.pImageStruct : null);
            NvarException.ThrowIfNotSuccess(status, ImageApi.PREFIX + nameof(ImageApi.Transfer));
        }

        public static void Transfer(
            Image src, Rect2i srcRect,
            Image dst, Point2i dstPt,
            float scale, CudaStream stream, Image? tmp)
        {
            var status = ImageApi.TransferRect(
                src.pImageStruct, srcRect,
                dst.pImageStruct, dstPt,
                scale, stream.NativePointer,
                tmp is not null ? tmp.pImageStruct : null);
            NvarException.ThrowIfNotSuccess(status, ImageApi.PREFIX + nameof(ImageApi.TransferRect));
        }

        private readonly NativeBuffer.Struct<ImageStruct> imageStructBuffer = new(default);
        private readonly ImageStruct* pImageStruct;
        private volatile int disposeCount;
        private NativeBuffer.Array<byte>? pixelsBuffer;

        private Image(Action<Image> initAction)
        {
            pImageStruct = (ImageStruct*)imageStructBuffer.Pointer.ToPointer();

            initAction(this);

            Width = pImageStruct->Width;
            Height = pImageStruct->Height;
            PixelFormat = pImageStruct->PixelFormat;
            ComponentType = pImageStruct->ComponentType;
            MemorySpace = (ImageMemorySpace)pImageStruct->GpuMem;
            Layout = (ImageLayout)pImageStruct->Planar;
            Stride = pImageStruct->Pitch;
            BytesPerPixel = pImageStruct->PixelBytes;
            ComponentsPerPixel = pImageStruct->NumComponents;
            BytesPerComponent = pImageStruct->ComponentBytes;
        }

        /// <summary>
        /// Creates and allocates memory for image with specified parameters.
        /// </summary>
        /// <param name="width">The desired width  of the image, in pixels.</param>
        /// <param name="height">The desired height of the image, in pixels.</param>
        /// <param name="pixelFormat">The format of the pixels.</param>
        /// <param name="componentType">The type of the components of the pixels.</param>
        /// <param name="layout">One of <see cref="ImageLayout.Chunky"/>, <see cref="ImageLayout.Planar"/> or one of the YUV layouts.<</param>
        /// <param name="memorySpace">Location of the buffer.</param>
        /// <param name="alignment">
        /// Row byte alignment. Choose <c>0</c> or a power of 2.
        /// <c>1</c> yields no gap whatsoever between scanlines.
        /// <c>0</c> default alignment: 4 on CPU, and cudaMallocPitch's choice on GPU.
        /// Other common values are 16 or 32 for cache line size.
        /// </param>
        public Image(int width, int height, ImagePixelFormat pixelFormat, ImageComponentType componentType,
            ImageLayout layout, ImageMemorySpace memorySpace, int alignment = 0)
            : this((img) => AllocateImage(img, width, height, pixelFormat, componentType, layout, memorySpace, alignment))
        { }

        private static void AllocateImage(
            Image image, int width, int height,
            ImagePixelFormat pixelFormat, ImageComponentType componentType,
            ImageLayout layout, ImageMemorySpace memorySpace,
            int alignment)
        {
            var status = ImageApi.Alloc(image.pImageStruct, width, height, pixelFormat, componentType, layout, memorySpace, alignment);
            NvarException.ThrowIfNotSuccess(status, ImageApi.PREFIX + nameof(ImageApi.Init));

            Debug.Assert(image.pImageStruct->Width == width);
            Debug.Assert(image.pImageStruct->Height == height);
            Debug.Assert(image.pImageStruct->PixelFormat == pixelFormat);
            Debug.Assert(image.pImageStruct->ComponentType == componentType);
            Debug.Assert(image.pImageStruct->GpuMem == (int)memorySpace);
            Debug.Assert(image.pImageStruct->Planar == (int)layout);
        }

        ~Image() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (Interlocked.Increment(ref disposeCount) == 1)
            {
                ImageApi.Dealloc(pImageStruct);
                if (disposing)
                {
                    imageStructBuffer.Dispose();
                    pixelsBuffer?.Dispose();
                    pixelsBuffer = null;
                    Disposed?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public bool IsDisposed => disposeCount > 0;

        public event EventHandler? Disposed;

        private void CheckNotDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(Image));
        }

        public int Width { get; }
        public int Height { get; }
        public ImagePixelFormat PixelFormat { get; }
        public ImageComponentType ComponentType { get; }
        public ImageLayout Layout { get; }
        public ImageMemorySpace MemorySpace { get; }
        public int Stride { get; }
        public int BytesPerPixel { get; }
        public int ComponentsPerPixel { get; }
        public int BytesPerComponent { get; }

        internal IntPtr StructPointer => imageStructBuffer.Pointer;

        public NativeArrayView<byte> PixelsData
        {
            get
            {
                CheckNotDisposed();

                if (MemorySpace != ImageMemorySpace.CPU && MemorySpace != ImageMemorySpace.CPU_PINNED)
                    throw new InvalidOperationException($"Memory space {MemorySpace} cannot be access in this way.");

                if (pixelsBuffer is null
                    || pixelsBuffer.Pointer != pImageStruct->Pixels
                    || pixelsBuffer.MaxCount != (int)pImageStruct->BufferBytes)
                {
                    pixelsBuffer?.Dispose();
                    pixelsBuffer = new NativeBuffer.Array<byte>(pImageStruct->Pixels, (int)pImageStruct->BufferBytes, ownsBuffer: false);
                }

                return new(pixelsBuffer);
            }
        }

        /// <summary>
        /// Between rendering by a graphics system and Transfer by CUDA, it is necessary to map the texture resource.
        /// There is a fair amount of overhead, so its use should be minimized.
        /// Every call to <see cref="MapResource(CudaStream)"/> should be matched by a subsequent call to <see cref="UnmapResource(CudaStream)"/>.
        /// </summary>
        /// <param name="stream">the stream on which the mapping is to be performed.</param>
        /// <remarks>
        /// This is an experimental API.
        /// </remarks>
        public void MapResource(CudaStream stream)
        {
            CheckNotDisposed();
            var status = ImageApi.MapResource(pImageStruct, stream.NativePointer);
            NvarException.ThrowIfNotSuccess(status, ImageApi.PREFIX + nameof(ImageApi.MapResource));
        }

        /// <summary>
        /// After transfer by CUDA, the texture resource must be unmapped in order to be used by the graphics system again.
        /// There is a fair amount of overhead, so its use should be minimized.
        /// Every call to <see cref="UnmapResource(CudaStream)"/> should correspond to a preceding call to <see cref="MapResource(CudaStream)"/>.
        /// </summary>
        /// <param name="stream">the CUDA stream on which the mapping is to be performed.</param>
        /// <remarks>
        /// This is an experimental API.
        /// </remarks>
        public void UnmapResource(CudaStream stream)
        {
            CheckNotDisposed();
            var status = ImageApi.UnmapResource(pImageStruct, stream.NativePointer);
            NvarException.ThrowIfNotSuccess(status, ImageApi.PREFIX + nameof(ImageApi.UnmapResource));
        }
    }
}
