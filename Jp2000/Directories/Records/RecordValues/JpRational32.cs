using System.Text;

namespace Jp2000.Directories.Records.RecordValues;

public class JpRational32 : IRecordValue
{
    private Int16 m_num;
    private Int16 m_denom;

    public byte[]? Value { get; private set; }

    public void SetFromData(ReadOnlySpan<byte> data)
    {
        Value = new byte[data.Length];
        data.CopyTo(Value);

        m_num = Reader.Int16FromBytes(data, 0);
        m_denom = Reader.Int16FromBytes(data, 2);
    }

    public bool IsEqual(ReadOnlySpan<byte> data)
    {
        if (Value == null)
            return false;

        return Reader.CompareBytes(Value, data);
    }

    public JpRational32(ReadOnlySpan<byte> data)
    {
        SetFromData(data);
    }

    public IRecordValue Factory(ReadOnlySpan<byte> data)
    {
        return new JpRational32(data);
    }

    public static IRecordValue StaticFactory(ReadOnlySpan<byte> data)
    {
        return new JpRational32(data);
    }

    public override string ToString()
    {
        return $"{((double)m_num) / ((double)m_denom)}";
    }
}
