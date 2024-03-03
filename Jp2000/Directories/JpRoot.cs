using Jp2000.Boxes;

namespace Jp2000.Directories;

public class JpRoot: DirectoryBase, IDirectory
{
    public bool Parse(IBox parent, ref long offset, Dictionary<string, string>? valueMap) => true;
}
