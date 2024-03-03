using Jp2000;
using Jp2000.Directories;

namespace Jp2000.Boxes;

public class Jp2Header_Resolution: BoxBase, IBox
{
    private readonly byte[] ID = Tables.BoxId_Jp2Header_ImageHeader;

    ReadOnlySpan<byte> IBox.ID => ID;
    ReadOnlySpan<byte> IBox.BoxData => BoxData;

    public string Name => "ImageHeader";

    public bool Parse()
    {
        // root directory is FileType
//        Directories.Jp2Header_Resolution_CaptureResolution? dir = DirectoryBase.CreateDirectory<Jp2Header_Resolution>(this, null, 0, BoxLength, ValueMap);
//
//        if (dir == null) 
//            return false;
//
        return true;
    }
}
