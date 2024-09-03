using Space.Common.Clock.Abstractions;

namespace Space.Common.Clock;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}