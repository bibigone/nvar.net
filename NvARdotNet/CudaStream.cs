using NvARdotNet.Native;
using System;
using System.Threading;

namespace NvARdotNet;

public sealed class CudaStream : IDisposable
{
    private readonly CudaStreamPointer pointer;
    private volatile int disposeCount;

    public CudaStream()
    {
        var status = PoseApi.CudaStreamCreate(out pointer);
        NvarException.ThrowIfNotSuccess(status, PoseApi.PREFIX + nameof(PoseApi.CudaStreamCreate));
    }

    ~CudaStream() => Dispose(false);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        var newValue = Interlocked.Increment(ref disposeCount);
        if (newValue == 1)
        {
            PoseApi.CudaStreamDestroy(pointer);
            if (disposing)
                Disposed?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsDisposed => disposeCount > 0;

    public event EventHandler? Disposed;

    private void CheckNotDisposed()
    {
        if (IsDisposed)
            throw new ObjectDisposedException(nameof(CudaStream));
    }

    internal CudaStreamPointer NativePointer
    {
        get
        {
            CheckNotDisposed();
            return pointer;
        }
    }

    public IntPtr UnsafeNativePointer
    {
        get
        {
            CheckNotDisposed();
            return pointer.NativeValue;
        }
    }
}
