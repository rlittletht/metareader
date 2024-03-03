using Jp2000.Boxes;
using Jp2000.Directories.Records;

namespace Jp2000.Directories;


public class Jp2Header_Resolution_CaptureResolution : DirectoryBase, IDirectory
{
    private readonly IRecord[] Records =
    {
    };

    public bool Parse(IBox parent, ref long offset, Dictionary<string, string>? valueMap) => Parse(Records, parent, ref offset, valueMap);

}
