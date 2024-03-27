using Dahomey.Cbor.Serialization;
using Dahomey.Cbor.Serialization.Converters;
using SurrealDb.Net.Models;

namespace SurrealDb.Net.Internals.Cbor.Converters;

internal class DurationConverter : CborConverterBase<Duration>
{
    public override Duration Read(ref CborReader reader)
    {
        var value = reader.ReadString();

        return (value is not null) ? new Duration(value) : default;
    }

    public override void Write(ref CborWriter writer, Duration value)
    {
        writer.WriteSemanticTag(CborTagConstants.TAG_DURATION);
        writer.WriteString(value.ToString());
    }
}
