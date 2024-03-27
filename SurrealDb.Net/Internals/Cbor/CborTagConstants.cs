namespace SurrealDb.Net.Internals.Cbor;

internal static class CborTagConstants
{
    public const ulong TAG_DATETIME = 0;
    public const ulong TAG_UUID = 7;
    public const ulong TAG_DECIMAL = 8;
    public const ulong TAG_DURATION = 9;
    public const ulong TAG_RECORDID = 10;
    public const ulong TAG_GEOMETRY_POINT = 88;
    public const ulong TAG_GEOMETRY_LINE = 89;
    public const ulong TAG_GEOMETRY_POLYGON = 90;
    public const ulong TAG_GEOMETRY_MULTIPOINT = 91;
    public const ulong TAG_GEOMETRY_MULTILINE = 92;
    public const ulong TAG_GEOMETRY_MULTIPOLYGON = 93;
    public const ulong TAG_GEOMETRY_COLLECTION = 94;
}
