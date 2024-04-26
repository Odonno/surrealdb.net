using Dahomey.Cbor.Attributes;

namespace SurrealDb.Embedded.InMemory.Internals;

internal class SurrealDbEmbeddedRequest
{
    //[JsonPropertyName("id")]
    //[CborProperty("id")]
    //public string Id { get; set; } = string.Empty;

    [CborProperty("method")]
    public string Method { get; set; } = string.Empty;

    [CborProperty("params")]
    [CborIgnoreIfDefault]
    public object?[]? Parameters { get; set; }
}
