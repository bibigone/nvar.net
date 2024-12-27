using System;

namespace NvARdotNet;

/// <summary>Accessor to array of elements of type <typeparamref name="T"/> stored in native (unmanaged) memory.</summary>
/// <typeparam name="T">Unmanaged type of array elements.</typeparam>
public readonly struct NativeArrayView<T>
    where T : unmanaged
{
    private readonly Native.NativeBuffer.Array<T> buffer;
    private readonly int count;

    internal NativeArrayView(Native.NativeBuffer.Array<T> buffer)
        : this(buffer, buffer.MaxCount)
    { }

    internal NativeArrayView(Native.NativeBuffer.Array<T> buffer, int count)
    {
        this.buffer = buffer;
        this.count = count;
    }

    /// <summary>Count of elements in native array.</summary>
    public int Count => count;

    /// <summary>Pointer to native buffer in memory. Use with caution.</summary>
    public IntPtr UnsafePointer => buffer.Pointer;

    /// <summary>Index-based access to array elements.</summary>
    /// <param name="index">Zero-based index of element.</param>
    /// <returns>Value of element with index <paramref name="index"/>.</returns>
    public T this[int index]
    {
        get => buffer[index];
        set => buffer[index] = value;
    }

    /// <summary>Returns managed copy of underlying native array.</summary>
    /// <returns>Managed copy of all elements from underlying native buffer.</returns>
    public T[] ToArray()
    {
        var res = new T[count];
        CopyTo(res, 0, count);
        return res;
    }

#if NET6_0_OR_GREATER
    public unsafe Span<T> AsSpan()
        => new(buffer.Pointer.ToPointer(), Count);
#endif

    /// <summary>Copies elements from underlying native buffer to destination managed array <paramref name="array"/>.</summary>
    /// <param name="array">Destination managed array.</param>
    /// <param name="offset">Zero-based offset in destination array <paramref name="array"/>.</param>
    /// <param name="count">Count of elements to be copied.</param>
    public void CopyTo(T[] array, int offset, int count)
        => buffer.CopyTo(array, offset, count);
}
