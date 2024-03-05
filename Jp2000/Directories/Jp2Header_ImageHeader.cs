using Jp2000.Boxes;
using Jp2000.Directories.Records;
using Jp2000.Directories.Records.RecordValues;

namespace Jp2000.Directories;


public class Jp2Header_ImageHeader: DirectoryBase, IDirectory
{
    private readonly IRecord[] Records =
    {
        new Record(
            "ImageHeight",
            RecordType.Int32,
            new RecordLength(1),
            JpInt32U.StaticFactory),
        new Record(
            "ImageWidth",
            RecordType.Int32,
            new RecordLength(1),
            JpInt32U.StaticFactory),
        new Record(
            "NumberOfComponents",
            RecordType.Int16,
            new RecordLength(1),
            JpInt16U.StaticFactory),
        new Record(
            "BitsPerComponent",
            RecordType.Byte,
            new RecordLength(1),
            JpByte.StaticFactory),
        new Record(
            "Compression",
            RecordType.Byte,
            new RecordLength(1),
            new Dictionary<IRecordValue, string>()
            {
                { new Records.RecordValues.JpByte(0x00), "Uncompressed" },
                { new Records.RecordValues.JpByte(0x01), "Modified Huffman" },
                { new Records.RecordValues.JpByte(0x02), "Modified READ" },
                { new Records.RecordValues.JpByte(0x03), "Modified Modified READ" },
                { new Records.RecordValues.JpByte(0x04), "JBIG" },
                { new Records.RecordValues.JpByte(0x05), "JPEG" },
                { new Records.RecordValues.JpByte(0x06), "JPEG-LS" },
                { new Records.RecordValues.JpByte(0x07), "JPEG 2000" },
                { new Records.RecordValues.JpByte(0x08), "JBIG2" }
            })
    };

    public bool Parse(IBox parent, ref long offset, Dictionary<string, IRecordValue?>? valueMap) => Parse(Records, parent, ref offset, valueMap);

}
