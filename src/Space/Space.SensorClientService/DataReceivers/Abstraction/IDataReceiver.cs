using Space.SensorClientService.Dtos;

namespace Space.SensorClientService.DataReceivers.Abstraction;

public interface IDataReceiver
{
    Task<SensorDataItemDto> GetSensorData();
}