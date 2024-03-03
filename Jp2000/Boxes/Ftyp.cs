using Jp2000;
using Jp2000.Directories;

namespace Jp2000.Boxes;

public class Ftyp: BoxBase, IBox
{
    private readonly byte[] ID = Tables.BoxId_Ftyp;

    ReadOnlySpan<byte> IBox.ID => ID;
    ReadOnlySpan<byte> IBox.BoxData => BoxData;

    public string Name => "FileType";

    public bool Parse()
    {
        // root directory is FileType
        Directories.FileType? dir = DirectoryBase.CreateDirectory<FileType>(this, null, 0, BoxLength, ValueMap);

        if (dir == null) 
            return false;

        return true;
    }
}
