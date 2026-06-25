using Fleet.Api.DTOs.Device;

namespace Fleet.Api.Contracts;

public interface IDeviceService
{
    Task<IEnumerable<DeviceDto>> GetDevicesAsync();

    Task<DeviceDto?> GetDeviceAsync(Guid id);

    Task<DeviceDto> CreateDeviceAsync(CreateDeviceDto createDeviceDto);

    Task UpdateDeviceAsync(Guid id, DeviceDto deviceDto);

    Task DeleteDeviceAsync(Guid id);

    Task<bool> DeviceExistsAsync(Guid id);
}
