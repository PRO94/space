using Space.SensorClientService.Dtos;
using Space.SensorClientService.Entities;

namespace Space.SensorClientService;

public static class Extensions
{
    /// <summary>
    /// Convert temperature in Fahrenheits units to Celsius units
    /// </summary>
    /// <param name="temperatureInFahrenheits"></param>
    /// <returns>Temperature value in Celsius units</returns>
    public static int FahrenheitToCelsius(this int temperatureInFahrenheits)
    {
        return (temperatureInFahrenheits - 32) * 5 / 9;
    }

    public static SensorDataItem ToEntity(this SensorDataItemDto sensorDataItem, Guid id)
    {
        return new SensorDataItem
        {
            Id = id,
            Temperature = sensorDataItem.TemperatureInF.FahrenheitToCelsius(),
            Timestamp = sensorDataItem.Timestamp
        };
    }
}