using System;
using System.Runtime.InteropServices;

namespace NvARdotNet.Native
{
    // typedef struct nvAR_Feature nvAR_Feature;
    // typedef struct nvAR_Feature * NvAR_FeatureHandle;

    /// <summary>
    /// This type defines the handle of a feature that is defined by the SDK.
    /// It is used to reference the feature at runtime when the feature is executed and must be destroyed when it is no longer required.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct FeatureHandle
    {
        /// <summary>Native handle value.</summary>
        public readonly IntPtr NativeValue;
    }
}
