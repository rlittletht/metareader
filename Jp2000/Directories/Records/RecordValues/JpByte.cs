using System.Text;
using Jp2000;

namespace Jp2000.Directories.Records.RecordValues;

class JpByte : IRecordValue
{
    public byte? Value { get; private set; }

    public void SetFromData(ReadOnlySpan<byte> data)
    {
        Value = data[0];
    }

    public bool IsEqual(ReadOnlySpan<byte> other)
    {
        if (Value == null)
            return false;

        return other[0] == Value;
    }

    public JpByte(byte data)
    {
        Value = data;
    }

    public IRecordValue Factory(ReadOnlySpan<byte> data)
    {
        return StaticFactory(data);
    }

    public static IRecordValue StaticFactory(ReadOnlySpan<byte> data)
    {
        return new JpByte(data[0]);
    }

    public override string ToString()
    {
        if (Value == null)
            return "<null>";

        return $"0x{Value:x2}";
    }
}