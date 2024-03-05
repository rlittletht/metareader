using System.Text;
using Jp2000;

namespace Jp2000.Directories.Records.RecordValues;

class JpInt32U : IRecordValue
{
    public UInt32? Value { get; private set; }

    public void SetFromData(ReadOnlySpan<byte> data)
    {
        Value = Reader.UInt32FromBytes(data, 0);
    }

    public bool IsEqual(ReadOnlySpan<byte> other)
    {
        if (Value == null)
            return false;

        return other[0] == Value;
    }

    public JpInt32U(ReadOnlySpan<byte> data)
    {
        SetFromData(data);
    }

    public IRecordValue Factory(ReadOnlySpan<byte> data)
    {
        return StaticFactory(data);
    }

    public static IRecordValue StaticFactory(ReadOnlySpan<byte> data)
    {
        return new JpInt32U(data);
    }

    public override string ToString()
    {
        if (Value == null)
            return "<null>";

        return $"0x{Value:x8}";
    }
}