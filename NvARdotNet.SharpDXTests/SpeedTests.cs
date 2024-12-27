using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace NvARdotNet.SharpDXTests
{
    [TestClass]
    public class SpeedTests
    {
        public const string DEFAULT_SDK_BIN_PATH = @"C:\Program Files\NVIDIA Corporation\NVIDIA AR SDK";
        public const int NVIDIA_ADAPTER_INDEX = 1;

        public const int WIDTH = 1280;
        public const int HEIGHT = 720;
        public const int ITERATIONS = 1000;

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext _)
        {
            Sdk.AddPath(DEFAULT_SDK_BIN_PATH);
        }

        [TestMethod]
        public void TestTransferFromTextureToGpuSpeed()
        {
            using (var device = Helpers.CreateNvidiaDevice())
            {
                var data = Marshal.AllocHGlobal(WIDTH * HEIGHT * 4);
                using (var texture = Helpers.CreateTextureBgra(device, data, WIDTH, HEIGHT))
                using (var image = Image.D3D.TextureAsImage(texture.NativePointer))
                {
                    Assert.AreEqual(WIDTH, image.Width);
                    Assert.AreEqual(HEIGHT, image.Height);

                    using (var cudaStream = new CudaStream())
                    using (var gpuImage = new Image(WIDTH, HEIGHT, ImagePixelFormat.BGR, ImageComponentType.U8, ImageLayout.Interleaved, ImageMemorySpace.GPU, alignment: 1))
                    {
                        var sw = Stopwatch.StartNew();
                        for (var i = 0; i < ITERATIONS; i++)
                        {
                            image.MapResource(cudaStream);
                            Image.Transfer(image, gpuImage, 1f, cudaStream, null);
                            image.UnmapResource(cudaStream);
                        }
                        Console.WriteLine($"Speed = {sw.Elapsed.TotalMilliseconds / ITERATIONS} ms per frame");
                    }
                }

                Marshal.FreeHGlobal(data);
            }
        }

        [TestMethod]
        public void TestTransferFromTextureToGpuNoMappingSpeed()
        {
            using (var device = Helpers.CreateNvidiaDevice())
            {
                var data = Marshal.AllocHGlobal(WIDTH * HEIGHT * 4);
                using (var texture = Helpers.CreateTextureBgra(device, data, WIDTH, HEIGHT))
                using (var image = Image.D3D.TextureAsImage(texture.NativePointer))
                {
                    using (var cudaStream = new CudaStream())
                    using (var gpuImage = new Image(WIDTH, HEIGHT, ImagePixelFormat.BGR, ImageComponentType.U8, ImageLayout.Interleaved, ImageMemorySpace.GPU, alignment: 1))
                    {
                        var sw = Stopwatch.StartNew();
                        for (var i = 0; i < ITERATIONS; i++)
                        {
                            Image.Transfer(image, gpuImage, 1f, cudaStream, null);
                        }
                        Console.WriteLine($"Speed = {sw.Elapsed.TotalMilliseconds / ITERATIONS} ms per frame");
                    }
                }

                Marshal.FreeHGlobal(data);
            }
        }

        [TestMethod]
        public void TestTransferWithRewrappingFromTextureToGpuSpeed()
        {
            using (var device = Helpers.CreateNvidiaDevice())
            {
                var data = Marshal.AllocHGlobal(WIDTH * HEIGHT * 4);
                using (var texture = Helpers.CreateTextureBgra(device, data, WIDTH, HEIGHT))
                {
                    using (var cudaStream = new CudaStream())
                    using (var gpuImage = new Image(WIDTH, HEIGHT, ImagePixelFormat.BGR, ImageComponentType.U8, ImageLayout.Interleaved, ImageMemorySpace.GPU, alignment: 1))
                    {
                        var sw = Stopwatch.StartNew();
                        for (var i = 0; i < ITERATIONS; i++)
                        {
                            using (var image = Image.D3D.TextureAsImage(texture.NativePointer))
                            {
                                image.MapResource(cudaStream);
                                Image.Transfer(image, gpuImage, 1f, cudaStream, null);
                                image.UnmapResource(cudaStream);
                            }
                        }
                        Console.WriteLine($"Speed = {sw.Elapsed.TotalMilliseconds / ITERATIONS} ms per frame");
                    }
                }

                Marshal.FreeHGlobal(data);
            }
        }

        [TestMethod]
        public void TestTransferFromCpuToGpuSpeed()
        {
            using (var image = new Image(WIDTH, HEIGHT, ImagePixelFormat.RGB, ImageComponentType.U8, ImageLayout.Chunky, ImageMemorySpace.CPU, alignment: 1))
            using (var tmpImage = new Image(WIDTH, HEIGHT, ImagePixelFormat.BGR, ImageComponentType.U8, ImageLayout.Chunky, ImageMemorySpace.GPU, alignment: 1))
            {
                using (var cudaStream = new CudaStream())
                using (var gpuImage = new Image(WIDTH, HEIGHT, ImagePixelFormat.BGR, ImageComponentType.U8, ImageLayout.Interleaved, ImageMemorySpace.GPU, alignment: 1))
                {
                    var sw = Stopwatch.StartNew();
                    for (var i = 0; i < ITERATIONS; i++)
                    {
                        Image.Transfer(image, gpuImage, 1f, cudaStream, tmpImage);
                    }
                    Console.WriteLine($"Speed = {sw.Elapsed.TotalMilliseconds / ITERATIONS} ms per frame");
                }
            }
        }

        [TestMethod]
        public void TestTransferFromCpuToGpuNoTempBufferSpeed()
        {
            using (var image = new Image(WIDTH, HEIGHT, ImagePixelFormat.RGB, ImageComponentType.U8, ImageLayout.Chunky, ImageMemorySpace.CPU, alignment: 1))
            {
                using (var cudaStream = new CudaStream())
                using (var gpuImage = new Image(WIDTH, HEIGHT, ImagePixelFormat.BGR, ImageComponentType.U8, ImageLayout.Interleaved, ImageMemorySpace.GPU, alignment: 1))
                {
                    var sw = Stopwatch.StartNew();
                    for (var i = 0; i < ITERATIONS; i++)
                    {
                        Image.Transfer(image, gpuImage, 1f, cudaStream, null);
                    }
                    Console.WriteLine($"Speed = {sw.Elapsed.TotalMilliseconds / ITERATIONS} ms per frame");
                }
            }
        }
    }
}
