using System.Text;

namespace Jp2000.Directories.Records.RecordValues;

public class JpRational64 : IRecordValue
{
    private Int32 m_num;
    private Int32 m_denom;

    public byte[]? Value { get; private set; }

    public void SetFromData(ReadOnlySpan<byte> data)
    {
        Value = new byte[data.Length];
        data.CopyTo(Value);

        m_num = Reader.Int32FromBytes(data, 0);
        m_denom = Reader.Int32FromBytes(data, 4);
    }

    public bool IsEqual(ReadOnlySpan<byte> data)
    {
        if (Value == null)
            return false;

        return Reader.CompareBytes(Value, data);
    }

    public JpRational64(ReadOnlySpan<byte> data)
    {
        SetFromData(data);
    }

    public IRecordValue Factory(ReadOnlySpan<byte> data)
    {
        return new JpRational64(data);
    }

    public static IRecordValue StaticFactory(ReadOnlySpan<byte> data)
    {
        return new JpRational64(data);
    }

    public override string ToString()
    {
        return $"{((double)m_num) / ((double)m_denom)}";
    }
}
