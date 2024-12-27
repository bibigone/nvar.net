using NvARdotNet.Native;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace NvARdotNet
{
    partial class Image
    {
        /// <summary>
        /// Group of static methods to integrate with DirectX (Windows only).
        /// </summary>
        public static class D3D
        {
            /// <summary>
            /// Initialize an NvCVImage from a D3D11 texture.
            /// The <see cref="PixelFormat"/> and <see cref="ComponentType"/> with be transferred over,
            /// and a <c>cudaGraphicsResource</c> will be registered;
            /// the <see cref="Image.Dispose"/> destructor will unregister the resource.
            /// It is necessary to call <see cref="MapResource(CudaStream)"/> after rendering D3D and before calling <see cref="Transfer(Image, Image, float, CudaStream, Image?)"/>,
            /// and to call <see cref="UnmapResource(CudaStream)"/> before rendering in D3D again.
            /// </summary>
            /// <param name="d3d11Texture2D">The texture to be used for initialization. Pointer to <c>ID3D11Texture2D</c> instance.</param>
            /// <returns>Initialized image for a specified D3D11 texture.</returns>
            /// <remarks>
            /// This is an experimental API.
            /// </remarks>
            public static Image TextureAsImage(IntPtr d3d11Texture2D)
                => new(img => InitFromD3D11Texture(img, d3d11Texture2D));

            private static unsafe void InitFromD3D11Texture(Image image, IntPtr d3d11Texture2D)
            {
                var status = ImageApi.InitFromD3D11Texture(image.pImageStruct, d3d11Texture2D);
                NvarException.ThrowIfNotSuccess(status, ImageApi.PREFIX + nameof(ImageApi.InitFromD3D11Texture));
            }

            /// <summary>
            /// Utility to determine the D3D format from the NvAR Image format, type and layout.
            /// </summary>
            /// <param name="pixelFormat">the pixel format.</param>
            /// <param name="componentType">the component type.</param>
            /// <param name="imageLayout">the layout.</param>
            /// <returns>Corresponding D3D format (the actual type is <c>DXGI_FORMAT</c> enumeration).</returns>
            public static int ToDxgiFormat(ImagePixelFormat pixelFormat, ImageComponentType componentType, ImageLayout imageLayout)
            {
                var status = ImageApi.ToD3DFormat(pixelFormat, componentType, imageLayout, out var dxgiFormat);
                NvarException.ThrowIfNotSuccess(status, ImageApi.PREFIX + nameof(ImageApi.ToD3DFormat));
                return dxgiFormat;
            }

            /// <summary>
            /// Utility to determine the NvAR image format, component type and layout from a D3D format.
            /// </summary>
            /// <param name="dxgiFormat">D3D format (the actual type is <c>DXGI_FORMAT</c> enumeration).</param>
            /// <param name="pixelFormat">a place to store the NvAR image pixel format.</param>
            /// <param name="componentType">a place to store the NvAR image component type.</param>
            /// <param name="imageLayout">a place to store the NvAR image layout.</param>
            public static void FromDxgiFormat(int dxgiFormat, out ImagePixelFormat pixelFormat, out ImageComponentType componentType, out ImageLayout imageLayout)
            {
                var status = ImageApi.FromD3DFormat(dxgiFormat, out pixelFormat, out componentType, out imageLayout);
                NvarException.ThrowIfNotSuccess(status, ImageApi.PREFIX + nameof(ImageApi.FromD3DFormat));
            }
        }
    }
}
