using Jp2000.Boxes;
using Jp2000.Directories.Records;
using Jp2000.Directories.Records.RecordValues;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.Iptc;
using MetadataExtractor.IO;

namespace Jp2000.Directories;


public class IPTC : DirectoryBase, IDirectory
{
    public IPTC()
    {
    }

    public bool Parse(IBox parent, ref long offset, Dictionary<string, IRecordValue?>? valueMap)
    {
        byte[] iptcData = new byte[parent.BoxLength];
        parent.BoxData[(int)offset..(int)(parent.BoxLength)].CopyTo(iptcData);

        SequentialByteArrayReader byteReader = new SequentialByteArrayReader(iptcData, 0, Reader.ByteOrder == ByteOrder.BigEndian);
        IptcReader iptcReader = new IptcReader();
        IptcDirectory iptcDir = iptcReader.Extract(byteReader, parent.BoxLength - offset);

        if (valueMap != null)
        {
            foreach (Tag tag in iptcDir.Tags)
            {
                valueMap.Add(tag.Name, new JpConstant(tag.Description ?? "<null>"));
            }
        }

        return true;
    }
}
