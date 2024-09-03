using Microsoft.Extensions.Options;
using Quartz;
using Space.SensorClientService.Outbox.BackgroundJobs;
using Space.SensorClientService.Settings;

namespace Space.SensorClientService.Outbox;

public class CollectSensorsDataJobSetup : IConfigureOptions<QuartzOptions>
{
    private readonly OutboxSettings _outboxOptions;
    private readonly ILogger<CollectSensorsDataJobSetup> _logger;

    public CollectSensorsDataJobSetup(
        IOptions<OutboxSettings> outboxOptions,
        ILogger<CollectSensorsDataJobSetup> logger)
    {
        _outboxOptions = outboxOptions.Value;
        _logger = logger;
    }

    public void Configure(QuartzOptions options)
    {
        const string jobName = nameof(CollectSensorsDataJob);

        options
            .AddJob<CollectSensorsDataJob>(configure => configure.WithIdentity(jobName))
            .AddTrigger(configure =>
                configure
                    .ForJob(jobName)
                    .WithSimpleSchedule(schedule =>
                        schedule.WithIntervalInSeconds(_outboxOptions.IntervalInSeconds).RepeatForever()));

        _logger.LogInformation($"{nameof(CollectSensorsDataJob)} has started data collection");
    }
}