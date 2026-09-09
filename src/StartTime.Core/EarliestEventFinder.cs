namespace StartTime.Core;

public sealed class EarliestEventFinder
{
    /// <summary>Returns the earliest timestamp that falls on the same calendar day as <paramref name="referenceNow"/>, or null if none do.</summary>
    public DateTime? FindEarliestToday(IEnumerable<DateTime> timestamps, DateTime referenceNow)
    {
        var today = referenceNow.Date;
        DateTime? earliest = null;

        foreach (var timestamp in timestamps)
        {
            if (timestamp.Date != today)
                continue;

            if (earliest is null || timestamp < earliest)
                earliest = timestamp;
        }

        return earliest;
    }
}
