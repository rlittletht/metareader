
using Jp2000.Boxes;
using Jp2000.Directories;
using System.Reflection.PortableExecutable;

namespace Jp2000;

public class Metadata
{
    public Metadata()
    {

    }

    static bool FCheckMagicNumber(Reader reader)
    {
        byte[]? magic = reader.ReadAndRestore(12);
        if (magic == null
            || (!Reader.CompareBytes(magic, Tables.Magic1) && !Reader.CompareBytes(magic, Tables.Magic2)))
        {
            return false;
        }

        return true;
    }
    
    public static Metadata? CreateFromStream(Stream stream)
    {
        Metadata metadata = new Metadata();

        Reader reader = new Reader(stream);

        if (!FCheckMagicNumber(reader))
            return null;

        long position = Tables.MagicLength;

        IBox? firstBox = Box.CreateBox(reader, Tables.MagicLength);

        if (firstBox == null)
            return null;

        Console.WriteLine($"Box1: {firstBox.Header?.Describe()}");
        Dictionary<string, Dictionary<string, string>> valueMaps = new();

        if (firstBox is Ftyp typeBox)
        {
            firstBox.Read(reader);
            firstBox.Parse();

            if (!typeBox.ValueMap.TryGetValue("MajorBrand", out string? value))
                throw new Exception("no ftyp record");

            if (value != "image/jp2" && value != "image/jpx")
                Console.WriteLine($"WARNING: Processing non jp2/jpx file: ${value}");

            valueMaps.Add("ftyp", firstBox.ValueMap);
        }

        // now process the rest of the boxes
        position = firstBox.BoxLim;

        Box.ReadBoxesInRange(reader, position, stream.Length - position, valueMaps);

        foreach (KeyValuePair<string, Dictionary<string, string>> kvp in valueMaps)
        {
            Console.WriteLine($"Box: {kvp.Key}");
            foreach (KeyValuePair<string, string> metadataPair in kvp.Value)
            {
                Console.WriteLine($"{metadataPair.Key}: {metadataPair.Value}");
            }
        }

        return metadata;
    }
}
