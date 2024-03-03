using System.Text;

namespace Jp2000.Directories.Records.RecordValues;

public class RationalRecord : IRecordValue
{
    private UInt16 m_num;
    private UInt16 m_denom;

    public byte[]? Value { get; private set; }

    public void SetFromData(ReadOnlySpan<byte> data)
    {
        Value = new byte[data.Length];
        data.CopyTo(Value);

        m_num = Reader.UInt16FromBytes(data, 0);
        m_denom = Reader.UInt16FromBytes(data, 2);
    }

    public bool IsEqual(ReadOnlySpan<byte> data)
    {
        if (Value == null)
            return false;

        return Reader.CompareBytes(Value, data);
    }

    public RationalRecord(ReadOnlySpan<byte> data)
    {
        SetFromData(data);
    }

    public IRecordValue Factory(ReadOnlySpan<byte> data)
    {
        return new RationalRecord(data);
    }

    public static IRecordValue StaticFactory(ReadOnlySpan<byte> data)
    {
        return new RationalRecord(data);
    }

    public override string ToString()
    {
        return $"{((double)m_num) / ((double)m_denom)}";
    }
}
