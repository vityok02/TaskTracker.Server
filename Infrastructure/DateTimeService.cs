using Application.Abstract.Interfaces;

namespace Infrastructure;

public class DateTimeService : IDateTimeService
{
    public DateTime GetCurrentTime()
        => DateTime.UtcNow;
}
