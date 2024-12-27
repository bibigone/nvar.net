using NvARdotNet.Native;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace NvARdotNet;

/// <summary>
/// Base class for all features that are defined by the SDK.
/// </summary>
public abstract partial class Feature: IDisposable
{
    private readonly FeatureHandle handle;
    private volatile int disposeCount;
    private readonly LinkedList<IDisposable> toBeDisposed = new();

    private protected Feature(FeatureId featureId)
    {
        var status = PoseApi.Create(featureId, out handle);
        NvarException.ThrowIfNotSuccess(status, PoseApi.PREFIX + nameof(PoseApi.Create));
    }

    ~Feature() => Dispose(false);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        var newValue = Interlocked.Increment(ref disposeCount);
        if (newValue == 1)
        {
            PoseApi.Destroy(handle);

            if (disposing)
            {
                foreach (var item in toBeDisposed)
                    item.Dispose();
                toBeDisposed.Clear();

                IsLoaded = false;

                Disposed?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public bool IsDisposed => disposeCount > 0;

    public event EventHandler? Disposed;

    internal FeatureHandle Handle
    {
        get
        {
            CheckNotDisposed();
            return handle;
        }
    }

    public IntPtr UnsafeNativeHandle
    {
        get
        {
            CheckNotDisposed();
            return handle.NativeValue;
        }
    }

    /// <summary>String that describes the feature.</summary>
    public string Description
    {
        get
        {
            var status = PoseApi.GetString(Handle, ParameterNames.Config.FeatureDescription, out var strPtr);
            NvarException.ThrowIfNotSuccess(status, PoseApi.PREFIX + nameof(PoseApi.GetString));
            return Marshal.PtrToStringAnsi(strPtr) ?? string.Empty;
        }
    }

    #region Loading

    public void Load()
    {
        CheckNotLoaded();
        BeforeLoad();
        var status = PoseApi.Load(Handle);
        NvarException.ThrowIfNotSuccess(status, PoseApi.PREFIX + nameof(PoseApi.Load));
        AfterLoad();
        IsLoaded = true;
    }

    protected virtual void BeforeLoad()
    {
        var status = PoseApi.SetCudaStream(Handle, ParameterNames.Config.CudaStream, CudaStream.NativePointer);
        NvarException.ThrowIfNotSuccess(status, PoseApi.PREFIX + nameof(PoseApi.SetCudaStream));

        var buffer = ToBeDisposed(new NativeBuffer.StringAnsi(ModelDir));
        status = PoseApi.SetString(Handle, ParameterNames.Config.ModelDir, buffer.Pointer);
        NvarException.ThrowIfNotSuccess(status, PoseApi.PREFIX + nameof(PoseApi.SetString));

        status = PoseApi.SetU32(Handle, ParameterNames.Config.Temporal, Temporal ? uint.MaxValue : 0);
        NvarException.ThrowIfNotSuccess(status, PoseApi.PREFIX + nameof(PoseApi.SetU32));
    }

    protected virtual void AfterLoad()
    { }

    public bool IsLoaded { get; private set; }

    #endregion

    #region Running

    public void Run()
    {
        CheckLoaded();
        BeforeRun();
        var status = PoseApi.Run(Handle);
        NvarException.ThrowIfNotSuccess(status, PoseApi.PREFIX + nameof(PoseApi.Run));
        AfterRun();
    }

    protected virtual void BeforeRun()
    {
        // Check that input Image is specified and that this Image was not disposed.
        if (InputImage.IsDisposed)
            throw new ObjectDisposedException(nameof(InputImage));
    }

    protected virtual void AfterRun()
    { }

    #endregion

    #region Common Configuration Parameters

    /// <summary>
    /// The CUDA stream.
    /// Set by the user.
    /// </summary>
    public CudaStream CudaStream
    {
        get => GetConfigValue(cudaStream);
        set => SetConfigValue(ref cudaStream, value);
    }
    private CudaStream? cudaStream;

    /// <summary>
    /// String that contains the path to the folder that contains the TensorRT package files.
    /// Set by the user. Default value is <see cref="Sdk.ModelDir"/>.
    /// </summary>
    public string ModelDir
    {
        get => GetConfigValue(modelDir);
        set => SetConfigValue(ref modelDir, value);
    }
    private string? modelDir = Sdk.ModelDir;

    /// <summary>
    /// Enable/disable the temporal optimization of detected objects.
    /// Set by the user.
    /// Default value is <see langword="false"/>.
    /// </summary>
    public bool Temporal
    {
        get => GetConfigValue(temporal);
        set => SetConfigValue(ref temporal, value);
    }
    private bool? temporal = false;

    #endregion

    #region Common Input Parameters

    /// <summary>
    /// Interleaved (or chunky) 8-bit BGR input image in a CUDA buffer.
    /// To be allocated and set by the user.
    /// </summary>
    public Image InputImage
    {
        set
        {
            CheckLoaded();
            if (image != value)
            {
                var status = PoseApi.SetObject(Handle, ParameterNames.Input.Image, value.StructPointer, (uint)Marshal.SizeOf<ImageStruct>());
                NvarException.ThrowIfNotSuccess(status, PoseApi.PREFIX + nameof(PoseApi.SetObject));
                image = value;
            }
        }

        get
        {
            if (image is null)
                throw ValueMustBeSpecified(nameof(InputImage));
            return image;
        }
    }
    private Image? image;

    #endregion

    #region Common helpers

    protected void CheckNotDisposed()
    {
        if (IsDisposed)
            throw new ObjectDisposedException(nameof(CudaStream));
    }

    protected T ToBeDisposed<T>(T value) where T : IDisposable
    {
        toBeDisposed.AddLast(value);
        return value;
    }

    protected void CheckNotLoaded()
    {
        if (IsLoaded)
            throw new InvalidOperationException("The feature has been already loaded");
    }

    protected void CheckLoaded()
    {
        if (!IsLoaded)
            throw new InvalidOperationException("The feature must be loaded before");
    }

    protected T GetConfigValue<T>(T? value, [CallerMemberName] string? name = null) where T : class
    {
        CheckNotDisposed();
        return value ?? throw ValueMustBeSpecified(name);
    }

    protected T GetConfigValue<T>(T? value, [CallerMemberName] string? name = null) where T : struct
    {
        CheckNotDisposed();
        return value ?? throw ValueMustBeSpecified(name);
    }

    protected static Exception ValueMustBeSpecified(string? name)
        => new InvalidOperationException($"Value of {name} must be specified first.");

    protected void SetConfigValue<T>(ref T? field, T value)
    {
        CheckNotDisposed();
        CheckNotLoaded();
        field = value;
    }

    private protected void SetParameterObject<T>(ParameterName name, NativeBuffer.Array<T> value) where T : unmanaged
    {
        var status = PoseApi.SetObject(Handle, name, value.Pointer, (ulong)NativeBuffer.Array<T>.SizeOfElement);
        NvarException.ThrowIfNotSuccess(status, PoseApi.PREFIX + nameof(PoseApi.SetObject));
    }

    private protected void SetParameterObject<T>(ParameterName name, NativeBuffer.Struct<T> value) where T : unmanaged
    {
        var status = PoseApi.SetObject(Handle, name, value.Pointer, (ulong)NativeBuffer.Struct<T>.SizeOf);
        NvarException.ThrowIfNotSuccess(status, PoseApi.PREFIX + nameof(PoseApi.SetObject));
    }

    private protected void SetParameterF32Array(ParameterName name, NativeBuffer.Array<float> value)
    {
        var status = PoseApi.SetF32Array(Handle, name, value.Pointer, value.MaxCount);
        NvarException.ThrowIfNotSuccess(status, PoseApi.PREFIX + nameof(PoseApi.SetF32Array));
    }

    #endregion
}
