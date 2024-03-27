namespace SurrealDb.Net.Tests.Serializers.Cbor;

public class DurationConverterTests : BaseCborConverterTests
{
    [Fact]
    public async Task Serialize()
    {
        var value = new Duration("38w1d28m3s58ms255µs");

        string result = await SerializeCborBinaryAsHexaAsync(value);

        result.Should().Be("c974333877316432386d337335386d73323535c2b573");
    }

    [Fact]
    public async Task Deserialize()
    {
        var result = await DeserializeCborBinaryAsHexaAsync<Duration>(
            "c974333877316432386d337335386d73323535c2b573"
        );

        var expected = new Duration("38w1d28m3s58ms255µs");

        result.Should().Be(expected);
    }
}
