using Quartz;
using Space.Common;
using Space.SensorClientService.DataReceivers.Abstraction;
using Space.SensorClientService.Entities;

namespace Space.SensorClientService.Outbox.BackgroundJobs;

public class CollectSensorsDataJob : IJob
{
    private readonly IRepository<SensorDataItem> _repository;
    private readonly IDataReceiver _dataReceiver;
    private readonly ILogger<CollectSensorsDataJob> _logger;

    public CollectSensorsDataJob(
        IRepository<SensorDataItem> repository,
        IDataReceiver dataReceiver,
        ILogger<CollectSensorsDataJob> logger)
    {
        _repository = repository;
        _dataReceiver = dataReceiver;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var data = await _dataReceiver.GetSensorData();

        var entity = data.ToEntity(Guid.NewGuid());

        await _repository.CreateAsync(entity);

        _logger.LogInformation($"Sensor data is collected: Temperature: {entity.Temperature}, Timestamp: {entity.Timestamp}");
    }
}