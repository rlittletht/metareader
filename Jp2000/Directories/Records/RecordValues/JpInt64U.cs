using System.Text;
using Jp2000;

namespace Jp2000.Directories.Records.RecordValues;

class JpInt64U : IRecordValue
{
    public UInt64? Value { get; private set; }

    public void SetFromData(ReadOnlySpan<byte> data)
    {
        Value = Reader.UInt64FromBytes(data, 0);
    }

    public bool IsEqual(ReadOnlySpan<byte> other)
    {
        if (Value == null)
            return false;

        return other[0] == Value;
    }

    public JpInt64U(ReadOnlySpan<byte> data)
    {
        SetFromData(data);
    }

    public IRecordValue Factory(ReadOnlySpan<byte> data)
    {
        return StaticFactory(data);
    }

    public static IRecordValue StaticFactory(ReadOnlySpan<byte> data)
    {
        return new JpInt64U(data);
    }

    public override string ToString()
    {
        if (Value == null)
            return "<null>";

        return $"0x{Value:x16}";
    }
}