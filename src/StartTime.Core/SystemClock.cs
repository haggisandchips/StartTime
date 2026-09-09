namespace StartTime.Core;

public sealed class SystemClock : IClock
{
    public DateTime Now => DateTime.Now;
}
