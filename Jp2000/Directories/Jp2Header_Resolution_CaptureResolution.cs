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
            JpRational32U.StaticFactory),
        new Record(
            "ResX",
            RecordType.Byte,
            new RecordLength(4),
            JpRational32U.StaticFactory),
        new Record(
            "XUnit",
            RecordType.Byte,
            new RecordLength(1),
            new Dictionary<IRecordValue, string>()
            {
                { new Records.RecordValues.JpByte(0xfd), "km" },
                { new Records.RecordValues.JpByte(0xfe), "100m" },
                { new Records.RecordValues.JpByte(0xff), "10m" },
                { new Records.RecordValues.JpByte(0x00), "m" },
                { new Records.RecordValues.JpByte(0x01), "10cm" },
                { new Records.RecordValues.JpByte(0x02), "cm" },
                { new Records.RecordValues.JpByte(0x03), "mm" },
                { new Records.RecordValues.JpByte(0x04), "0.1mm" },
                { new Records.RecordValues.JpByte(0x05), "0.01mm" },
                { new Records.RecordValues.JpByte(0x06), "um" }
            }),
        new Record(
            "YUnit",
            RecordType.Byte,
            new RecordLength(1),
            new Dictionary<IRecordValue, string>()
            {
                { new Records.RecordValues.JpByte(0xfd), "km" },
                { new Records.RecordValues.JpByte(0xfe), "100m" },
                { new Records.RecordValues.JpByte(0xff), "10m" },
                { new Records.RecordValues.JpByte(0x00), "m" },
                { new Records.RecordValues.JpByte(0x01), "10cm" },
                { new Records.RecordValues.JpByte(0x02), "cm" },
                { new Records.RecordValues.JpByte(0x03), "mm" },
                { new Records.RecordValues.JpByte(0x04), "0.1mm" },
                { new Records.RecordValues.JpByte(0x05), "0.01mm" },
                { new Records.RecordValues.JpByte(0x06), "um" }
            }),

    };

    public bool Parse(IBox parent, ref long offset, Dictionary<string, IRecordValue?>? valueMap) => Parse(Records, parent, ref offset, valueMap);

}
