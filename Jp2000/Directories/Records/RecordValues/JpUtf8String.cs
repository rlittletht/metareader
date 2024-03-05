using System.Text;
using Jp2000;

namespace Jp2000.Directories.Records.RecordValues;

class JpUtf8String : IRecordValue
{
    public string? Value { get; private set; }

    public void SetFromData(ReadOnlySpan<byte> data)
    {
        Value = Encoding.UTF8.GetString(data);
    }

    public bool IsEqual(ReadOnlySpan<byte> other)
    {
        if (Value == null)
            return false;

        return Value == Encoding.UTF8.GetString(other);
    }

    public JpUtf8String(ReadOnlySpan<byte> data)
    {
        SetFromData(data);
    }

    public IRecordValue Factory(ReadOnlySpan<byte> data)
    {
        return StaticFactory(data);
    }

    public static IRecordValue StaticFactory(ReadOnlySpan<byte> data)
    {
        return new JpUtf8String(data);
    }

    public override string ToString()
    {
        return Value ?? "<null>";
    }
}