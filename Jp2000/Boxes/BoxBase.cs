using System.Reflection.PortableExecutable;

namespace Jp2000.Boxes;

public class BoxBase
{
    public BoxHeader? HeaderInternal { get; set; }
    public BoxHeader Header => HeaderInternal ?? throw new Exception("no header");

    public byte[]? BoxData { get; set; }

    public long DataOffset { get; set; }
    public int BoxLength => Header?.BoxLength ?? 0;
    public long BoxLim => Header?.BoxLim ?? throw new Exception("no Header");

    public Dictionary<string, string> ValueMap { get; set; } = new Dictionary<string, string>();

    public bool Read(ReadOnlySpan<byte> data)
    {
        DataOffset = Header.BoxStart;
        BoxData = new byte[Header.BoxLength];
        data[(int)DataOffset..((int)DataOffset + Header.BoxLength)].CopyTo(BoxData);

        return true;
    }

    public bool Read(Reader reader)
    {
        DataOffset = Header.BoxStart;
        BoxData = reader.ReadAndRestore(Header.BoxLength, DataOffset);

        return true;
    }
}
