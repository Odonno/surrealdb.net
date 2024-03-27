#if NET6_0_OR_GREATER
using Dahomey.Cbor.Serialization;
using Dahomey.Cbor.Serialization.Converters;
using SurrealDb.Net.Internals.Parsers;

namespace SurrealDb.Net.Internals.Cbor.Converters;

internal class DateOnlyConverter : CborConverterBase<DateOnly>
{
    public override DateOnly Read(ref CborReader reader)
    {
        var value = reader.ReadString();

        return (value is not null) ? DateOnlyParser.Parse(value) : default;
    }

    public override void Write(ref CborWriter writer, DateOnly value)
    {
        writer.WriteSemanticTag(CborTagConstants.TAG_DATETIME);
        writer.WriteString(value.ToDateTime(new(), DateTimeKind.Utc).ToString("O"));
    }
}
#endif
