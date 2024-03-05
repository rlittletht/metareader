using Jp2000;
using Jp2000.Directories;
using Jp2000.Directories.Records;
using Jp2000.Directories.Records.RecordValues;
using System.Text;

namespace Jp2000.Boxes;

public class Uuid: BoxBase, IBox
{
    private readonly byte[] ID = Tables.BoxId_Uuid;

    ReadOnlySpan<byte> IBox.ID => ID;
    ReadOnlySpan<byte> IBox.BoxData => BoxData;

    public string Name => "Uuid";

    private readonly Dictionary<IRecordValue, string> m_uuidDirectoryMap =
        new()
        {
            { new Bytes("JpgTiffExif->JP2"u8.ToArray()), "EXIF" },
            { new Bytes("\x05\x37\xcd\xab\x9d\x0c\x44\x31\xa7\x2a\xfa\x56\x1f\x2a\x11\x3e"u8.ToArray()), "EXIF-Adobe" },
            { new Bytes("\x33\xc7\xa4\xd2\xb8\x1d\x47\x23\xa0\xba\xf1\xa3\xe0\x97\xad\x38"u8.ToArray()), "IPTC" },
            { new Bytes("\x09\xa1\x4e\x97\xc0\xb4\x42\xe0\xbe\xbf\x36\xdf\x6f\x0c\xe3\x6f"u8.ToArray()), "IPTC-Adobe" },
            { new Bytes("\xbe\x7a\xcf\xcb\x97\xa9\x42\xe8\x9c\x71\x99\x94\x91\xe3\xaf\xac"u8.ToArray()), "XMP" },
            { new Bytes("\xb1\x4b\xf8\xbd\x08\x3d\x4b\x43\xa5\xae\x8c\xd7\xd5\xa6\xce\x03"u8.ToArray()), "GeoJP2" },
            { new Bytes("\x2c\x4c\x01\x00\x85\x04\x40\xb9\xa0\x3e\x56\x21\x48\xd6\xdf\xeb"u8.ToArray()), "Photoshop" },
            { new Bytes("c2cs\x00\x11\x00\x10\x80\x00\x00\xaa\x00\x38\x9b\x71"u8.ToArray()), "C2PAClaimSignature" },
            { new Bytes("casg\x00\x11\x00\x10\x80\x00\x00\xaa\x00\x38\x9b\x71"u8.ToArray()), "Signature" },
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
                int exifStart =
                    Reader.CompareBytes("Exif\0\0"u8.ToArray(), BoxData[16..22])
                        ? 22 // broken Digikam structure
                        : 16;

                dir = DirectoryBase.CreateDirectory<EXIF>(this, null, exifStart, BoxLength - exifStart, ValueMap);
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
