using Dahomey.Cbor;
using SurrealDb.Net.Internals.Cbor.Converters;
using SurrealDb.Net.Models;

namespace SurrealDb.Net.Internals.Cbor;

internal static class SurrealDbCborOptions
{
    private static readonly Lazy<CborOptions> _lazy = new(CreateCborSerializerOptions);

    public static CborOptions Default => _lazy.Value;

    private static CborOptions CreateCborSerializerOptions()
    {
        var options = new CborOptions();

        options.Registry.ConverterRegistry.RegisterConverter(typeof(Thing), new ThingConverter());

        return options;
    }
}
