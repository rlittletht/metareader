using Jp2000.Boxes;
using Jp2000.Directories.Records;

namespace Jp2000.Directories;

public interface IDirectory
{
    public long Start { get; set; }
    public long LengthLong { get; set; }
    public int Length { get; }
    IDirectory? Parent { get; set; }
    IBox Box { get; }
    IBox? BoxInternal { get; set; }
    bool Parse(IBox parent, ref long offset, Dictionary<string, IRecordValue?>? valueMap);
}
