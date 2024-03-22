using Dahomey.Cbor;
using Dahomey.Cbor.Serialization;
using Dahomey.Cbor.Serialization.Converters;
using SurrealDb.Net.Models;

namespace SurrealDb.Net.Internals.Cbor.Converters;

// TODO : Register converters
// options.Registry.ConverterRegistry.RegisterConverter()

internal class ThingConverter : CborConverterBase<Thing>
{
    public override Thing Read(ref CborReader reader)
    {
        bool hasSemanticTag = reader.TryReadSemanticTag(out var semanticTag);
        if (!hasSemanticTag || semanticTag != CborTagConstants.TAG_RECORDID)
        {
            throw new CborException("No record id detected...");
        }

        return reader.GetCurrentDataItemType() switch
        {
            CborDataItemType.String => new Thing(reader.ReadString()!),
            CborDataItemType.Array => ReadThingFromArray(ref reader),
            _
                => throw new CborException(
                    "Expected a CBOR text data type, or a CBOR array with 2 elements"
                )
        };

        //reader.ReadBeginArray();

        //ReadOnlySpan<byte> bytes = reader.ReadByteString();
        //return new Guid(bytes);
    }

    private static Thing ReadThingFromArray(ref CborReader reader)
    {
        reader.ReadBeginArray();

        throw new Exception("TODO"); // TODO

        //int len = reader.re();

        //if (len != 2)
        //{
        //    throw new CborException(
        //        "Expected a CBOR text data type, or a CBOR array with 2 elements"
        //    );
        //}

        //ReadBeginArray();
        //int num = ReadSize();
        //arrayReader.ReadBeginArray(num, ref context);
        //while (num > 0 || (num < 0 && GetCurrentDataItemType() != CborDataItemType.Break))
        //{
        //    arrayReader.ReadArrayItem(ref this, ref context);
        //    num--;
        //}

        //_state = CborReaderState.Start;
    }

    public override void Write(ref CborWriter writer, Thing value)
    {
        writer.WriteSemanticTag(CborTagConstants.TAG_RECORDID);

        writer.WriteBeginArray(2);

        writer.WriteString(value.Table);
        writer.WriteString(value.Id);

        writer.WriteEndArray(-1);
    }
}
