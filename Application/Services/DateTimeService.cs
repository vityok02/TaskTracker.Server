using Application.Abstract.Interfaces;

namespace Application.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime GetCurrentTime()
        => DateTime.UtcNow;
}
