namespace StartTime.Core;

public sealed class EarliestEventReportService
{
    private readonly IReadOnlyList<IEventTimestampSource> _sources;
    private readonly EarliestEventFinder _finder;
    private readonly IClock _clock;

    public EarliestEventReportService(IEnumerable<IEventTimestampSource> sources, EarliestEventFinder finder, IClock clock)
    {
        _sources = sources.ToList();
        _finder = finder;
        _clock = clock;
    }

    public DateTime? GetEarliestEventToday()
    {
        var allTimestamps = _sources.SelectMany(source => source.GetTimestamps());
        return _finder.FindEarliestToday(allTimestamps, _clock.Now);
    }
}
