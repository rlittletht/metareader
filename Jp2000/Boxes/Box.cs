namespace Jp2000.Boxes;

public class Box
{
    public static IBox[] RootBoxes =
        new IBox[]
        {
            new Ftyp(),
            //new Jp2Header(),
            new Unknown()
        };

    public static IBox? GetMatchingBoxHeader(IBox[] boxes, BoxHeader header)
    {
        // now look for the right box type
        foreach (IBox box in boxes)
        {
            if (header.MatchesID(box.ID))
                return box;
        }

        return null;
    }

//    public static IBox? CreateBoxFromData(ReadOnlySpan<byte> data, int offset, int length)
//    {
//
//    }
    /*----------------------------------------------------------------------------
        %%Function: CreateBox
        %%Qualified: Jp2000.Boxes.Box.CreateBox

        This will read the header and create the right box type. It does NOT read
        the data (since you might not want to handle it)
    ----------------------------------------------------------------------------*/
    public static IBox? CreateBox(Reader reader, long offset)
    {
        BoxHeader header = new BoxHeader(reader, offset);

        IBox? box = GetMatchingBoxHeader(RootBoxes, header);

        if (box == null)
            return null;

        box.HeaderInternal = header;

        return box;
    }

    public static void ReadBoxesInRange(Reader reader, long start, long length, Dictionary<string, Dictionary<string, string>> valueMaps)
    {
        long position = start;

        while (position < start + length)
        {
            IBox? box = Box.CreateBox(reader, position);

            if (box == null)
                throw new Exception("couldn't create box");

            Console.WriteLine($"Box: {box.Header.Describe()}");

            box.Read(reader);
            box.Parse();
            if (box.ValueMap.Count > 0)
            {
                valueMaps.Add(box.Name, box.ValueMap);
            }

            position = box.BoxLim;
        }
    }
}
