using System.Text;

namespace Jp2000.Directories.Records.RecordValues;

public class JpRational64U : IRecordValue
{
    private UInt32 m_num;
    private UInt32 m_denom;

    public byte[]? Value { get; private set; }

    public void SetFromData(ReadOnlySpan<byte> data)
    {
        Value = new byte[data.Length];
        data.CopyTo(Value);

        m_num = Reader.UInt32FromBytes(data, 0);
        m_denom = Reader.UInt32FromBytes(data, 4);
    }

    public bool IsEqual(ReadOnlySpan<byte> data)
    {
        if (Value == null)
            return false;

        return Reader.CompareBytes(Value, data);
    }

    public JpRational64U(ReadOnlySpan<byte> data)
    {
        SetFromData(data);
    }

    public IRecordValue Factory(ReadOnlySpan<byte> data)
    {
        return new JpRational64U(data);
    }

    public static IRecordValue StaticFactory(ReadOnlySpan<byte> data)
    {
        return new JpRational64U(data);
    }

    public override string ToString()
    {
        return $"{((double)m_num) / ((double)m_denom)}";
    }
}
