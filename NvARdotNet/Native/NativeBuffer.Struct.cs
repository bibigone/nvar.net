using System;
using System.Runtime.InteropServices;

namespace NvARdotNet.Native;

partial class NativeBuffer
{
    public sealed class Struct<T> : NativeBuffer
        where T : unmanaged
    {
        public static readonly int SizeOf = Marshal.SizeOf<T>();

        public static readonly Struct<T> Default = new(default);

        public Struct(T value)
            : base(SizeOf)
            => Value = value;

        public Struct(IntPtr pointer, bool ownsBuffer)
            : base(pointer, ownsBuffer)
        { }

        public T Value
        {
            get => Marshal.PtrToStructure<T>(Pointer);
            set => Marshal.StructureToPtr(value, Pointer, fDeleteOld: false);
        }
    }
}
