using StartTime.Core;

namespace StartTime.Tests;

public class EarliestEventFinderTests
{
    private readonly EarliestEventFinder _finder = new();

    [Fact]
    public void ReturnsEarliestTimestamp_WhenMultipleEventsOccurredToday()
    {
        var now = new DateTime(2026, 9, 9, 17, 30, 0);
        var timestamps = new[]
        {
            new DateTime(2026, 9, 9, 9, 15, 0),
            new DateTime(2026, 9, 9, 6, 2, 30),
            new DateTime(2026, 9, 9, 11, 0, 0),
        };

        var result = _finder.FindEarliestToday(timestamps, now);

        Assert.Equal(new DateTime(2026, 9, 9, 6, 2, 30), result);
    }

    [Fact]
    public void IgnoresTimestampsFromOtherDays()
    {
        var now = new DateTime(2026, 9, 9, 12, 0, 0);
        var timestamps = new[]
        {
            new DateTime(2026, 9, 8, 3, 0, 0),
            new DateTime(2026, 9, 9, 8, 0, 0),
            new DateTime(2026, 9, 10, 1, 0, 0),
        };

        var result = _finder.FindEarliestToday(timestamps, now);

        Assert.Equal(new DateTime(2026, 9, 9, 8, 0, 0), result);
    }

    [Fact]
    public void ReturnsNull_WhenNoEventsOccurredToday()
    {
        var now = new DateTime(2026, 9, 9, 12, 0, 0);
        var timestamps = new[]
        {
            new DateTime(2026, 9, 8, 3, 0, 0),
            new DateTime(2026, 9, 7, 3, 0, 0),
        };

        var result = _finder.FindEarliestToday(timestamps, now);

        Assert.Null(result);
    }

    [Fact]
    public void ReturnsNull_WhenNoTimestampsGiven()
    {
        var result = _finder.FindEarliestToday(Array.Empty<DateTime>(), new DateTime(2026, 9, 9));

        Assert.Null(result);
    }

    [Fact]
    public void TreatsMidnightAsTheEarliestPossibleTimeToday()
    {
        var now = new DateTime(2026, 9, 9, 23, 59, 0);
        var timestamps = new[]
        {
            new DateTime(2026, 9, 9, 0, 0, 0),
            new DateTime(2026, 9, 9, 0, 0, 1),
        };

        var result = _finder.FindEarliestToday(timestamps, now);

        Assert.Equal(new DateTime(2026, 9, 9, 0, 0, 0), result);
    }
}
