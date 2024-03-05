using Jp2000.Boxes;
using Jp2000.Directories.Records;

namespace Jp2000.Directories;

public class DirectoryBase
{
    public long Start { get; set; }
    public long LengthLong { get; set; }
    public IDirectory? Parent { get; set; }
    public IBox Box => BoxInternal ?? throw new Exception("Box not set!");
    public Dictionary<string, string> Metadata { get; } = new();

    public IBox? BoxInternal { get; set; }

    public int Length
    {
        get
        {
            if (LengthLong > Int32.MaxValue)
                throw new Exception("can't handle directories > 2gb");
            return (int)LengthLong;
        }
    }

    public bool Parse(IRecord[] records, IBox box, ref long start, Dictionary<string, IRecordValue?>? valueMap)
    {
        long position = start;

        foreach (IRecord record in records)
        {
            IRecordValue? recordValue = record.Parse(box.BoxData, ref position);

            valueMap?.Add(record.Name, recordValue);
        }

        start = position;

        return true;
    }

    public static T? CreateDirectory<T>(IBox box, IDirectory? parent, long start, long length, Dictionary<string, IRecordValue?>? valueMap)
        where T : class, IDirectory, new()
    {
        T t = new()
              {
                  BoxInternal = box,
                  Parent = parent,
                  Start = start,
                  LengthLong = length
              };

        if (!t.Parse(box, ref start, valueMap))
            return null;

        return t;
    }
}
