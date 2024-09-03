using MassTransit;
using Space.Common.Contracts;
using Space.SensorClientService.Outbox;

namespace Space.SensorClientService.Consumers;

public class SensorWorkIsStoppedConsumer : IConsumer<SensorWorkIsStopped>
{
    private readonly SyncJobManager _syncJobManager;
    private readonly ILogger<SensorWorkIsStoppedConsumer> _logger;

    public SensorWorkIsStoppedConsumer(
        SyncJobManager syncJobManager,
        ILogger<SensorWorkIsStoppedConsumer> logger)
    {
        _syncJobManager = syncJobManager;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SensorWorkIsStopped> context)
    {
        await _syncJobManager.PauseSyncJob();
        _logger.LogWarning("Stopped sync background job");
    }
}