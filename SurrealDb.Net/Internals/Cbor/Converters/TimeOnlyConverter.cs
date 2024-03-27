#if NET6_0_OR_GREATER
using Dahomey.Cbor.Serialization;
using Dahomey.Cbor.Serialization.Converters;
using SurrealDb.Net.Internals.Formatters;
using SurrealDb.Net.Internals.Parsers;

namespace SurrealDb.Net.Internals.Cbor.Converters;

internal class TimeOnlyConverter : CborConverterBase<TimeOnly>
{
    public override TimeOnly Read(ref CborReader reader)
    {
        var value = reader.ReadString();

        return (value is not null) ? TimeOnlyParser.Parse(value) : default;
    }

    public override void Write(ref CborWriter writer, TimeOnly value)
    {
        writer.WriteSemanticTag(CborTagConstants.TAG_DURATION);
        writer.WriteString(TimeSpanFormatter.Format(value.ToTimeSpan()));
    }
}
#endif
