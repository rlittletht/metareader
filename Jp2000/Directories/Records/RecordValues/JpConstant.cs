using System.Text;
using Jp2000;

namespace Jp2000.Directories.Records.RecordValues;

class JpConstant : IRecordValue
{
    public string? Value { get; private set; }

    public bool IsEqual(ReadOnlySpan<byte> other)
    {
        return false;
    }

    public void SetFromData(ReadOnlySpan<byte> data)
    {
        throw new NotImplementedException();
    }

    public JpConstant(string value)
    {
        Value = value;
    }

    public IRecordValue Factory(ReadOnlySpan<byte> data)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        if (Value == null)
            return "<null>";

        return Value;
    }
}