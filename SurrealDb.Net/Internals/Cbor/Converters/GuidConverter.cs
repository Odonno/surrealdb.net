using Dahomey.Cbor;
using Dahomey.Cbor.Serialization;
using Dahomey.Cbor.Serialization.Converters;

namespace SurrealDb.Net.Internals.Cbor.Converters;

internal class GuidConverter : CborConverterBase<Guid>
{
    public override Guid Read(ref CborReader reader)
    {
        var value = reader.ReadString();

        if (value is null)
        {
            return default;
        }

        return Guid.TryParse(value, out var guid)
            ? guid
            : throw new CborException("Expected a valid UUID value");
    }

    public override void Write(ref CborWriter writer, Guid value)
    {
        writer.WriteSemanticTag(CborTagConstants.TAG_UUID);
        writer.WriteString(value.ToString());
    }
}
