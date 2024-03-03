using Jp2000;

namespace Jp2000.Directories.Records.RecordValues;

public class MinorVersion: IRecordValue
{
    public struct Version
    {
        public short MM;
        public byte M1;
        public byte M2;
    }

    public byte[]? Value { get; private set; }

    private Version m_version;

    public Version VersionFromData(ReadOnlySpan<byte> data)
    {
        return new Version
               {
                   MM = Reader.ShortFromBytes(data, 0),
                   M1 = data[2],
                   M2 = data[3]
               };
    }

    public void SetFromData(ReadOnlySpan<byte> data)
    {
        Value = new byte[data.Length];
        data.CopyTo(Value);

        m_version = VersionFromData(data);
    }

    public bool IsEqual(ReadOnlySpan<byte> data)
    {
        if (Value == null)
            return false;

        Version otherVersion = VersionFromData(data);

        if (otherVersion.MM != m_version.MM)
            return false;
        if (otherVersion.M1 != m_version.M1)
            return false;
        if (otherVersion.M2 != m_version.M2)
            return false;

        return true;
    }

    public MinorVersion(ReadOnlySpan<byte> data)
    {
        SetFromData(data);
    }

    public IRecordValue Factory(ReadOnlySpan<byte> data)
    {
        return new MinorVersion(data);
    }

    public override string ToString()
    {
        return $"{m_version.MM}.{(int)m_version.M1}.{(int)m_version.M2}";
    }
}
