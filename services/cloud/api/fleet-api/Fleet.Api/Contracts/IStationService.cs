using Fleet.Api.DTOs.Station;
using Fleet.Domain.Entities;

namespace Fleet.Api.Contracts;

public interface IStationService
{
    Task<IEnumerable<StationDto>> GetStationsAsync();

    Task<StationDto?> GetStationAsync(Guid id);

    Task<StationDto> CreateStationAsync(CreateStationDto createStationDto);

    Task UpdateStationAsync(Guid id, StationDto stationDto);

    Task DeleteStationAsync(Guid id);

    Task<bool> StationExistsAsync(Guid id);

    Task<StationStatus?> GetStationStatusByCodeAsync(string stationStatusCode);
}
