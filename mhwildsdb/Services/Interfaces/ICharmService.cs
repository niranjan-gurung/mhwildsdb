using mhwildsdb.DTOs.Charms;

namespace mhwildsdb.Services.Interfaces;

public interface ICharmService
{
    Task<CharmDto> CreateCharmAsync(CreateCharmDto request);
    Task<ICollection<CharmDto>> CreateCharmRangeAsync(ICollection<CreateCharmDto> requests);
    Task<CharmDto> GetCharmByIdAsync(Guid id);
    Task<IEnumerable<CharmDto>> GetAllCharmsAsync();
    Task UpdateCharmAsync(Guid id, UpdateCharmDto request);
    Task DeleteCharmAsync(Guid id);
}
