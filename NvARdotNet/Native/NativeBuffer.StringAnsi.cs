using System;
using System.Runtime.InteropServices;

namespace NvARdotNet.Native;

partial class NativeBuffer
{
    public sealed class StringAnsi : NativeBuffer
    {
        public StringAnsi(string? str)
            : base(Marshal.StringToHGlobalAnsi(str), ownsBuffer: true)
        { }

        public StringAnsi(IntPtr pointer, bool ownsBuffer)
            : base(pointer, ownsBuffer)
        { }

        public string? Value
            => Marshal.PtrToStringAnsi(Pointer);
    }
}
