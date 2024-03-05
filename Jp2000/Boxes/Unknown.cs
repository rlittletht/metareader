using Jp2000;
using Jp2000.Directories;

namespace Jp2000.Boxes;

/*----------------------------------------------------------------------------
    %%Class: Unknown
    %%Qualified: Jp2000.Boxes.Unknown

    This matches all boxes. Used to skip unknown boxes
----------------------------------------------------------------------------*/
public class Unknown: BoxBase, IBox
{
    private readonly byte[] ID = { };

    ReadOnlySpan<byte> IBox.ID => ID;
    ReadOnlySpan<byte> IBox.BoxData => BoxData;

    public string Name => "Unknown";

    public new bool Read(Reader reader)
    {
        return true;
    }

    public new bool Read(ReadOnlySpan<byte> data)
    {
        return true;
    }

    public bool MatchBoxId(BoxHeader header) => true;

    public bool Parse()
    {
        return true;
    }

    public static IBox StaticFactory()
    {
        return new Unknown();
    }
}
