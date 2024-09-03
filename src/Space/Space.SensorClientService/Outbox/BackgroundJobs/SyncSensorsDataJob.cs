using MassTransit;
using Microsoft.Extensions.Options;
using Quartz;
using Space.Common;
using Space.Common.Contracts;
using Space.SensorClientService.Entities;
using Space.SensorClientService.Settings;

namespace Space.SensorClientService.Outbox.BackgroundJobs;

public class SyncSensorsDataJob : IJob
{
    private readonly IRepository<SensorDataItem> _repository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<SyncSensorsDataJob> _logger;
    private readonly OutboxSettings _outboxSettings;

    public SyncSensorsDataJob(
        IOptions<OutboxSettings> outboxSettings,
        IRepository<SensorDataItem> repository,
        IPublishEndpoint publishEndpoint,
        ILogger<SyncSensorsDataJob> logger)
    {
        _outboxSettings = outboxSettings.Value;
        _repository = repository;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var data = await _repository.GetManyAsync(
            //filter: item => item.Status == SyncStatus.NotSent,
            amount: _outboxSettings.BatchSize);

        if (data == null || !data.Any())
        {
            return;
        }

        await _publishEndpoint.Publish(
            new SensorsDataReceived(data.Select(d => new SensorDataItemReceived(d.Temperature, d.Timestamp, d.Id)).ToList()));

        // TODO: it'd be great to make a transactional approach here
        // with sync Statuses like NotSent, Sent, Synchronized instead of just removing data from the DB

        var ids = data.Select(e => e.Id).ToList();
        await _repository.RemoveManyAsync(ids);
    }
}