using Space.Common.MassTransit;
using Space.Common.Settings;
using Microsoft.OpenApi.Models;
using Space.SensorClientService.DataReceivers.Abstraction;
using Space.SensorClientService.DataReceivers;
using Space.Common.MongoDb;
using Space.SensorClientService.Entities;
using Space.SensorClientService.Settings;
using Space.Common.Clock;
using Space.SensorClientService.Outbox;
using Quartz;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

var serviceSettings = configuration
    .GetRequiredSection(nameof(ServiceSettings))
    .Get<ServiceSettings>();

services
    .AddMongo()
    .AddMongoRepository<SensorDataItem>("sensor_client_data_items");

services.AddMassTransitWithRabbitMq();

services.AddDateTimeProvider();

services.AddTransient<IDataReceiver, MainSensorDataReceiver>();

// OutBox & Jobs
services.Configure<OutboxSettings>(configuration.GetSection(nameof(OutboxSettings)));
services.AddQuartz(options => { options.UseMicrosoftDependencyInjectionJobFactory(); });
services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
services.ConfigureOptions<CollectSensorsDataJobSetup>();
services.AddSingleton<SyncJobManager>();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Space.SensorClient.Service", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Space.SensorClient.Service v1"));
}

app.UseHttpsRedirection();

var jobManager = app.Services.GetRequiredService<SyncJobManager>();
await jobManager.StartSyncJob();

await app.RunAsync();