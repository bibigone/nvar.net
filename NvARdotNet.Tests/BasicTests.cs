namespace NvARdotNet.Tests;

[TestClass]
public class BasicTests
{
    public const string DEFAULT_SDK_BIN_PATH = @"C:\Program Files\NVIDIA Corporation\NVIDIA AR SDK";
    public const string MODELS_SUBDIR_NAME = "models";

    [AssemblyInitialize]
    public static void AssemblyInitialize(TestContext _)
    {
        var sdkBinPath = DEFAULT_SDK_BIN_PATH;

        // Try to calculate path to NvAR SDK binaries from NVAR_MODEL_DIR;
        var modelDir = Sdk.ModelDir;
        if (!string.IsNullOrEmpty(modelDir))
        {
            modelDir = modelDir.TrimEnd('\\', '/');
            if (modelDir.EndsWith(MODELS_SUBDIR_NAME, StringComparison.InvariantCultureIgnoreCase))
            {
                sdkBinPath = modelDir[..^MODELS_SUBDIR_NAME.Length];
            }
        }

        Sdk.AddPath(sdkBinPath);

        if (string.IsNullOrEmpty(modelDir))
            Sdk.ModelDir = Path.Combine(sdkBinPath, MODELS_SUBDIR_NAME);
    }

    [TestMethod]
    public void TestVersion()
    {
        var ver = Sdk.Version;
        Assert.IsNotNull(ver);
        Assert.AreEqual(0, ver.Major);
        Assert.AreEqual(8, ver.Minor);
        Assert.AreEqual(2, ver.Build);
    }

    [TestMethod]
    public void TestCudaStream()
    {
        var disposedCount = 0;
        var cudaStream = new CudaStream();
        cudaStream.Disposed += (_, __) => { disposedCount++; };

        Assert.IsFalse(cudaStream.IsDisposed);
        Assert.AreEqual(0, disposedCount);
        Assert.AreNotEqual(IntPtr.Zero, cudaStream.UnsafeNativePointer);

        cudaStream.Dispose();
        Assert.IsTrue(cudaStream.IsDisposed);
        Assert.AreEqual(1, disposedCount);

        cudaStream.Dispose();
        Assert.IsTrue(cudaStream.IsDisposed);
        Assert.AreEqual(1, disposedCount);
    }

    [TestMethod]
    public void TestImage()
    {
        var disposedCount = 0;
        var image = new Image(32, 16, ImagePixelFormat.BGR, ImageComponentType.U8, ImageLayout.Interleaved, ImageMemorySpace.CPU);
        image.Disposed += (_, __) => { disposedCount++; };

        Assert.IsFalse(image.IsDisposed);
        Assert.AreEqual(0, disposedCount);
        Assert.AreNotEqual(IntPtr.Zero, image.PixelsData.UnsafePointer);
        Assert.AreEqual(3, image.BytesPerPixel);
        Assert.AreEqual(3, image.ComponentsPerPixel);
        Assert.AreEqual(1, image.BytesPerComponent);
        Assert.IsTrue(image.PixelsData.Count >= image.Height * image.Stride);

        image.Dispose();
        Assert.IsTrue(image.IsDisposed);
        Assert.AreEqual(1, disposedCount);

        image.Dispose();
        Assert.IsTrue(image.IsDisposed);
        Assert.AreEqual(1, disposedCount);
    }
}