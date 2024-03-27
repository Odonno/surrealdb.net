namespace SurrealDb.Net.Tests.Serializers.Cbor;

public class DateOnlyConverterTests : BaseCborConverterTests
{
    [Fact]
    public async Task Serialize()
    {
        string result = await SerializeCborBinaryAsHexaAsync(DateOnly.Parse("2024-03-24"));

        result.Should().Be("c0781c323032342d30332d32345430303a30303a30302e303030303030305a");
    }

    [Fact]
    public async Task Deserialize()
    {
        var result = await DeserializeCborBinaryAsHexaAsync<DateOnly>(
            "c0781c323032342d30332d32345430303a30303a30302e303030303030305a"
        );

        var expected = DateOnly.Parse("2024-03-24");

        result.Should().Be(expected);
    }
}
