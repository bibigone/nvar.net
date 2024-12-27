namespace NvARdotNet.Tests;

[TestClass]
public class FeatureTests
{
    [TestMethod]
    public void TestCreationAndDisposing()
    {
        var disposedCount = 0;
        var feature = new Feature.FaceBoxDetection();
        feature.Disposed += (_, __) => { disposedCount++; };

        Assert.IsFalse(feature.IsDisposed);
        Assert.AreEqual(0, disposedCount);
        Assert.AreNotEqual(IntPtr.Zero, feature.UnsafeNativeHandle);
        Assert.IsFalse(string.IsNullOrWhiteSpace(feature.Description));
        Assert.IsFalse(feature.IsLoaded);

        feature.Dispose();
        Assert.IsTrue(feature.IsDisposed);
        Assert.AreEqual(1, disposedCount);

        feature.Dispose();
        Assert.IsTrue(feature.IsDisposed);
        Assert.AreEqual(1, disposedCount);
    }

    [TestMethod]
    public void TestFaceBoxDetection()
    {
        const int MAX_BOX_COUNT = 10;
        using var feature = new Feature.FaceBoxDetection(MAX_BOX_COUNT);

        Assert.AreEqual(MAX_BOX_COUNT, feature.MaxBoundingBoxCount);
        Assert.IsFalse(string.IsNullOrWhiteSpace(feature.Description));
        Assert.AreEqual(0, feature.OutputBoundingBoxes.Count);
        Assert.AreEqual(0, feature.OutputBoundingBoxConfidences.Count);

        using var cudaStream = new CudaStream();

        feature.CudaStream = cudaStream;
        feature.Temporal = false;
        feature.Load();
        Assert.IsTrue(feature.IsLoaded);

        using var srcImage = LoadEmbededImageBgr("test_faces.jpg");
        using var featureImage = new Image(srcImage.Width, srcImage.Height, ImagePixelFormat.BGR, ImageComponentType.U8, ImageLayout.Interleaved, ImageMemorySpace.GPU);
        using var tmpImage = new Image(featureImage.Width, featureImage.Height, featureImage.PixelFormat, featureImage.ComponentType, featureImage.Layout, ImageMemorySpace.GPU);

        feature.InputImage = featureImage;
        Image.Transfer(srcImage, featureImage, 1f, cudaStream, tmpImage);

        feature.Run();

        Assert.IsTrue(feature.OutputBoundingBoxes.Count == 4);
        Assert.AreEqual(feature.OutputBoundingBoxes.Count, feature.OutputBoundingBoxConfidences.Count);
    }

    [TestMethod]
    public void TestBodyDetection()
    {
        const int MAX_BOX_COUNT = 25;
        using var feature = new Feature.BodyDetection(MAX_BOX_COUNT);

        Assert.AreEqual(MAX_BOX_COUNT, feature.MaxBoundingBoxCount);
        Assert.IsFalse(string.IsNullOrWhiteSpace(feature.Description));
        Assert.AreEqual(0, feature.OutputBoundingBoxes.Count);
        Assert.AreEqual(0, feature.OutputBoundingBoxConfidences.Count);

        using var cudaStream = new CudaStream();

        feature.CudaStream = cudaStream;
        feature.Temporal = false;
        feature.Load();
        Assert.IsTrue(feature.IsLoaded);

        using var srcImage = LoadEmbededImageBgr("test_bodies.jpg");
        using var featureImage = new Image(srcImage.Width, srcImage.Height, ImagePixelFormat.BGR, ImageComponentType.U8, ImageLayout.Interleaved, ImageMemorySpace.GPU, alignment: 1);
        using var tmpImage = new Image(featureImage.Width, featureImage.Height, featureImage.PixelFormat, featureImage.ComponentType, featureImage.Layout, ImageMemorySpace.GPU, alignment: 1);

        feature.InputImage = featureImage;
        Image.Transfer(srcImage, featureImage, 1f, cudaStream, tmpImage);

        feature.Run();

        // TODO: current version of NvAR returns only one bounding box for some reason...
        Assert.IsTrue(feature.OutputBoundingBoxes.Count > 0);
        Assert.AreEqual(feature.OutputBoundingBoxes.Count, feature.OutputBoundingBoxConfidences.Count);
    }

