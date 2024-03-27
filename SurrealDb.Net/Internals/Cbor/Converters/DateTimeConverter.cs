using Dahomey.Cbor;
using Dahomey.Cbor.Serialization;
using Dahomey.Cbor.Serialization.Converters;
using SurrealDb.Net.Internals.Parsers;

namespace SurrealDb.Net.Internals.Cbor.Converters;

internal class DateTimeConverter : CborConverterBase<DateTime>
{
    public override DateTime Read(ref CborReader reader)
    {
        //bool hasSemanticTag = reader.TryReadSemanticTag(out var semanticTag);
        //if (!hasSemanticTag || semanticTag != CborTagConstants.TAG_DATETIME)
        //{
        //    throw new CborException("No datetime detected...");
        //}

        var value = reader.ReadString();

        return (value is not null) ? DateTimeParser.Parse(value) : default;
    }

    public override void Write(ref CborWriter writer, DateTime value)
    {
        writer.WriteSemanticTag(CborTagConstants.TAG_DATETIME);
        writer.WriteString(value.ToUniversalTime().ToString("O"));
    }
}
