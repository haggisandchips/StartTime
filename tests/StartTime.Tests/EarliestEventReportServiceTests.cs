using StartTime.Core;

namespace StartTime.Tests;

internal sealed class FakeClock(DateTime now) : IClock
{
    public DateTime Now { get; } = now;
}

internal sealed class FakeEventTimestampSource(params DateTime[] timestamps) : IEventTimestampSource
{
    public IEnumerable<DateTime> GetTimestamps() => timestamps;
}

public class EarliestEventReportServiceTests
{
    [Fact]
    public void CombinesTimestampsAcrossAllSources_AndReturnsTheOverallEarliestToday()
    {
        var now = new DateTime(2026, 9, 9, 18, 0, 0);
        var sources = new IEventTimestampSource[]
        {
            new FakeEventTimestampSource(new DateTime(2026, 9, 9, 9, 0, 0), new DateTime(2026, 9, 8, 1, 0, 0)),
            new FakeEventTimestampSource(new DateTime(2026, 9, 9, 5, 30, 0)),
        };
        var service = new EarliestEventReportService(sources, new EarliestEventFinder(), new FakeClock(now));

        var result = service.GetEarliestEventToday();

        Assert.Equal(new DateTime(2026, 9, 9, 5, 30, 0), result);
    }

    [Fact]
    public void ReturnsNull_WhenNoSourceHasEventsToday()
    {
        var now = new DateTime(2026, 9, 9, 18, 0, 0);
        var sources = new IEventTimestampSource[]
        {
            new FakeEventTimestampSource(new DateTime(2026, 9, 1, 9, 0, 0)),
        };
        var service = new EarliestEventReportService(sources, new EarliestEventFinder(), new FakeClock(now));

        var result = service.GetEarliestEventToday();

        Assert.Null(result);
    }

    [Fact]
    public void ReturnsNull_WhenThereAreNoSources()
    {
        var service = new EarliestEventReportService(
            Array.Empty<IEventTimestampSource>(), new EarliestEventFinder(), new FakeClock(DateTime.Now));

        var result = service.GetEarliestEventToday();

        Assert.Null(result);
    }
}
