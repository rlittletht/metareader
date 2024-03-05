using Jp2000.Boxes;
using Jp2000.Directories.Records;

namespace Jp2000.Directories;

public class JpRoot: DirectoryBase, IDirectory
{
    public bool Parse(IBox parent, ref long offset, Dictionary<string, IRecordValue?>? valueMap) => true;
}
