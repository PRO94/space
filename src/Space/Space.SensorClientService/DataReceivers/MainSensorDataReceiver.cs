using Space.Common.Clock.Abstractions;
using Space.SensorClientService.DataReceivers.Abstraction;
using Space.SensorClientService.Dtos;

namespace Space.SensorClientService.DataReceivers;

public class MainSensorDataReceiver : IDataReceiver
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<MainSensorDataReceiver> _logger;

    public MainSensorDataReceiver(
        IDateTimeProvider dateTimeProvider,
        ILogger<MainSensorDataReceiver> logger)
    {
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task<SensorDataItemDto> GetSensorData()
    {
        _logger.LogInformation("Started collecting data...");
        
        // Imitation of connection to the physical sensor
        await Task.Delay(20);

        var sensorData = new SensorDataItemDto(
            TemperatureInF: new Random().Next(50, 100),
            Timestamp: _dateTimeProvider.UtcNow);

        _logger.LogInformation($"Data received from the sensor: {sensorData.ToString()}");

        return sensorData;
    }
}