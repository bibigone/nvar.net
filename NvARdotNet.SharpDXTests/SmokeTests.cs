using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Runtime.InteropServices;

namespace NvARdotNet.SharpDXTests
{
    [TestClass]
    public class SmokeTests
    {
        [TestMethod]
        public void TestToFromDxgiFormat()
        {
            var srcFormat = ImagePixelFormat.RGBA;
            var srcComponentType = ImageComponentType.U8;
            var srcLayout = ImageLayout.Interleaved;

            var dxgiFormat = Image.D3D.ToDxgiFormat(srcFormat, srcComponentType, srcLayout);
            Console.WriteLine(dxgiFormat);

            Image.D3D.FromDxgiFormat(dxgiFormat, out var dstFormat, out var dstComponentType, out var dstLayout);
            Assert.AreEqual(srcFormat, dstFormat);
            Assert.AreEqual(srcComponentType, dstComponentType);
            Assert.AreEqual(srcLayout, dstLayout);
        }

        [TestMethod]
        public void TestFaceDetectionOnBlankTexture()
        {
            int width = 640;
            int height = 480;

            using (var device = Helpers.CreateNvidiaDevice())
            {
                var data = Marshal.AllocHGlobal(width * height * 4);
                for (var i = 0; i < width * height; i++)
                    Marshal.WriteInt32(data + i * 4, 0);

                using (var texture = Helpers.CreateTextureBgra(device, data, width, height))
                using (var image = Image.D3D.TextureAsImage(texture.NativePointer))
                using (var cudaStream = new CudaStream())
                using (var gpuImage = new Image(width, height, ImagePixelFormat.BGR, ImageComponentType.U8, ImageLayout.Interleaved, ImageMemorySpace.GPU, alignment: 1))
                using (var fd = new Feature.FaceBoxDetection())
                {
                    fd.Temporal = false;
                    fd.CudaStream = cudaStream;
                    fd.Load();

                    fd.InputImage = gpuImage;

                    image.MapResource(cudaStream);
                    Image.Transfer(image, gpuImage, 1f, cudaStream, null);
                    image.UnmapResource(cudaStream);

                    fd.Run();

                    Assert.AreEqual(0, fd.OutputBoundingBoxes.Count);
                }
                Marshal.FreeHGlobal(data);
            }
        }

        [TestMethod]
        public void TestFaceDetectionOnLoadedTexture()
        {
            using (var loadedImage = Helpers.LoadEmbededImageBgra("test_faces.jpg"))
            using (var device = Helpers.CreateNvidiaDevice())
            {
                using (var texture = Helpers.CreateTextureBgra(device, loadedImage.PixelsData.UnsafePointer, loadedImage.Width, loadedImage.Height))
                using (var image = Image.D3D.TextureAsImage(texture.NativePointer))
                using (var cudaStream = new CudaStream())
                using (var gpuImage = new Image(loadedImage.Width, loadedImage.Height, ImagePixelFormat.BGR, ImageComponentType.U8, ImageLayout.Interleaved, ImageMemorySpace.GPU, alignment: 1))
                using (var fd = new Feature.FaceBoxDetection())
                {
                    fd.Temporal = false;
                    fd.CudaStream = cudaStream;
                    fd.Load();

                    fd.InputImage = gpuImage;

                    image.MapResource(cudaStream);
                    Image.Transfer(image, gpuImage, 1f, cudaStream, null);
                    image.UnmapResource(cudaStream);

                    fd.Run();

                    Assert.AreEqual(4, fd.OutputBoundingBoxes.Count);
                }
            }
        }

    }
}
