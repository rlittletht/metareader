using Jp2000;
using Jp2000.Directories;
using Jp2000.Directories.Records;

namespace Jp2000.Boxes;

public class Jp2Header_ImageHeader: BoxBase, IBox
{
    public static Dictionary<byte[], BoxFactoryDelegate> Boxes =
        new()
        {
            { Tables.BoxId_Jp2Header_ImageHeader, Jp2Header_ImageHeader.StaticFactory },
            { Array.Empty<byte>(), Unknown.StaticFactory }
        };

    private readonly byte[] ID = Tables.BoxId_Jp2Header_ImageHeader;

    ReadOnlySpan<byte> IBox.ID => ID;
    ReadOnlySpan<byte> IBox.BoxData => BoxData;

    public string Name => "ImageHeader";

    public bool Parse()
    {
        // root directory is CaptureResolution
        Directories.Jp2Header_ImageHeader? dir =
            DirectoryBase.CreateDirectory<Directories.Jp2Header_ImageHeader>(this, null, 0, BoxLength, ValueMap);

        if (dir == null)
            return false;

        return true;
    }

    public static IBox StaticFactory()
    {
        return new Jp2Header_ImageHeader();
    }
}
