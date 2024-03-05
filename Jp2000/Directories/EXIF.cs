﻿using Jp2000.Boxes;
using Jp2000.Directories.Records;
using Jp2000.Directories.Records.RecordValues;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.IO;

namespace Jp2000.Directories;


public class EXIF : DirectoryBase, IDirectory
{
    private readonly IRecord[] HeaderRecords =
    {
        new Record(
            "ByteOrderMarker",
            RecordType.Byte,
            new RecordLength(2),
            ByteOrderMarker.StaticFactory),
        new Record(
            "TiffMagic",
            RecordType.Int16,
            new RecordLength(1),
            JpInt16.StaticFactory),
        new Record(
            "Ifd0Offset",
            RecordType.Int32,
            new RecordLength(1),
            JpInt32.StaticFactory),
    };

    private bool m_pushedBom = false;

    public EXIF()
    {
        // need to set the OnParse for the byte order marker
        HeaderRecords[0].OnRecordParsed = PushByteOrder;
    }

    public void PushByteOrder(IRecordValue value)
    {
        if (value is ByteOrderMarker bom)
        {
            Reader.PushByteOrder(bom.ByteOrder);
            m_pushedBom = true;
        }
    }

    class ExifRecord
    {
        public UInt16 ID;
        public Record.RecordValueFactoryDelegate Factory;
        public string Name;

        public ExifRecord(UInt16 id, Record.RecordValueFactoryDelegate factory, string name)
        {
            ID = id;
            Factory = factory;
            Name = name;
        }
    }

    public class ExifFormat
    {
        public static int s_int8u = 1;
        public static int s_string = 2;
        public static int s_int16u = 3;
        public static int s_int32u = 4;
        public static int s_rational64u = 5;
        public static int s_int8s = 6;
        public static int s_binary = 7;
        public static int s_int16s = 8;
        public static int s_int32s = 9;
        public static int s_rational64s = 10;
        public static int s_float = 11;
        public static int s_double = 12;
        public static int s_ifd = 13;
        public static int s_unicode = 14;
        public static int s_complex = 15;
        public static int s_int64u = 16;
        public static int s_int64s = 17;
        public static int s_ifd64 = 18;
        public static int s_utf8 = 129;
    }

    private Dictionary<int, Record.RecordValueFactoryDelegate> mapFormats =
        new()
        {
            { ExifFormat.s_int8u, JpByte.StaticFactory },
            { ExifFormat.s_string, JpUtf8String.StaticFactory },
            { ExifFormat.s_int16u, JpInt16U.StaticFactory },
            { ExifFormat.s_int32u, JpInt32U.StaticFactory },
            { ExifFormat.s_rational64u, JpRational64U.StaticFactory },
            { ExifFormat.s_int8s, JpByte.StaticFactory },
            { ExifFormat.s_binary, Bytes.StaticFactory },
            { ExifFormat.s_int16s, JpInt16.StaticFactory },
            { ExifFormat.s_int32s, JpInt32.StaticFactory },
            { ExifFormat.s_rational64s, JpRational64.StaticFactory },
            // { ExifFormat.s_float, Jpfloat.StaticFactory },
            // { ExifFormat.s_double, Jpdouble.StaticFactory },
            // { ExifFormat.s_ifd, Jpifd.StaticFactory },
            // { ExifFormat.s_unicode, Jpunicode.StaticFactory },
            // { ExifFormat.s_complex, Jpcomplex.StaticFactory },
            { ExifFormat.s_int64u, JpInt64U.StaticFactory },
            { ExifFormat.s_int64s, JpInt64.StaticFactory },
            // { ExifFormat.s_ifd64, Jpifd64.StaticFactory },
            { ExifFormat.s_utf8, JpUtf8String.StaticFactory }
        };

