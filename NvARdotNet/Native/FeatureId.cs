using System;
using System.Runtime.InteropServices;

namespace NvARdotNet.Native;

internal sealed class FeatureId : IEquatable<FeatureId>, IEquatable<string>
{
    // typedef const char* NvAR_FeatureID;
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Pointer
    {
        public readonly IntPtr NativeValue;
        public Pointer(IntPtr value) => NativeValue = value;
    }

    private readonly NativeBuffer.StringAnsi buffer;

    public FeatureId(string name)
        => buffer = new NativeBuffer.StringAnsi(name);

    public override string? ToString()
        => buffer.Value;

    public override int GetHashCode()
        => ToString()?.GetHashCode() ?? 0;

    public override bool Equals(object? obj)
        => obj switch
        {
            FeatureId fid => this == fid,
            string s => this == s,
            _ => false,
        };

    public bool Equals(FeatureId? other)
        => ReferenceEquals(other, this) || Equals(other?.ToString());

    public bool Equals(string? other)
        => other is not null && other.Equals(ToString(), StringComparison.InvariantCulture);

    public static bool operator == (FeatureId? left, FeatureId? right)
        => (left is null && right is null) || (left is not null && left.Equals(right));

    public static bool operator ==(FeatureId? left, string? right)
        => (left is null && right is null) || (left is not null && left.Equals(right));

    public static bool operator ==(string? left, FeatureId? right)
        => right == left;

    public static bool operator !=(FeatureId? left, FeatureId? right)
        => !(left == right);

    public static bool operator !=(FeatureId? left, string? right)
        => !(left == right);

    public static bool operator !=(string? left, FeatureId? right)
        => !(left == right);

    public static implicit operator string?(FeatureId? value)
        => value?.ToString();

    public static implicit operator Pointer(FeatureId value)
        => new(value.buffer.Pointer);
}
