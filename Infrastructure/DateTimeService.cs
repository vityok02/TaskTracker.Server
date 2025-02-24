using Application.Abstract.Interfaces;

namespace Infrastructure;

public class DateTimeService : IDateTimeProvider
{
    public DateTime GetCurrentTime()
        => DateTime.UtcNow;
}
