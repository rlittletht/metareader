using Jp2000;
using Jp2000.Directories;

namespace Jp2000.Boxes;

public class Jp2Header_Resolution_CaptureResolution: BoxBase, IBox
{
    private readonly byte[] ID = Tables.BoxId_Jp2Header_Resolution_CaptureResolution;

    ReadOnlySpan<byte> IBox.ID => ID;
    ReadOnlySpan<byte> IBox.BoxData => BoxData;

    public string Name => "Capture";

    public bool Parse()
    {
        // root directory is CaptureResolution
        Directories.Jp2Header_Resolution_CaptureResolution? dir = DirectoryBase.CreateDirectory<Directories.Jp2Header_Resolution_CaptureResolution>(this, null, 0, BoxLength, ValueMap);

        if (dir == null) 
            return false;

        return true;
    }

    public static IBox StaticFactory()
    {
        return new Jp2Header_Resolution_CaptureResolution();
    }
}
