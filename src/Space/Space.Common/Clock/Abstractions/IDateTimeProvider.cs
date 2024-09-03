namespace Space.Common.Clock.Abstractions;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}