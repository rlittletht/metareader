using Jp2000;
using Jp2000.Directories;
using Jp2000.Directories.Records;

namespace Jp2000.Boxes;

public class Jp2Header_Resolution: BoxBase, IBox
{
    public static Dictionary<byte[], BoxFactoryDelegate> Boxes =
        new()
        {
            { Tables.BoxId_Jp2Header_Resolution_CaptureResolution, Jp2Header_Resolution_CaptureResolution.StaticFactory },
            { Array.Empty<byte>(), Unknown.StaticFactory }
        };

    private readonly byte[] ID = Tables.BoxId_Jp2Header_Resolution;

    ReadOnlySpan<byte> IBox.ID => ID;
    ReadOnlySpan<byte> IBox.BoxData => BoxData;

    public string Name => "Resolution";

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
        return new Jp2Header_Resolution();
    }
}
