using System.Diagnostics.Eventing.Reader;
using System.Runtime.Versioning;

namespace StartTime.Core;

/// <summary>Reads event timestamps for the current day from a local Windows event log, using a server-side time filter so full log scans are avoided.</summary>
[SupportedOSPlatform("windows")]
public sealed class SystemEventLogTimestampSource : IEventTimestampSource
{
    private readonly string _logName;
    private readonly IClock _clock;

    public SystemEventLogTimestampSource(string logName, IClock? clock = null)
    {
        _logName = logName;
        _clock = clock ?? new SystemClock();
    }

    public IEnumerable<DateTime> GetTimestamps()
    {
        var todayUtc = _clock.Now.Date.ToUniversalTime();
        var query = $"*[System[TimeCreated[@SystemTime>='{todayUtc:yyyy-MM-ddTHH:mm:ss.fffZ}']]]";
        var eventLogQuery = new EventLogQuery(_logName, PathType.LogName, query)
        {
            ReverseDirection = false,
        };

        using var reader = new EventLogReader(eventLogQuery);

        EventRecord? record;
        while ((record = reader.ReadEvent()) != null)
        {
            using (record)
            {
                if (record.TimeCreated.HasValue)
                    yield return record.TimeCreated.Value;
            }
        }
    }
}
