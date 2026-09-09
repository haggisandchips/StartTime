namespace StartTime.Core;

/// <summary>Supplies the creation timestamps of log entries from one event source.</summary>
public interface IEventTimestampSource
{
    IEnumerable<DateTime> GetTimestamps();
}
