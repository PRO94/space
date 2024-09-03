using Quartz;
using Space.SensorClientService.Outbox.BackgroundJobs;
using Space.SensorClientService.Settings;
using Microsoft.Extensions.Options;

namespace Space.SensorClientService.Outbox;

public class SyncJobManager
{
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly ILogger<SyncJobManager> _logger;
    private IScheduler _scheduler;
    private readonly OutboxSettings _outboxSettings;

    private IJobDetail _jobDetail;
    private ITrigger _jobTrigger;
    private const string JobName = nameof(SyncSensorsDataJob);
    private const string JobTrigger = $"{JobName}_Trigger";

    public SyncJobManager(
        IOptions<OutboxSettings> outboxSettings,
        ISchedulerFactory schedulerFactory,
        ILogger<SyncJobManager> logger)
    {
        _schedulerFactory = schedulerFactory;
        _outboxSettings = outboxSettings.Value;
        _logger = logger;
    }

    public async Task StartSyncJob()
    {
        if (_scheduler == null)
        {
            _scheduler = await _schedulerFactory.GetScheduler();
        }

        _jobDetail = JobBuilder
            .Create<SyncSensorsDataJob>()
            .WithIdentity(JobName)
            .Build();

        _jobTrigger = TriggerBuilder
            .Create()
            .WithIdentity(JobTrigger)
            .StartNow()
            .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(_outboxSettings.IntervalInSeconds)
                .RepeatForever())
            .Build();

        await _scheduler.ScheduleJob(_jobDetail, _jobTrigger);
        await StartScheduler(_scheduler);

        _logger.LogInformation($"{JobName} Scheduler started.");
    }

    public async Task ResumeSyncJob()
    {
        if (_scheduler != null && !_scheduler.IsShutdown && _jobDetail != null)
        {
            await _scheduler.ScheduleJob(_jobDetail, _jobTrigger);
            await StartScheduler(_scheduler);

            _logger.LogWarning($"{JobName} resumed.");
        }
    }

    public async Task PauseSyncJob()
    {
        if (_scheduler != null && !_scheduler.IsShutdown)
        {
            await _scheduler.DeleteJob(new JobKey(JobName));
        }
        _logger.LogWarning($"{JobName} stopped.");
    }

    private async Task StartScheduler(IScheduler scheduler)
    {
        if (!scheduler.IsStarted)
        {
            await scheduler.Start();
        }
    }
}