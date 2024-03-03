using System;

namespace Jp2000;

public class Tables
{
    public static byte[] Magic1 = new byte[] { 0x00, 0x00, 0x00, 0x0c, (byte)'j', (byte)'P', 0x20, 0x20, 0x0d, 0x0a, 0x87, 0x0a };
    public static byte[] Magic2 = new byte[] { 0x00, 0x00, 0x00, 0x0c, (byte)'j', (byte)'P', 0x1a, 0x1a, 0x0d, 0x0a, 0x87, 0x0a };

    public static int MagicLength = Math.Max(Magic1.Length, Magic2.Length);

    public static int BoxHeaderLength = 8;

    public static byte[] BoxId_Ftyp = "ftyp"u8.ToArray();
    public static byte[] BoxId_Jp2Header = "jp2h"u8.ToArray();
    public static byte[] BoxId_Jp2Header_Resolution = "res "u8.ToArray();
    public static byte[] BoxId_Jp2Header_Resolution_CaptureResolution = "resc"u8.ToArray();
}