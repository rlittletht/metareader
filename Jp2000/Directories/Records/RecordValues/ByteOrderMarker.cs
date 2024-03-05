using System.Text;
using Jp2000;

namespace Jp2000.Directories.Records.RecordValues;

public class ByteOrderMarker: IRecordValue
{
    private ByteOrder m_byteOrder = ByteOrder.LittleEndian;

    public ByteOrder ByteOrder => m_byteOrder;

    public byte[]? Value { get; private set; }

    public ByteOrder ByteOrderFromData(ReadOnlySpan<byte> data)
    {
        if (data[0] == 'M' && data[1] == 'M')
            return ByteOrder.BigEndian;
        else if (data[0] == 'I' && data[1] == 'I')
            return ByteOrder.LittleEndian;
        else
        {
            Console.WriteLine($"unknown byte order mark: {Encoding.UTF8.GetString(data[0..2])}");
            return ByteOrder.LittleEndian;
        }
    }

    public void SetFromData(ReadOnlySpan<byte> data)
    {
        Value = new byte[data.Length];
        data.CopyTo(Value);

        m_byteOrder = ByteOrderFromData(data);
    }

    public bool IsEqual(ReadOnlySpan<byte> other)
    {
        if (Value == null)
            return false;

        if (Value.Length != other.Length)
            return false;

        return Reader.CompareBytes(Value, other);
    }

    public ByteOrderMarker(ReadOnlySpan<byte> data)
    {
        SetFromData(data);
    }

    public IRecordValue Factory(ReadOnlySpan<byte> data)
    {
        return new ByteOrderMarker(data);
    }

    public static IRecordValue StaticFactory(ReadOnlySpan<byte> data)
    {
        return new ByteOrderMarker(data);
    }

    public override string ToString()
    {
        return m_byteOrder.ToString();
    }
}

