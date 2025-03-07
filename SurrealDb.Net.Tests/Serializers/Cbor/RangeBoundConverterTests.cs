﻿namespace SurrealDb.Net.Tests.Serializers.Cbor;

public class RangeBoundConverterTests : BaseCborConverterTests
{
    [Test]
    public async Task SerializeInclusiveBoundRange()
    {
        var value = RangeBound.Inclusive(14);

        string result = await SerializeCborBinaryAsHexaAsync(value);

        result.Should().Be("d8320e");
    }

    [Test]
    public async Task DeserializeInclusiveBoundRange()
    {
        var result = await DeserializeCborBinaryAsHexaAsync<RangeBound<int>>("d8320e");

        result.Should().Be(RangeBound.Inclusive(14));
    }

    [Test]
    public async Task SerializeExclusiveBoundRange()
    {
        var value = RangeBound.Exclusive("hello");

        string result = await SerializeCborBinaryAsHexaAsync(value);

        result.Should().Be("d8336568656c6c6f");
    }

    [Test]
    public async Task DeserializeExclusiveBoundRange()
    {
        var result = await DeserializeCborBinaryAsHexaAsync<RangeBound<string>>("d8336568656c6c6f");

        result.Should().Be(RangeBound.Exclusive("hello"));
    }
}
