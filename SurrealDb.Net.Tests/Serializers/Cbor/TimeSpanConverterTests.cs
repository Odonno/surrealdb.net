namespace SurrealDb.Net.Tests.Serializers.Cbor;

public class TimeSpanConverterTests : BaseCborConverterTests
{
    [Fact]
    public async Task Serialize()
    {
        var value = new TimeSpan(267, 0, 28, 3, 58, 255);

        string result = await SerializeCborBinaryAsHexaAsync(value);

        result.Should().Be("c974333877316432386d337335386d73323535c2b573");
    }

    [Fact]
    public async Task Deserialize()
    {
        var result = await DeserializeCborBinaryAsHexaAsync<TimeSpan>(
            "c974333877316432386d337335386d73323535c2b573"
        );

        var expected = new TimeSpan(267, 0, 28, 3, 58, 255);

        result.Should().Be(expected);
    }
}
