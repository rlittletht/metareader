
using Jp2000.Boxes;
using Jp2000.Directories;
using System.Reflection.PortableExecutable;
using Jp2000.Directories.Records;

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

        IBox? firstBox = Box.CreateBox(Box.RootBoxes, reader, Tables.MagicLength);

        if (firstBox == null)
            return null;

        Console.WriteLine($"Box1: {firstBox.Header?.Describe()}");
        Dictionary<string, Dictionary<string, IRecordValue?>> valueMaps = new();

        if (firstBox is Ftyp typeBox)
        {
            firstBox.Read(reader);
            firstBox.Parse();

            if (!typeBox.ValueMap.TryGetValue("MajorBrand", out IRecordValue? value))
                throw new Exception("no ftyp record");

            string sValue = value?.ToString() ?? "";

            if (sValue != "image/jp2" && sValue != "image/jpx")
                Console.WriteLine($"WARNING: Processing non jp2/jpx file: ${value}");

            valueMaps.Add("ftyp", firstBox.ValueMap);
        }

        // now process the rest of the boxes
        position = firstBox.BoxLim;

        Box.ReadBoxesInRange(Box.RootBoxes, reader, position, stream.Length - position, valueMaps);

        Box.EnumerateValueMaps(
            valueMaps,
            (key)=> Console.WriteLine($"Box: {key}"),
            (_, key, value) => Console.WriteLine($"{key}: {value}"));

        return metadata;
    }
}
