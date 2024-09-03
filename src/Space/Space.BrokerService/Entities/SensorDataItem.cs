using Space.Common;

namespace Space.BrokerService.Entities;

public record SensorDataItem (
    Guid Id,
    int Temperature,
    DateTime Timestamp,
    DateTime CreatedAt) : IEntity
{ 
    public override string ToString()
    {
        return $"ID: {Id}, Temperature: {Temperature} *C, Timestamp: {Timestamp}";
    }
}