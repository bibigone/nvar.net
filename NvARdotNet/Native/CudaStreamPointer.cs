using System;
using System.Runtime.InteropServices;

namespace NvARdotNet.Native
{
    // struct CUstream_st;
    // typedef struct CUstream_st * CUstream;
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct CudaStreamPointer
    {
        public readonly IntPtr NativeValue;
    }
}
