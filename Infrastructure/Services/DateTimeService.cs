using Application.Abstract.Interfaces;

namespace Infrastructure.Services;

public class DateTimeService : IDateTimeProvider
{
    public DateTime GetCurrentTime()
        => DateTime.UtcNow;
}