    [TestMethod]
    public void TestSingleBodyPoseEstimation()
    {
        using var feature = new Feature.SingleBodyPoseEstimation();

        Assert.IsFalse(string.IsNullOrWhiteSpace(feature.Description));
        Assert.AreEqual(34, feature.KeyPointCount);
        var refPose = feature.ReferencePose;
        Assert.AreEqual(34, refPose.Count);

        using var cudaStream = new CudaStream();

        feature.CudaStream = cudaStream;
        feature.Temporal = false;
        feature.Load();
        Assert.IsTrue(feature.IsLoaded);

        using var srcImage = LoadEmbededImageBgr("test_bodies.jpg");
        using var featureImage = new Image(srcImage.Width, srcImage.Height, ImagePixelFormat.BGR, ImageComponentType.U8, ImageLayout.Interleaved, ImageMemorySpace.GPU, alignment: 1);
        using var tmpImage = new Image(featureImage.Width, featureImage.Height, featureImage.PixelFormat, featureImage.ComponentType, featureImage.Layout, ImageMemorySpace.GPU, alignment: 1);

        feature.InputImage = featureImage;
        Image.Transfer(srcImage, featureImage, 1f, cudaStream, tmpImage);

        feature.Run();

        Assert.AreEqual(1, feature.OutputBodyCount);
        Assert.IsTrue(feature.OutputBoundingBox.HasValue);
        Assert.AreEqual(34, feature.OutputKeyPoints.Count);
        Assert.AreEqual(34, feature.OutputKeyPoints3D.Count);
        Assert.AreEqual(34, feature.OutputKeyPointConfidences.Count);

        var confs = feature.OutputKeyPointConfidences;
        for (var i = 0; i < confs.Count; i++)
            Assert.IsTrue(confs[i] > 0.5f && confs[i] <= 1f);

        var kps = feature.OutputKeyPoints;
        for (var i = 0; i < kps.Count; i++)
        {
            var p = kps[i];
            Assert.IsTrue(0 <= p.X && p.X < srcImage.Width);
            Assert.IsTrue(0 <= p.Y && p.Y < srcImage.Height);
        }
    }

    [TestMethod]
    public void TestMultiBodyPoseEstimation()
    {
        using var feature = new Feature.MultiBodyPoseEstimation();

        Assert.IsFalse(string.IsNullOrWhiteSpace(feature.Description));
        Assert.AreEqual(34, feature.KeyPointCount);
        var refPose = feature.ReferencePose;
        Assert.AreEqual(34, refPose.Count);

        using var cudaStream = new CudaStream();

        feature.CudaStream = cudaStream;
        feature.Temporal = false;
        feature.Load();
        Assert.IsTrue(feature.IsLoaded);

        using var srcImage = LoadEmbededImageBgr("test_bodies.jpg");
        using var featureImage = new Image(srcImage.Width, srcImage.Height, ImagePixelFormat.BGR, ImageComponentType.U8, ImageLayout.Interleaved, ImageMemorySpace.GPU, alignment: 1);
        using var tmpImage = new Image(featureImage.Width, featureImage.Height, featureImage.PixelFormat, featureImage.ComponentType, featureImage.Layout, ImageMemorySpace.GPU, alignment: 1);

        feature.InputImage = featureImage;
        Image.Transfer(srcImage, featureImage, 1f, cudaStream, tmpImage);

        feature.Run();

        Assert.IsTrue(feature.OutputBodyCount > 0);
        Assert.AreEqual(feature.OutputBodyCount, feature.OutputTrackingBoundingBoxes.Count);
        Assert.AreEqual(feature.OutputBodyCount * 34, feature.OutputKeyPoints.Count);
        Assert.AreEqual(feature.OutputBodyCount * 34, feature.OutputKeyPoints3D.Count);
        Assert.AreEqual(feature.OutputBodyCount * 34, feature.OutputKeyPointConfidences.Count);

        var confs = feature.OutputKeyPointConfidences;
        for (var i = 0; i < confs.Count; i++)
            Assert.IsTrue(confs[i] > 0.5f && confs[i] <= 1f);

        var kps = feature.OutputKeyPoints;
        for (var i = 0; i < kps.Count; i++)
        {
            var p = kps[i];
            Assert.IsTrue(0 <= p.X && p.X < srcImage.Width);
            Assert.IsTrue(0 <= p.Y && p.Y < srcImage.Height);
        }
    }

    private static Image LoadEmbededImageBgr(string fileName, string? folder = null)
    {
        var assembly = typeof(BasicTests).Assembly;
        var assemblyName = assembly.GetName().Name;
        var resourcePath = !string.IsNullOrEmpty(folder)
            ? $"{assemblyName}.{folder}.{fileName}"
            : $"{assemblyName}.{fileName}";
        using var stream = assembly.GetManifestResourceStream(resourcePath);
        using var imageSharp = SixLabors.ImageSharp.Image.Load<SixLabors.ImageSharp.PixelFormats.Bgr24>(stream);
        var res = new Image(
            imageSharp.Width, imageSharp.Height,
            ImagePixelFormat.BGR, ImageComponentType.U8, ImageLayout.Interleaved,
            ImageMemorySpace.CPU,
            alignment: 1);
        imageSharp.CopyPixelDataTo(res.PixelsData.AsSpan());
        return res;
    }
}
