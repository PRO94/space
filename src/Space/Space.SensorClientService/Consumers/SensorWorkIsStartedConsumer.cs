using MassTransit;
using Space.Common.Contracts;
using Space.SensorClientService.Outbox;

namespace Space.SensorClientService.Consumers;

public class SensorWorkIsStartedConsumer : IConsumer<SensorWorkIsStarted>
{
    private readonly SyncJobManager _syncJobManager;
    private readonly ILogger<SensorWorkIsStartedConsumer> _logger;

    public SensorWorkIsStartedConsumer(
        SyncJobManager syncJobManager,
        ILogger<SensorWorkIsStartedConsumer> logger)
    {
        _syncJobManager = syncJobManager;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SensorWorkIsStarted> context)
    {
        await _syncJobManager.StartSyncJob();
        _logger.LogWarning("Resumed sync background job");
    }
}