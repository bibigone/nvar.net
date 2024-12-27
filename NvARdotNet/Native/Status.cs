namespace NvARdotNet.Native;

// typedef enum NVCV_Status {
//   ...
// } NVCV_Status;

/// <summary>Status codes returned from APIs.</summary>
internal enum Status : int
{
    SUCCESS = 0,    //!< The procedure returned successfully.
    ERR_GENERAL = -1,   //!< An otherwise unspecified error has occurred.
    ERR_UNIMPLEMENTED = -2,   //!< The requested feature is not yet implemented.
    ERR_MEMORY = -3,   //!< There is not enough memory for the requested operation.
    ERR_EFFECT = -4,   //!< An invalid effect handle has been supplied.
    ERR_SELECTOR = -5,   //!< The given parameter selector is not valid in this effect filter.
    ERR_BUFFER = -6,   //!< An image buffer has not been specified.
    ERR_PARAMETER = -7,   //!< An invalid parameter value has been supplied for this effect+selector.
    ERR_MISMATCH = -8,   //!< Some parameters are not appropriately matched.
    ERR_PIXELFORMAT = -9,   //!< The specified pixel format is not accommodated.
    ERR_MODEL = -10,  //!< Error while loading the TRT model.
    ERR_LIBRARY = -11,  //!< Error loading the dynamic library.
    ERR_INITIALIZATION = -12,  //!< The effect has not been properly initialized.
    ERR_FILE = -13,  //!< The file could not be found.
    ERR_FEATURENOTFOUND = -14,  //!< The requested feature was not found
    ERR_MISSINGINPUT = -15,  //!< A required parameter was not set
    ERR_RESOLUTION = -16,  //!< The specified image resolution is not supported.
    ERR_UNSUPPORTEDGPU = -17,  //!< The GPU is not supported
    ERR_WRONGGPU = -18,  //!< The current GPU is not the one selected.
    ERR_UNSUPPORTEDDRIVER = -19,  //!< The currently installed graphics driver is not supported
    ERR_MODELDEPENDENCIES = -20,  //!< There is no model with dependencies that match this system
    ERR_PARSE = -21,  //!< There has been a parsing or syntax error while reading a file
    ERR_MODELSUBSTITUTION = -22,  //!< The specified model does not exist and has been substituted.
    ERR_READ = -23,  //!< An error occurred while reading a file.
    ERR_WRITE = -24,  //!< An error occurred while writing a file.
    ERR_PARAMREADONLY = -25,  //!< The selected parameter is read-only.
    ERR_TRT_ENQUEUE = -26,  //!< TensorRT enqueue failed.
    ERR_TRT_BINDINGS = -27,  //!< Unexpected TensorRT bindings.
    ERR_TRT_CONTEXT = -28,  //!< An error occurred while creating a TensorRT context.
    ERR_TRT_INFER = -29,  ///< The was a problem creating the inference engine.
    ERR_TRT_ENGINE = -30,  ///< There was a problem deserializing the inference runtime engine.
    ERR_NPP = -31,  //!< An error has occurred in the NPP library.
    ERR_CONFIG = -32,  //!< No suitable model exists for the specified parameter configuration.
    ERR_TOOSMALL = -33,  //!< A supplied parameter or buffer is not large enough.
    ERR_TOOBIG = -34,  //!< A supplied parameter is too big.
    ERR_WRONGSIZE = -35,  //!< A supplied parameter is not the expected size.
    ERR_OBJECTNOTFOUND = -36,  //!< The specified object was not found.
    ERR_SINGULAR = -37,  //!< A mathematical singularity has been encountered.
    ERR_NOTHINGRENDERED = -38,  //!< Nothing was rendered in the specified region.
    ERR_CONVERGENCE = -39,  //!< An iteration did not converge satisfactorily.

    ERR_OPENGL = -98,  //!< An OpenGL error has occurred.
    ERR_DIRECT3D = -99,  //!< A Direct3D error has occurred.

    ERR_CUDA_BASE = -100,  //!< CUDA errors are offset from this value.
    ERR_CUDA_VALUE = -101,  //!< A CUDA parameter is not within the acceptable range.
    ERR_CUDA_MEMORY = -102,  //!< There is not enough CUDA memory for the requested operation.
    ERR_CUDA_PITCH = -112,  //!< A CUDA pitch is not within the acceptable range.
    ERR_CUDA_INIT = -127,  //!< The CUDA driver and runtime could not be initialized.
    ERR_CUDA_LAUNCH = -819,  //!< The CUDA kernel launch has failed.
    ERR_CUDA_KERNEL = -309,  //!< No suitable kernel image is available for the device.
    ERR_CUDA_DRIVER = -135,  //!< The installed NVIDIA CUDA driver is older than the CUDA runtime library.
    ERR_CUDA_UNSUPPORTED = -901,  //!< The CUDA operation is not supported on the current system or device.
    ERR_CUDA_ILLEGAL_ADDRESS = -800,  //!< CUDA tried to load or store on an invalid memory address.
    ERR_CUDA = -1099, //!< An otherwise unspecified CUDA error has been reported.
}
