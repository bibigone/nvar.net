using System;
using System.Runtime.InteropServices;

namespace NvARdotNet.Native;

internal partial class NativeBuffer
{
    public sealed class Array<T> : NativeBuffer
        where T : struct
    {
        public static readonly int SizeOfElement = Marshal.SizeOf<T>();

        public static readonly Array<T> Empty = new(0);

        public Array(int maxCount)
            : base(SizeOfElement * maxCount)
        {
            MaxCount = maxCount;
        }

        public Array(T[] arr)
            : this(arr.Length)
        {
            for (var i = 0; i < arr.Length; i++)
                this[i] = arr[i];
        }

        public Array(IntPtr ptr, int maxCount, bool ownsBuffer)
            : base(ptr, ownsBuffer)
        {
            MaxCount = maxCount;
        }

        public int MaxCount { get; }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= MaxCount)
                    throw new ArgumentOutOfRangeException(nameof(index));
                return Marshal.PtrToStructure<T>(Pointer + SizeOfElement * index);
            }

            set
            {
                if (index < 0 || index >= MaxCount)
                    throw new ArgumentOutOfRangeException(nameof(index));
                Marshal.StructureToPtr<T>(value, Pointer + SizeOfElement * index, fDeleteOld: false);
            }
        }

        public void CopyTo(T[] array, int offset, int count)
        {
            if (offset < 0 || offset >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (count < 0 || offset + count > array.Length || count > MaxCount)
                throw new ArgumentOutOfRangeException(nameof(count));

            for (var i = 0; i < count; i++)
                array[offset + i] = this[i];
        }
    }
}
