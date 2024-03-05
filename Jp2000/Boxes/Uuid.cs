using Jp2000;
using Jp2000.Directories;
using Jp2000.Directories.Records;
using Jp2000.Directories.Records.RecordValues;
using System.Text;

namespace Jp2000.Boxes;

public class Uuid : BoxBase, IBox
{
    private readonly byte[] ID = Tables.BoxId_Uuid;

    ReadOnlySpan<byte> IBox.ID => ID;
    ReadOnlySpan<byte> IBox.BoxData => BoxData;

    public string Name { get; set; } =  "Uuid";

    private readonly Dictionary<IRecordValue, string> m_uuidDirectoryMap =
        new()
        {
            { new Bytes("JpgTiffExif->JP2"u8.ToArray()), "EXIF" },
            { new Bytes(new byte[] {0x05,0x37,0xcd,0xab,0x9d,0x0c,0x44,0x31,0xa7,0x2a,0xfa,0x56,0x1f,0x2a,0x11,0x3e }), "EXIF-Adobe" },
            { new Bytes(new byte[] {0x33,0xc7,0xa4,0xd2,0xb8,0x1d,0x47,0x23,0xa0,0xba,0xf1,0xa3,0xe0,0x97,0xad,0x38 }), "IPTC" },
            { new Bytes(new byte[] {0x09,0xa1,0x4e,0x97,0xc0,0xb4,0x42,0xe0,0xbe,0xbf,0x36,0xdf,0x6f,0x0c,0xe3,0x6f }), "IPTC-Adobe" },
            { new Bytes(new byte[] {0xbe,0x7a,0xcf,0xcb,0x97,0xa9,0x42,0xe8,0x9c,0x71,0x99,0x94,0x91,0xe3,0xaf,0xac }), "XMP" },
            { new Bytes(new byte[] {0xb1,0x4b,0xf8,0xbd,0x08,0x3d,0x4b,0x43,0xa5,0xae,0x8c,0xd7,0xd5,0xa6,0xce,0x03 }), "GeoJP2" },
            { new Bytes(new byte[] {0x2c,0x4c,0x01,0x00,0x85,0x04,0x40,0xb9,0xa0,0x3e,0x56,0x21,0x48,0xd6,0xdf,0xeb }), "Photoshop" },
            { new Bytes(new byte[] {(byte)'c', (byte)'2', (byte)'c', (byte)'s',0x00,0x11,0x00,0x10,0x80,0x00,0x00,0xaa,0x00,0x38,0x9b,0x71 }), "C2PAClaimSignature" },
            { new Bytes(new byte[] {(byte)'c', (byte)'a', (byte)'s', (byte)'g',0x00,0x11,0x00,0x10,0x80,0x00,0x00,0xaa,0x00,0x38,0x9b,0x71 }), "Signature" },
        };

    string? GetUuidType()
    {
        foreach (KeyValuePair<IRecordValue, string> kv in m_uuidDirectoryMap)
        {
            if (kv.Key.IsEqual(BoxData![0..16]))
                return kv.Value;
        }

        return null;
    }

    public bool Parse()
    {
        if (BoxData == null)
            return true;

        // This box has several sub-boxes
        Dictionary<string, Dictionary<string, IRecordValue?>> valuesMaps = new();

        // examine the first 16 bytes of the box data to see what kind of
        // directory we are going to find

        // map the record value to something
        string? uuidType = GetUuidType();

        if (uuidType == null)
        {
            Console.WriteLine($"Skipping unknown UUID box: {Encoding.UTF8.GetString(BoxData[0..16])}");
            return true;
        }

        IDirectory? dir;

        switch (uuidType)
        {
            case "EXIF":
                Name = "EXIF";
                int exifStart =
                    Reader.CompareBytes("Exif\0\0"u8.ToArray(), BoxData[16..22])
                        ? 22 // broken Digikam structure
                        : 16;

                dir = DirectoryBase.CreateDirectory<EXIF>(this, null, exifStart, BoxLength - exifStart, ValueMap);
                break;
            case "IPTC":
                Name = "IPTC";
                int iptcStart = 16;
                dir = DirectoryBase.CreateDirectory<IPTC>(this, null, iptcStart, BoxLength - iptcStart, ValueMap);
                break;
            case "XMP":
                Name = "XMP";
                int xmpStart = 16;
                dir = DirectoryBase.CreateDirectory<XMP>(this, null, xmpStart, BoxLength - xmpStart, ValueMap);
                break;
        }

        Box.EnumerateValueMaps(
            valuesMaps,
            _ => { },
            (outerKey, key, value) => ValueMap.Add($"{outerKey}:{key}", value));

        return true;
    }

    public static IBox StaticFactory()
    {
        return new Uuid();
    }
}
