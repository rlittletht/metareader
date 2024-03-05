using System.Text;
using Jp2000;

namespace Jp2000.Directories.Records.RecordValues;

class Bytes : IRecordValue
{
    public byte[]? Value { get; private set; }

    public void SetFromData(ReadOnlySpan<byte> data)
    {
        Value = new byte[data.Length];
        data.CopyTo(Value);
    }

    public bool IsEqual(ReadOnlySpan<byte> other)
    {
        if (Value == null)
            return false;
        if (Value.Length != other.Length)
            return false;

        return Reader.CompareBytes(Value, other);
    }

    public Bytes(ReadOnlySpan<byte> data)
    {
        SetFromData(data);
    }

    public IRecordValue Factory(ReadOnlySpan<byte> data)
    {
        return StaticFactory(data);
    }

    public static IRecordValue StaticFactory(ReadOnlySpan<byte> data)
    {
        return new Bytes(data);
    }

    public override string ToString()
    {
        if (Value == null)
            return "<null>";

        return Encoding.UTF8.GetString(Value);
    }
}