using System;
using System.Runtime.InteropServices;

namespace NvARdotNet.Native;

internal sealed class ParameterName : IEquatable<ParameterName>, IEquatable<string>
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Pointer
    {
        public readonly IntPtr NativeValue;
        public Pointer(IntPtr ptr) => NativeValue = ptr;
    }

    private readonly NativeBuffer.StringAnsi buffer;

    public ParameterName(string name)
        => buffer = new NativeBuffer.StringAnsi(name);

    public override string? ToString()
        => buffer.Value;

    public override int GetHashCode()
        => ToString()?.GetHashCode() ?? 0;

    public override bool Equals(object? obj)
        => obj switch
        {
            ParameterName pn => this == pn,
            string s => this == s,
            _ => false,
        };

    public bool Equals(ParameterName? other)
        => ReferenceEquals(other, this) || Equals(other?.ToString());

    public bool Equals(string? other)
        => other is not null && other.Equals(ToString(), StringComparison.InvariantCulture);

    public static bool operator ==(ParameterName? left, ParameterName? right)
        => (left is null && right is null) || (left is not null && left.Equals(right));

    public static bool operator ==(ParameterName? left, string? right)
        => (left is null && right is null) || (left is not null && left.Equals(right));

    public static bool operator ==(string? left, ParameterName? right)
        => right == left;

    public static bool operator !=(ParameterName? left, ParameterName? right)
        => !(left == right);

    public static bool operator !=(ParameterName? left, string? right)
        => !(left == right);

    public static bool operator !=(string? left, ParameterName? right)
        => !(left == right);

    public static implicit operator string?(ParameterName? value)
        => value?.ToString();

    public static implicit operator Pointer(ParameterName value)
        => new(value.buffer.Pointer);
}
