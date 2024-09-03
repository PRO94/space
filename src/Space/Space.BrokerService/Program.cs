using Microsoft.OpenApi.Models;
using Space.BrokerService.Entities;
using Space.Common.Clock;
using Space.Common.MassTransit;
using Space.Common.MongoDb;
using Space.Common.Settings;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

var serviceSettings = configuration
    .GetRequiredSection(nameof(ServiceSettings))
    .Get<ServiceSettings>();

services
    .AddMongo()
    .AddMongoRepository<SensorDataItem>("received_by_broker_data_items");

services.AddMassTransitWithRabbitMq();

services.AddDateTimeProvider();

services.AddEndpointsApiExplorer();
services.AddControllers();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Space.Broker.Service", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Space.Broker.Service v1"));
}

app.MapControllers();
app.UseHttpsRedirection();

app.Run();