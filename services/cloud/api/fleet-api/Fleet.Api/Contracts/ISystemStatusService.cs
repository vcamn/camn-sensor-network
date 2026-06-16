using Fleet.Api.DTOs.Common;

namespace Fleet.Api.Contracts;

public interface ISystemStatusService<TDto>
    where TDto : SystemStatusDto
{
    Task DeleteStatusAsync(Guid id);

    Task<TDto?> GetStatusAsync(Guid id);

    Task<IEnumerable<TDto>> GetStatusesAsync();

    Task<TDto> CreateStatusAsync(TDto statusDto);

    Task UpdateStatusAsync(Guid id, TDto statusDto);

    Task<bool> StatusExistsAsync(Guid id);
}