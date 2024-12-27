using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace NvARdotNet.Native;

internal partial class NativeBuffer : IDisposable
{
    private readonly bool ownsBuffer;
    private readonly IntPtr pointer;
    private volatile int disposeCount;

    public NativeBuffer(int sizeBytes)
    {
        pointer = Marshal.AllocHGlobal(sizeBytes);
        ownsBuffer = true;
    }

    public NativeBuffer(IntPtr pointer, bool ownsBuffer)
    {
        this.pointer = pointer;
        this.ownsBuffer = ownsBuffer;
        if (!ownsBuffer)
            GC.SuppressFinalize(this);
    }

    ~NativeBuffer()
        => Dispose(false);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool _)
    {
        if (Interlocked.Increment(ref disposeCount) == 1 && ownsBuffer)
            Marshal.FreeHGlobal(pointer);
    }

    public bool IsDisposed => disposeCount > 0;

    public IntPtr Pointer
    {
        get
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(NativeBuffer));
            return pointer;
        }
    }
}
