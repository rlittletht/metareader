using System.Text;
using Jp2000;

namespace Jp2000.Directories.Records.RecordValues;

public class CompatibleBrands: IRecordValue
{
    private readonly List<string> m_brands = new();

    public byte[]? Value { get; private set; }

    public void FillBrandsFromData(List<string> brands, ReadOnlySpan<byte> data)
    {
        // each 4 bytes is a brand
        int position = 0;

        while (position + 4 <= data.Length)
        {
            brands.Add(Encoding.UTF8.GetString(data[position..(position + 4)]));
            position += 4;
        }
    }

    public void SetFromData(ReadOnlySpan<byte> data)
    {
        Value = new byte[data.Length];
        data.CopyTo(Value);

        FillBrandsFromData(m_brands, data);
    }

    public bool IsEqual(ReadOnlySpan<byte> data)
    {
        if (Value == null)
            return false;

        return Reader.CompareBytes(Value, data);
    }

    public CompatibleBrands(ReadOnlySpan<byte> data)
    {
        SetFromData(data);
    }

    public IRecordValue Factory(ReadOnlySpan<byte> data)
    {
        return new CompatibleBrands(data);
    }

    public override string ToString()
    {
        return string.Join(",", m_brands);
    }
}
