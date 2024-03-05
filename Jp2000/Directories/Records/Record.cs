namespace Jp2000.Directories.Records;

public class Record : IRecord
{
    public RecordType RecordType { get; init; }
    public RecordLength Length { get; init; }

    public string Name { get; init; }
    public byte[]? Data;

    ReadOnlySpan<byte> IRecord.Data => Data;

    private readonly Dictionary<IRecordValue, string>? MapValueDescription;

    public delegate IRecordValue RecordValueFactoryDelegate(ReadOnlySpan<byte> data);
    private readonly RecordValueFactoryDelegate? m_ValueFactoryDelegate;

    public IRecordValue RecordValueFactory(ReadOnlySpan<byte> data) => m_ValueFactoryDelegate?.Invoke(data) ?? throw new Exception("no record value");

    public Action<IRecordValue>? OnRecordParsed { get; set; }

    public Record(string name, RecordType type, RecordLength len, Dictionary<IRecordValue, string> map, RecordValueFactoryDelegate? valueFactory = null, Action<IRecordValue>? onParse = null)
    {
        Name = name;
        RecordType = type;
        Length = len;
        MapValueDescription = map;
        OnRecordParsed = onParse;
    }

    public Record(string name, RecordType type, RecordLength len, RecordValueFactoryDelegate valueFactory, Action<IRecordValue>? onParse = null)
    {
        RecordType = type;
        Length = len;
        Name = name;
        m_ValueFactoryDelegate = valueFactory;
        OnRecordParsed = onParse;
    }

    private int LengthFromType()
    {
        return RecordType switch
        {
            RecordType.Byte => 1,
            RecordType.Int16 => 2,
            RecordType.Int32 => 4,
            RecordType.Int64 => 8,
            _ => throw new Exception("unknown type")
        };
    }

    /*----------------------------------------------------------------------------
        %%Function: CalculateLength
        %%Qualified: Jp2000.Directories.Records.Record.CalculateLength

        Return the length of this record. A negative value means the record
        is corrupt and the abs(length) is the length to skip
    ----------------------------------------------------------------------------*/
    private int CalculateLength(ReadOnlySpan<byte> data, long offset)
    {
        if (offset > Int32.MaxValue)
            throw new Exception("can't handle > 2gb");

        int position = (int)offset;

        if (Length.Type == RecordLength.LenType.Constant)
            return LengthFromType() * Length.Count;

        if (Length.Type == RecordLength.LenType.UntilEnd)
            return data.Length - position;

        int cZero = 0;

        while (position < data.Length)
        {
            if (data[position++] == 0)
            {
                cZero++;
                if (cZero == 4 && Length.Type == RecordLength.LenType.Until0000)
                    return (int)(position - offset);
                if (cZero == 2 && Length.Type == RecordLength.LenType.Until00)
                    return (int)(position - offset);
                if (cZero == 1 && Length.Type == RecordLength.LenType.Until0)
                    return (int)(position - offset);
            }
            else
            {
                // reset zero count
                cZero = 0;
            }
        }

        return -(data.Length - position);
    }

    public IRecordValue? Parse(ReadOnlySpan<byte> data, ref long position)
    {
        int length = CalculateLength(data, position);
        if (length < 0)
        {
            position += -length;
            return null;
        }

        Data = new byte[length];

        data[(int)position..(int)(position + length)].CopyTo(Data);
        position += length;

        if (MapValueDescription != null)
        {
            // map the record value to something
            foreach (KeyValuePair<IRecordValue, string> kv in MapValueDescription)
            {
                if (kv.Key.IsEqual(Data))
                {
                    OnRecordParsed?.Invoke(kv.Key);
                    return new RecordValues.JpConstant(kv.Value);
                }
            }
        }
        else
        {
            IRecordValue value = RecordValueFactory(Data);

            OnRecordParsed?.Invoke(value);

            // just return the value
            return value;
        }

        return null;
    }

}
