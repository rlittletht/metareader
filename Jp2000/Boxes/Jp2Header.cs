using Jp2000;
using Jp2000.Directories;

namespace Jp2000.Boxes;

public class Jp2Header: BoxBase, IBox
{
    public static IBox[] Boxes =
        new IBox[]
        {
            new Ftyp(),
            new Jp2Header(),
            new Unknown()
        };

    private readonly byte[] ID = Tables.BoxId_Jp2Header;

    ReadOnlySpan<byte> IBox.ID => ID;
    ReadOnlySpan<byte> IBox.BoxData => BoxData;

    public string Name => "Jp2Header";

    public bool Parse()
    {
        // This box has several sub-boxes

        Dictionary<string, Dictionary<string, string>> valuesMaps = new();

//        Box.ReadBoxesInRange(Header.BoxStart,)
        // root directory is FileType
        Directories.FileType? dir = DirectoryBase.CreateDirectory<FileType>(this, null, 0, BoxLength, ValueMap);

        if (dir == null) 
            return false;

        return true;
    }
}
