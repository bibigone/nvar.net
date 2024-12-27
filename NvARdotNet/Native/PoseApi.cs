using System;
using System.Runtime.InteropServices;

namespace NvARdotNet.Native;

/// <summary>See nvAR.h from native SDK.</summary>
internal static unsafe class PoseApi
{
    public const string LIB_NAME = "nvARPose";
    public const string PREFIX = "NvAR_";
    public const CallingConvention CALLING_CONVENTION = CallingConvention.Cdecl;

    // NvCV_Status NvAR_API NvAR_GetVersion(unsigned int *version);
    /// <summary>Get the SDK version.</summary>
    /// <param name="version">
    /// Pointer to an unsigned int set to 
    /// (major << 24) | (minor << 16) | (build << 8) | 0
    /// </param>
    /// <returns>NVCV_SUCCESS  if the version was set. NVCV_ERR_PARAMETER if version was NULL.</returns>
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(GetVersion), CallingConvention = CALLING_CONVENTION)]
    public static extern Status GetVersion(out uint version);

    #region Features management

    // NvCV_Status NvAR_API NvAR_Create(NvAR_FeatureID featureID, NvAR_FeatureHandle* handle);
    /// <summary>Create a new feature instantiation.</summary>
    /// <param name="featureId">The selector code for the desired feature.</param>
    /// <param name="featureHandle">Handle to the feature instance.</param>
    /// <returns></returns>
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(Create), CallingConvention = CALLING_CONVENTION)]
    public static extern Status Create(FeatureId.Pointer featureId, out FeatureHandle featureHandle);

    // NvCV_Status NvAR_API NvAR_Load(NvAR_FeatureHandle handle);
    /// <summary>Load the model based on the set params.</summary>
    /// <param name="featureHandle">Handle to the feature instance.</param>
    /// <returns></returns>
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(Load), CallingConvention = CALLING_CONVENTION)]
    public static extern Status Load(FeatureHandle featureHandle);

    // NvCV_Status NvAR_API NvAR_Run(NvAR_FeatureHandle handle);
    /// <summary>Run the selected feature instance.</summary>
    /// <param name="featureHandle">Handle to the feature instance.</param>
    /// <returns></returns>
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(Run), CallingConvention = CALLING_CONVENTION)]
    public static extern Status Run(FeatureHandle featureHandle);

    // NvCV_Status NvAR_API NvAR_Destroy(NvAR_FeatureHandle handle);
    /// <summary>Delete a previously created feature instance.</summary>
    /// <param name="featureHandle">Handle to the feature instance.</param>
    /// <returns></returns>
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(Destroy), CallingConvention = CALLING_CONVENTION)]
    public static extern Status Destroy(FeatureHandle featureHandle);

    #endregion

    #region Integration with CUDA

    // NvCV_Status NvAR_API NvAR_CudaStreamCreate(CUstream* stream);
    /// <summary>Wrapper for cudaStreamCreate(), if it is desired to avoid linking with the cuda lib.</summary>
    /// <param name="stream">A place to store the newly allocated stream.</param>
    /// <returns></returns>
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(CudaStreamCreate), CallingConvention = CALLING_CONVENTION)]
    public static extern Status CudaStreamCreate(out CudaStreamPointer stream);

    // NvCV_Status NvAR_API NvAR_CudaStreamDestroy(CUstream stream);
    /// <summary>Wrapper for cudaStreamDestroy(), if it is desired to avoid linking with the cuda lib.</summary>
    /// <param name="stream">The stream to destroy.</param>
    /// <returns></returns>
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(CudaStreamCreate), CallingConvention = CALLING_CONVENTION)]
    public static extern Status CudaStreamDestroy(CudaStreamPointer stream);

    #endregion

    #region Setters for property values

    // NvCV_Status NvAR_API NvAR_SetU32(NvAR_FeatureHandle handle, const char* name, unsigned int val);
    /// <summary>Set the value of the selected parameter.</summary>
    /// <param name="featureHandle">Handle to the feature instance.</param>
    /// <param name="name">The selector of the feature parameter to configure.</param>
    /// <param name="val">The value to be assigned to the selected feature parameter.</param>
    /// <returns></returns>
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(SetU32), CallingConvention = CALLING_CONVENTION)]
    public static extern Status SetU32(FeatureHandle featureHandle, ParameterName.Pointer name, uint val);

    // NvCV_Status NvAR_API NvAR_SetS32(NvAR_FeatureHandle handle, const char* name, int val);
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(SetS32), CallingConvention = CALLING_CONVENTION)]
    public static extern Status SetS32(FeatureHandle featureHandle, ParameterName.Pointer name, int val);

    // NvCV_Status NvAR_API NvAR_SetF32(NvAR_FeatureHandle handle, const char* name, float val);
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(SetF32), CallingConvention = CALLING_CONVENTION)]
    public static extern Status SetF32(FeatureHandle featureHandle, ParameterName.Pointer name, float val);

    // NvCV_Status NvAR_API NvAR_SetF64(NvAR_FeatureHandle handle, const char* name, double val);
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(SetF64), CallingConvention = CALLING_CONVENTION)]
    public static extern Status SetF64(FeatureHandle featureHandle, ParameterName.Pointer name, double val);

    // NvCV_Status NvAR_API NvAR_SetU64(NvAR_FeatureHandle handle, const char* name, unsigned long long val);
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(SetU64), CallingConvention = CALLING_CONVENTION)]
    public static extern Status SetU64(FeatureHandle featureHandle, ParameterName.Pointer name, ulong val);

    // NvCV_Status NvAR_API NvAR_SetObject(NvAR_FeatureHandle handle, const char* name, void* ptr, unsigned long typeSize);
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(SetObject), CallingConvention = CALLING_CONVENTION)]
    public static extern Status SetObject(FeatureHandle featureHandle, ParameterName.Pointer name, IntPtr ptr, ulong typeSize);

    // NvCV_Status NvAR_API NvAR_SetString(NvAR_FeatureHandle handle, const char* name, const char* str);
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(SetString), CallingConvention = CALLING_CONVENTION)]
    public static extern Status SetString(FeatureHandle featureHandle, ParameterName.Pointer name, IntPtr str);

    // NvCV_Status NvAR_API NvAR_SetCudaStream(NvAR_FeatureHandle handle, const char* name, CUstream stream);
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(SetCudaStream), CallingConvention = CALLING_CONVENTION)]
    public static extern Status SetCudaStream(FeatureHandle featureHandle, ParameterName.Pointer name, CudaStreamPointer stream);

    // NvCV_Status NvAR_API NvAR_SetF32Array(NvAR_FeatureHandle handle, const char* name, float* vals, int /*count*/);
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(SetF32Array), CallingConvention = CALLING_CONVENTION)]
    public static extern Status SetF32Array(FeatureHandle featureHandle, ParameterName.Pointer name, IntPtr vals, int count);

    #endregion

    #region Getters for property values

    // NvCV_Status NvAR_API NvAR_GetU32(NvAR_FeatureHandle handle, const char* name, unsigned int* val);
    /// <summary>Get the value of the selected parameter.</summary>
    /// <param name="featureHandle">Handle to the feature instance.</param>
    /// <param name="name">The selector of the feature parameter to retrieve.</param>
    /// <param name="val">Place to store the retrieved parameter.</param>
    /// <returns></returns>
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(GetU32), CallingConvention = CALLING_CONVENTION)]
    public static extern Status GetU32(FeatureHandle featureHandle, ParameterName.Pointer name, out uint val);

    // NvCV_Status NvAR_API NvAR_GetS32(NvAR_FeatureHandle handle, const char* name, int* val);
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(GetS32), CallingConvention = CALLING_CONVENTION)]
    public static extern Status GetS32(FeatureHandle featureHandle, ParameterName.Pointer name, out int val);

    // NvCV_Status NvAR_API NvAR_GetF32(NvAR_FeatureHandle handle, const char* name, float* val);
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(GetF32), CallingConvention = CALLING_CONVENTION)]
    public static extern Status GetF32(FeatureHandle featureHandle, ParameterName.Pointer name, out float val);

    // NvCV_Status NvAR_API NvAR_GetF64(NvAR_FeatureHandle handle, const char* name, double* val);
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(GetF64), CallingConvention = CALLING_CONVENTION)]
    public static extern Status GetF64(FeatureHandle featureHandle, ParameterName.Pointer name, out double val);

    // NvCV_Status NvAR_API NvAR_GetU64(NvAR_FeatureHandle handle, const char* name, unsigned long long* val);
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(GetU64), CallingConvention = CALLING_CONVENTION)]
    public static extern Status GetU64(FeatureHandle featureHandle, ParameterName.Pointer name, out ulong val);

    // NvCV_Status NvAR_API NvAR_GetObject(NvAR_FeatureHandle handle, const char* name, const void** ptr,
    //                                     unsigned long typeSize);
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(GetObject), CallingConvention = CALLING_CONVENTION)]
    public static extern Status GetObject(FeatureHandle featureHandle, ParameterName.Pointer name, out IntPtr ptr, ulong typeSize);

    // NvCV_Status NvAR_API NvAR_GetString(NvAR_FeatureHandle handle, const char* name, const char** str);
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(GetString), CallingConvention = CALLING_CONVENTION)]
    public static extern Status GetString(FeatureHandle featureHandle, ParameterName.Pointer name, out IntPtr str);

    // NvCV_Status NvAR_API NvAR_GetCudaStream(NvAR_FeatureHandle handle, const char* name, const CUstream* stream);
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(GetCudaStream), CallingConvention = CALLING_CONVENTION)]
    public static extern Status GetCudaStream(FeatureHandle featureHandle, ParameterName.Pointer name, out CudaStreamPointer stream);

    // NvCV_Status NvAR_API NvAR_GetF32Array(NvAR_FeatureHandle handle, const char* name, const float** vals, int* /*count*/);
    [DllImport(LIB_NAME, EntryPoint = PREFIX + nameof(GetF32Array), CallingConvention = CALLING_CONVENTION)]
    public static extern Status GetF32Array(FeatureHandle featureHandle, ParameterName.Pointer name, out IntPtr vals, out int count);

    #endregion
}
