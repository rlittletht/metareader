﻿namespace Jp2000.Directories.Records;

public interface IRecord
{
    public RecordType RecordType { get; }
    public RecordLength Length { get; }
    public ReadOnlySpan<byte> Data { get; }

    public string Name { get; }
    public string? Parse(ReadOnlySpan<byte> data, ref long position);
    public IRecordValue RecordValueFactory(ReadOnlySpan<byte> data);
}
