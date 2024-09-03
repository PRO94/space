namespace Space.SensorClientService.Dtos;

public record SensorDataItemDto(int TemperatureInF, DateTime Timestamp)
{
    public override string ToString()
    {
        return $"Temperature - {TemperatureInF} *F, TimeStamp - {Timestamp}";
    }
}