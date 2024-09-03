using Space.Common;

namespace Space.SensorClientService.Entities;

public class SensorDataItem : IEntity
{
    public Guid Id { get; init; }

    public int Temperature { get; set; }

    public DateTime Timestamp { get; set; }

    //public SyncStatus Status { get; set; }
}

// TODO: Could be used for managing synchromization between servers
public enum SyncStatus
{
    NotSent = 0,
    Sent = 1,
    Synchronized = 2
}