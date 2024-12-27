using NvARdotNet.Native;
using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace NvARdotNet;

[Serializable]
public class NvarException : Exception
{
    internal static void ThrowIfNotSuccess(Status status, string nativeFuncName)
    {
        if (status == Status.SUCCESS)
            return;
        var messagePtr = ImageApi.GetErrorStringFromCode(status);
        var message = Marshal.PtrToStringAnsi(messagePtr);
        if (string.IsNullOrEmpty(message))
            message = "Unknown error with error code " + status;
        throw new NvarException(
            $"Error on call to native function {nativeFuncName}:{Environment.NewLine}{message}",
            (int)status);
    }

    public int ErrorCode { get; }

    public NvarException(string message, int errorCode) : base(message)
        => ErrorCode = errorCode;

    /// <summary>Constructor for deserialization needs.</summary>
    /// <param name="info">Serialization info.</param>
    /// <param name="context">Streaming context.</param>
    protected NvarException(SerializationInfo info, StreamingContext context) : base(info, context)
        => ErrorCode = info.GetInt32(nameof(ErrorCode));

    /// <summary>For serialization needs.</summary>
    /// <param name="info">Serialization info.</param>
    /// <param name="context">Streaming context.</param>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(ErrorCode), ErrorCode);
    }
}