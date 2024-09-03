using MassTransit;
using Space.BrokerService.Entities;
using Space.Common;
using Space.Common.Clock.Abstractions;
using Space.Common.Contracts;

namespace Space.BrokerService.Consumers;

public class SensorDataReceivedConsumer : IConsumer<SensorsDataReceived>
{
    private readonly IRepository<SensorDataItem> _repository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<SensorDataReceivedConsumer> _logger;

    public SensorDataReceivedConsumer(
        IRepository<SensorDataItem> repository,
        IDateTimeProvider dateTimeProvider,
        ILogger<SensorDataReceivedConsumer> logger)
    {
        _repository = repository;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SensorsDataReceived> context)
    {
        var message = context.Message;
        var items = message.Items;

        foreach (var item in items)
        {
            var newSensorData = new SensorDataItem(
                item.CorrelationId,
                item.Temperature,
                item.Timestamp,
                _dateTimeProvider.UtcNow
            );

            await _repository.CreateAsync(newSensorData);

            _logger.LogInformation($"Sensor data item created:\n \t{newSensorData.ToString()} \n");
        }
    }
}