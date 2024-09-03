namespace Space.Common.Contracts;

public record SensorsDataReceived(
    List<SensorDataItemReceived> Items);

public record SensorDataItemReceived(
    int Temperature,
    DateTime Timestamp,
    Guid CorrelationId
);

public record SensorDataStored(
    Guid CorrelationId);