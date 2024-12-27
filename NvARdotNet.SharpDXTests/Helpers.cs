using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;

namespace NvARdotNet.SharpDXTests
{
    internal static class Helpers
    {
        private static Adapter GetFirstNvidiaAdapter()
        {
            using (var factory = new Factory1())
            {
                var adapters = factory.Adapters;
                foreach (var adapter in adapters)
                {
                    if (adapter.Description.VendorId == 0x10DE /* NVIDIA */)
                        return adapter;
                }
            }

            Assert.Fail("Cannot find NVIDIA graphics adapter");
            return null;
        }

        public static SharpDX.Direct3D11.Device CreateNvidiaDevice()
        {
            var adapter = GetFirstNvidiaAdapter();
            FeatureLevel[] levels = new FeatureLevel[]
            {
                FeatureLevel.Level_11_1,
                FeatureLevel.Level_11_0,
                FeatureLevel.Level_10_1,
                FeatureLevel.Level_10_0,
                FeatureLevel.Level_9_3
            };
            var flags = DeviceCreationFlags.BgraSupport;

            return new SharpDX.Direct3D11.Device(adapter, flags, levels);
        }

        public static Texture2D CreateTextureBgra(SharpDX.Direct3D11.Device device, IntPtr data, int width, int height)
            => new Texture2D(device, new Texture2DDescription()
            {
                Width = width,
                Height = height,
                ArraySize = 1,
                BindFlags = BindFlags.ShaderResource,
                Usage = ResourceUsage.Immutable,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = Format.B8G8R8X8_UNorm,
                MipLevels = 1,
                OptionFlags = ResourceOptionFlags.None,
                SampleDescription = new SampleDescription(1, 0),
            }, new DataRectangle(data, width * 4));

        public static unsafe Image LoadEmbededImageBgra(string fileName, string folder = null)
        {
            var assembly = typeof(Helpers).Assembly;
            var assemblyName = assembly.GetName().Name;
            var resourcePath = !string.IsNullOrEmpty(folder)
                ? $"{assemblyName}.{folder}.{fileName}"
                : $"{assemblyName}.{fileName}";
            using (var stream = assembly.GetManifestResourceStream(resourcePath))
            using (var imageSharp = SixLabors.ImageSharp.Image.Load<SixLabors.ImageSharp.PixelFormats.Bgra32>(stream))
            {
                var res = new Image(
                    imageSharp.Width, imageSharp.Height,
                    ImagePixelFormat.BGRA, ImageComponentType.U8, ImageLayout.Interleaved,
                    ImageMemorySpace.CPU,
                    alignment: 1);
                var dstSpan = new Span<byte>(res.PixelsData.UnsafePointer.ToPointer(), res.PixelsData.Count);
                imageSharp.CopyPixelDataTo(dstSpan);
                return res;
            }
        }
    }
}
