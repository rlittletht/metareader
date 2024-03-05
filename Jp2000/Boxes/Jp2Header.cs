using Jp2000;
using Jp2000.Directories;
using Jp2000.Directories.Records;

namespace Jp2000.Boxes;

public class Jp2Header: BoxBase, IBox
{
    public static Dictionary<byte[], BoxFactoryDelegate> Boxes =
        new()
        {
            { Tables.BoxId_Jp2Header_Resolution, Jp2Header_Resolution.StaticFactory },
            { Array.Empty<byte>(), Unknown.StaticFactory }
        };

    private readonly byte[] ID = Tables.BoxId_Jp2Header;

    ReadOnlySpan<byte> IBox.ID => ID;
    ReadOnlySpan<byte> IBox.BoxData => BoxData;

    public string Name => "Jp2Header";

    public bool Parse()
    {
        // This box has several sub-boxes
        Dictionary<string, Dictionary<string, IRecordValue?>> valuesMaps = new();

        Box.ReadBoxesInRange(Boxes, BoxData, 0, BoxLength, valuesMaps);

        Box.EnumerateValueMaps(
            valuesMaps,
            _ => { },
            (outerKey, key, value) => ValueMap.Add($"{outerKey}:{key}", value));

        return true;
    }

    public static IBox StaticFactory()
    {
        return new Jp2Header();
    }
}
