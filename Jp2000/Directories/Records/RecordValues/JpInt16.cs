using System.Text;
using Jp2000;

namespace Jp2000.Directories.Records.RecordValues;

class JpInt16 : IRecordValue
{
    public short? Value { get; private set; }

    public void SetFromData(ReadOnlySpan<byte> data)
    {
        Value = Reader.Int16FromBytes(data, 0);
    }



    public bool IsEqual(ReadOnlySpan<byte> other)
    {
        if (Value == null)
            return false;

        return other[0] == Value;
    }

    public JpInt16(ReadOnlySpan<byte> data)
    {
        SetFromData(data);
    }

    public IRecordValue Factory(ReadOnlySpan<byte> data)
    {
        return StaticFactory(data);
    }

    public static IRecordValue StaticFactory(ReadOnlySpan<byte> data)
    {
        return new JpInt16(data);
    }

    public override string ToString()
    {
        if (Value == null)
            return "<null>";

        return $"0x{Value:x4}";


    }
}