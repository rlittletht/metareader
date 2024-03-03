using Jp2000.Boxes;
using Jp2000.Directories.Records;

namespace Jp2000.Directories;


public class FileType : DirectoryBase, IDirectory
{
    private readonly IRecord[] Records =
    {
        new Record(
            "MajorBrand",
            RecordType.Byte,
            new RecordLength(4),
            new Dictionary<IRecordValue, string>()
            {
                { new Records.RecordValues.Bytes("jp2 "u8.ToArray()), "image/jp2" },
                { new Records.RecordValues.Bytes("jpm "u8.ToArray()), "image/jpm" },
                { new Records.RecordValues.Bytes("jpx "u8.ToArray()), "image/jpx" },
                { new Records.RecordValues.Bytes("jpl "u8.ToArray()), "image/jxl" },
                { new Records.RecordValues.Bytes("jph "u8.ToArray()), "image/jph" }
            }),
        new Record(
            "Minor Version",
            RecordType.Byte,
            new RecordLength(4),
            FileType.MinorVersionValueFactory),
        new Record(
            "CompatibleBrands",
            RecordType.Byte,
            new RecordLength(RecordLength.LenType.UntilEnd),
            FileType.CompatibleBrandsFactory)
    };

    public bool Parse(IBox parent, ref long offset, Dictionary<string, string>? valueMap) => Parse(Records, parent, ref offset, valueMap);

    public static IRecordValue MinorVersionValueFactory(ReadOnlySpan<byte> data)
    {
        return new Records.RecordValues.MinorVersion(data);
    }

    public static IRecordValue CompatibleBrandsFactory(ReadOnlySpan<byte> data)
    {
        return new Records.RecordValues.CompatibleBrands(data);
    }

    public static byte[] Passthrough(ReadOnlySpan<byte> bytes)
    {
        byte[] ret = new byte[bytes.Length];
        bytes.CopyTo(ret);
        return ret;
    }

//    public bool Parse(IBox box, ref long start, Dictionary<string, string>? valueMap)
//    {
//        long position = start;
//
//        foreach (IRecord record in Records)
//        {
//            string? s = record.Parse(box.BoxData, ref position);
//
//            valueMap?.Add(record.Name, s ?? "<null>");
//        }
//
//        start = position;
//
//        return true;
//    }
}
