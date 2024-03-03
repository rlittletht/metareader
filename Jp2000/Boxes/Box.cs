﻿using System.Reflection.PortableExecutable;

namespace Jp2000.Boxes;

public class Box
{
    public static IBox[] RootBoxes =
        new IBox[]
        {
            new Ftyp(),
            new Jp2Header(),
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

    public static IBox? CreateBox(IBox[] boxes, ReadOnlySpan<byte> data, int start, int limit)
    {
        BoxHeader header = new BoxHeader(data, start, limit);

        IBox? box = GetMatchingBoxHeader(boxes, header);

        if (box != null)
            box.HeaderInternal = header;

        return box;
    }

    /*----------------------------------------------------------------------------
        %%Function: CreateBox
        %%Qualified: Jp2000.Boxes.Box.CreateBox

        This will read the header and create the right box type. It does NOT read
        the data (since you might not want to handle it)
    ----------------------------------------------------------------------------*/
    public static IBox? CreateBox(IBox[] boxes, Reader reader, long offset)
    {
        BoxHeader header = new BoxHeader(reader, offset);

        IBox? box = GetMatchingBoxHeader(boxes, header);

        if (box != null)
            box.HeaderInternal = header;

        return box;
    }

    public static void ReadBoxesInRange(IBox[] boxes, ReadOnlySpan<byte> data, int start, int limit, Dictionary<string, Dictionary<string, string>> valueMaps)
    {
        int position = start;

        while (position < limit)
        {
            IBox? box = Box.CreateBox(boxes, data, position, limit);

            if (box == null)
                throw new Exception("couldn't create box");

            Console.WriteLine($"Box: {box.Header.Describe()}");

            box.Read(data);
            box.Parse();
            if (box.ValueMap.Count > 0)
            {
                valueMaps.Add(box.Name, box.ValueMap);
            }

            position = (int)box.BoxLim;
        }
    }

    public static void ReadBoxesInRange(IBox[] boxes, Reader reader, long start, long length, Dictionary<string, Dictionary<string, string>> valueMaps)
    {
        long position = start;

        while (position < start + length)
        {
            IBox? box = Box.CreateBox(boxes, reader, position);

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

    public static void EnumerateValueMaps(Dictionary<string, Dictionary<string, string>> valueMaps, Action<string> onNewKey, Action<string, string, string> onValueMap)
    {
        foreach (KeyValuePair<string, Dictionary<string, string>> kvp in valueMaps)
        {
            onNewKey(kvp.Key);
            foreach (KeyValuePair<string, string> metadataPair in kvp.Value)
            {
                onValueMap(kvp.Key, metadataPair.Key, metadataPair.Value);
            }
        }
    }
}
