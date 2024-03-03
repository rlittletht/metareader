using System.Text;
using Jp2000;

namespace Jp2000.Boxes;

public class BoxHeader
{
    private byte[]? m_data;
    private byte[] m_id = null!;
    private int m_headerLength = 0;
    private long m_boxLength = 0;

    private static readonly int BoxIdSize = 4;

    public ReadOnlySpan<byte> ID => m_id;
    public int HeaderLength => m_headerLength;
    public long BoxLengthLong => m_boxLength;
    public long HeaderStart;
    public long BoxStart => HeaderStart + HeaderLength;
    public long BoxLim => BoxStart + BoxLength;

    public int BoxLength
    {
        get
        {
            if (m_boxLength > int.MaxValue)
                throw new Exception("Can't operate on boxes > 2gb in length");

            return (int)m_boxLength;
        }
    }

    public bool MatchesID(ReadOnlySpan<byte> id)
    {
        if (id.Length == 0)
            return true;

        return Reader.CompareBytes(ID, id);
    }

    void SetFromData(ReadOnlySpan<byte> data, int start, int maxLength)
    {
        m_boxLength = Reader.LongFromBytes(data, start);
        m_headerLength = 8;
        m_id = new byte[BoxIdSize];
        data[(start + 4)..(start + 4 + BoxIdSize)].CopyTo(m_id);
        if (m_boxLength == 1)
        {
            m_boxLength = Reader.LongLongFromBytes(data, start + 8);
            m_headerLength += 8;
        }

        if (m_boxLength == 0)
        {
            m_boxLength = maxLength - start;
        }

        m_boxLength -= HeaderLength;
    }

    public BoxHeader(ReadOnlySpan<byte> data, int start, int maxLength)
    {
        HeaderStart = start;
        SetFromData(data, start, maxLength);
    }

    public BoxHeader(Reader reader, long start)
    {
        reader.DoAndRestore(
            () =>
            {
                reader.Position = start;
                m_data = reader.Read(8);
                if (Reader.ShortFromBytes(m_data, 0) == 1)
                {
                    // data has to be 12 bytes because there's a 4byte box length following the 8 bytes
                    reader.Position = start;
                    m_data = reader.Read(16);
                }
                
                reader.Position = start;
                HeaderStart = start;

                SetFromData(m_data, 0, (int)reader.Length);

//                m_id = reader.Read(BoxIdSize) ?? throw new Exception("can't read box id");
//
//                if (m_boxLength == 1)
//                    m_boxLength = reader.ReadLong() ?? throw new Exception("Could not read extended length");
//
//                m_headerLength = (int)(reader.Position - start);
//                BoxStart = reader.Position;
//                if (m_boxLength == 0)
//                    m_boxLength = reader.Length - BoxStart;
//                else
//                    m_boxLength -= m_headerLength;
//
//                if (m_boxLength < 0)
//                    throw new Exception($"Invalid box header length for box at {start}");
            });
    }

    public string Describe()
    {
        return $"Box: {Encoding.UTF8.GetString(ID)}: Header: {HeaderStart:x8}:{HeaderStart + HeaderLength:x8}, Box: {BoxStart:x8}:{BoxStart + BoxLength:x8}";
    }
}
