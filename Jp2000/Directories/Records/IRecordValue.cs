namespace Jp2000.Directories.Records;

public interface IRecordValue
{
    bool IsEqual(ReadOnlySpan<byte> other);
    void SetFromData(ReadOnlySpan<byte> data);
    IRecordValue Factory(ReadOnlySpan<byte> data);
    string ToString();
}
