namespace SurrealDb.Net.Tests.Serializers.Cbor;

public class DateTimeConverterTests : BaseCborConverterTests
{
    [Fact]
    public async Task Serialize()
    {
        string result = await SerializeCborBinaryAsHexaAsync(
            DateTime.Parse("2024-03-24T13:30:26.1623225Z").ToUniversalTime()
        );

        result.Should().Be("c0781c323032342d30332d32345431333a33303a32362e313632333232355a");
    }

    [Fact]
    public async Task Deserialize()
    {
        var result = await DeserializeCborBinaryAsHexaAsync<DateTime>(
            "c0781c323032342d30332d32345431333a33303a32362e313632333232355a"
        );

        var expected = DateTime.Parse("2024-03-24T13:30:26.1623225Z").ToUniversalTime();

        result.Should().Be(expected);
    }
}
