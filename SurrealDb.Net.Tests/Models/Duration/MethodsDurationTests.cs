namespace SurrealDb.Net.Tests.Models;

public class MethodsDurationTests
{
    [Test]
    public void ShouldConvertDurationToTimeSpan()
    {
        var duration = new Duration("1h30m20s1350ms");
        var timeSpan = duration.ToTimeSpan();

        timeSpan.Should().Be(new TimeSpan(0, 1, 30, 21, 350));
    }

    [Test]
    public void ShouldConvertDurationToTimeSpanWithNanoseconds()
    {
        var duration = new Duration("25us918ns");
        var timeSpan = duration.ToTimeSpan();

        timeSpan.Ticks.Should().Be(259);
    }

    [Test]
    public void ShouldDisplayDurationString()
    {
        var duration = new Duration("1h30m");
        duration.ToString().Should().Be("1h30m");
    }
}
