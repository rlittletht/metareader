// See https://aka.ms/new-console-template for more information

using Jp2000;
using MetadataExtractor;
using Directory = MetadataExtractor.Directory;

Console.WriteLine("Hello, World!");

string filename = "c:\\temp\\image11.jp2";

using (FileStream stream = new FileStream(filename, FileMode.Open))
{

    Metadata.CreateFromStream(stream);
}

IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(filename);

Console.WriteLine("MetadataExtractor\n");
foreach (Directory directory in directories)
{
    Console.WriteLine(directory.Name);
    foreach (Tag tag in directory.Tags)
    {
        Console.WriteLine($"{tag.Name}: {tag.Description}");
    }
}

