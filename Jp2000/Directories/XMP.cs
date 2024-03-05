using Jp2000.Boxes;
using Jp2000.Directories.Records;
using Jp2000.Directories.Records.RecordValues;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.Iptc;
using MetadataExtractor.Formats.Xmp;
using MetadataExtractor.IO;
using XmpCore;
using XmpCore.Impl;

namespace Jp2000.Directories;


public class XMP : DirectoryBase, IDirectory
{
    public XMP()
    {
    }

    public bool Parse(IBox parent, ref long offset, Dictionary<string, IRecordValue?>? valueMap)
    {
        byte[] iptcData = new byte[parent.BoxLength];
        parent.BoxData[(int)offset..(int)(parent.BoxLength)].CopyTo(iptcData);

        SequentialByteArrayReader byteReader = new SequentialByteArrayReader(iptcData, 0, Reader.ByteOrder == ByteOrder.BigEndian);
        XmpReader iptcReader = new XmpReader();
        XmpDirectory xmpDir = iptcReader.Extract(iptcData);

        if (valueMap != null)
        {
            foreach (Tag tag in xmpDir.Tags)
            {
                valueMap.Add(tag.Name, new JpConstant(tag.Description ?? "<null>"));
            }

            if (xmpDir.XmpMeta != null)
            {
                foreach (IXmpPropertyInfo info in xmpDir.XmpMeta.Properties)
                {
                    valueMap.Add($"{info.Namespace}:{info.Path}", new JpConstant(info.Value));
                }
            }
        }

        return true;
    }
}
