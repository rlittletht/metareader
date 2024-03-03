using System.Reflection.PortableExecutable;

namespace Jp2000.Boxes;

public interface IBox
{
    public BoxHeader? HeaderInternal { get; set; }
    public BoxHeader Header { get; }
    public ReadOnlySpan<byte> ID { get; }
    public ReadOnlySpan<byte> BoxData { get; }
    public string Name { get; }

    public long DataOffset { get; set; }
    public int BoxLength { get; }
    public long BoxLim { get; }

    public bool Read(Reader reader);
    public bool Parse();
    public Dictionary<string, string> ValueMap { get; }
}
