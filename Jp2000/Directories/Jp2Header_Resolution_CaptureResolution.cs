using Jp2000.Boxes;
using Jp2000.Directories.Records;
using Jp2000.Directories.Records.RecordValues;

namespace Jp2000.Directories;


public class Jp2Header_Resolution_CaptureResolution : DirectoryBase, IDirectory
{
    private readonly IRecord[] Records =
    {
        new Record(
            "ResY",
            RecordType.Byte,
            new RecordLength(4),
            RationalRecord.StaticFactory),
        new Record(
            "ResX",
            RecordType.Byte,
            new RecordLength(4),
            RationalRecord.StaticFactory),
    };

    public bool Parse(IBox parent, ref long offset, Dictionary<string, string>? valueMap) => Parse(Records, parent, ref offset, valueMap);

}
