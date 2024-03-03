using System;
using System;
using System.IO;
using Jp2000;

namespace Jp2000;

public class Reader
{
    private Stream m_stream;

    public Reader(Stream stream)
    {
        m_stream = stream;
    }

    public void DoAndRestore(Action action)
    {
        long pos = m_stream.Position;
        action();
        m_stream.Position = pos;
    }

    public long Length => m_stream.Length;

    public long Position
    {
        get => m_stream.Position;
        set => m_stream.Position = value;
    }

    public byte[]? Read(int length)
    {
        byte[] buffer = new byte[length];

        if (m_stream.Read(buffer, 0, length) != length)
            return null;

        return buffer;
    }

    public long? ReadLong()
    {
        byte[]? buffer = Read(8);

        if (buffer == null)
            return null;

        if (BitConverter.IsLittleEndian)
            Array.Reverse(buffer, 0, 8);

        return BitConverter.ToInt32(buffer);
    }

    public static short ShortFromBytes(ReadOnlySpan<byte> data, int offset)
    {
        byte[] bytes = new byte[2];

        data[offset..(offset + 2)].CopyTo(bytes);

        if (BitConverter.IsLittleEndian)
            Array.Reverse(bytes, 0, bytes.Length);

        return BitConverter.ToInt16(bytes);
    }

    public static UInt16 UInt16FromBytes(ReadOnlySpan<byte> data, int offset)
    {
        byte[] bytes = new byte[2];

        data[offset..(offset + 2)].CopyTo(bytes);

        if (BitConverter.IsLittleEndian)
            Array.Reverse(bytes, 0, bytes.Length);

        return BitConverter.ToUInt16(bytes);
    }

    public static Int32 LongFromBytes(ReadOnlySpan<byte> data, int offset)
    {
        byte[] bytes = new byte[4];

        data[offset..(offset + 4)].CopyTo(bytes);

        if (BitConverter.IsLittleEndian)
            Array.Reverse(bytes, 0, bytes.Length);

        return BitConverter.ToInt32(bytes);
    }

    public static Int64 LongLongFromBytes(ReadOnlySpan<byte> data, int offset)
    {
        byte[] bytes = new byte[8];

        data[offset..(offset + 8)].CopyTo(bytes);

        if (BitConverter.IsLittleEndian)
            Array.Reverse(bytes, 0, bytes.Length);

        return BitConverter.ToInt64(bytes);
    }


    public short? ReadShort()
    {
        byte[]? buffer = Read(4);

        if (buffer == null)
            return null;

        if (BitConverter.IsLittleEndian)
            Array.Reverse(buffer, 0, 4);

        return BitConverter.ToInt16(buffer);
    }

    public byte[]? ReadAndRestore(int length, long? offset = null)
    {
        byte[]? buffer = null;

        DoAndRestore(() =>
                     {
                         if (offset != null)
                             Position = offset.Value;

                         buffer = Read(length);
                     });

        return buffer;
    }

    public static bool CompareBytes(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
    {
        return left.SequenceEqual(right);
    }


//    byte[]? ReadBoxHeader()
//    {
//
//    }
//    public bool FReadMetadata()
//    {
//        if (!FCheckMagicNumber())
//            return false;
//
//
//
//    }
}
