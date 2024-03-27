namespace SurrealDb.Net.Tests.Serializers.Cbor;

public class TimeOnlyConverterTests : BaseCborConverterTests
{
    [Fact]
    public async Task Serialize()
    {
        var value = new TimeOnly(0, 28, 3, 58, 255);

        string result = await SerializeCborBinaryAsHexaAsync(value);

        result.Should().Be("c96f32386d337335386d73323535c2b573");
    }

    [Fact]
    public async Task Deserialize()
    {
        var result = await DeserializeCborBinaryAsHexaAsync<TimeOnly>(
            "c96f32386d337335386d73323535c2b573"
        );

        var expected = new TimeOnly(0, 28, 3, 58, 255);

        result.Should().Be(expected);
    }
}