    private Dictionary<UInt16, IRecord> map =
        new()
        {
            { 0x100, new Record("ImageWidth", RecordType.Int16, new RecordLength(1), JpInt16U.StaticFactory) },
            { 0x101, new Record("ImageHeight", RecordType.Int16, new RecordLength(1), JpInt16U.StaticFactory) },
            { 0x102, new Record("BitsPerSample", RecordType.Int16, new RecordLength(1), JpInt16U.StaticFactory) },
            { 0x103, new Record("Compression", RecordType.Int16, new RecordLength(1), JpInt16U.StaticFactory) },
            { 0x106, new Record("PhotometricInterpretation", RecordType.Int16, new RecordLength(1), 
                new Dictionary<IRecordValue, string>()
                {
                    { new JpInt16U(0), "WhiteIsZero" },
                    { new JpInt16U(1), "BlackIsZero" },
                    { new JpInt16U(2), "RGB" },
                    { new JpInt16U(3), "RGB Palette" },
                    { new JpInt16U(4), "Transparency Mask" },
                    { new JpInt16U(5), "CMYK" },
                    { new JpInt16U(6), "YCbCr" },
                    { new JpInt16U(8), "CIELab" },
                    { new JpInt16U(9), "ICCLab" },
                    { new JpInt16U(10), "ITULab" },
                    { new JpInt16U(32803), "Color Filter Array" },
                    { new JpInt16U(32844), "Pixar LogL" },
                    { new JpInt16U(32845), "Pixar LogLuv" },
                    { new JpInt16U(32892), "Sequential Color Filter" },
                    { new JpInt16U(34892), "Linear Raw" },
                    { new JpInt16U(51177), "Depth Map" },
                    { new JpInt16U(52527), "Semantic Mask" },
                },
                JpInt16U.StaticFactory) },
            { 0x107, new Record("Thresholding", RecordType.Int16, new RecordLength(1), JpInt16U.StaticFactory) },
        };

    public bool Parse(IBox parent, ref long offset, Dictionary<string, IRecordValue?>? valueMap)
    {
        byte[] exifData = new byte[parent.BoxLength];
        //        parent.BoxData[(int)ifd0Offset..(int)(ifd0Offset + ifd0Len)].CopyTo(exifData);
        parent.BoxData[(int)offset..(int)(parent.BoxLength-offset)].CopyTo(exifData);

        ByteArrayReader byteReader = new ByteArrayReader(exifData, 0, Reader.ByteOrder == ByteOrder.BigEndian);
        ExifReader exifReader = new ExifReader();
        IEnumerable<MetadataExtractor.Directory> directories = exifReader.Extract(byteReader, 0);


        long start = offset;
        bool fRef = Parse(HeaderRecords, parent, ref offset, valueMap);

        if (valueMap == null)
            return false;

        // get the offset for the IFD0
        if (!valueMap.TryGetValue("Ifd0Offset", out IRecordValue? ifd0Value) || ifd0Value == null || !(ifd0Value is JpInt32 ifd0ValueInt32))
            return false;

        long ifd0Offset = start + ifd0ValueInt32.Value ?? 0;
        long ifd0Len = Length - ifd0Offset;


        // read the number of entries
        short entries = Reader.Int16FromBytes(parent.BoxData, (int)ifd0Offset);
        int dirSize = 2 + 12 * entries;
        long dirEnd = ifd0Offset + dirSize;
        long dirEndToEnd = offset + Length - dirEnd;

        for (int index = 0; index < entries; index++)
        {
            int entryStart = (int)ifd0Offset + 2 + 12 * index;
            int entryOffset = entryStart;

            UInt16 entryId = Reader.UInt16FromBytes(parent.BoxData, entryOffset);
            entryOffset += 2;
            short format = Reader.Int16FromBytes(parent.BoxData, entryOffset);
            entryOffset += 2;

            int entryCount = Reader.Int32FromBytes(parent.BoxData, entryOffset);
            entryOffset += 4;

            if (!map.TryGetValue(entryId, out IRecord? entryRecord) || entryRecord == null)
            {
                Console.WriteLine($"Skipping IFD0 ID: {entryId}");
                continue;
            }

            if (!mapFormats.TryGetValue(format, out Record.RecordValueFactoryDelegate? factory))
            {
                Console.WriteLine($"Skipping unknown format IFD0 ID({entryRecord.Name}: {format}");
                continue;
            }

            if (factory != entryRecord.RecordValueFactory)
                Console.WriteLine($"factory mismatch for record: {entryRecord.Name}: {factory} != {entryRecord.RecordValueFactory}");
            IRecordValue value = factory!(parent.BoxData[entryOffset..(int)(dirEnd - entryOffset)]);
            valueMap.Add(entryRecord.Name, value);
        }
        if (m_pushedBom)
            Reader.PopByteOrder();

        return fRef;
    }
}
