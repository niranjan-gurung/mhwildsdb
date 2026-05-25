using mhwildsdb.DTOs.Armours;
using mhwildsdb.DTOs.Armours.ArmourSet;

namespace mhwildsdb.Services.Interfaces;

public interface IArmourSetService
{
    Task<ArmourSetDto> CreateArmourSetAsync(CreateArmourSetDto request);
    Task<ICollection<ArmourSetDto>> CreateArmourRangeAsync(ICollection<CreateArmourSetDto> requests);
    Task<ArmourSetDto> GetArmourSetByIdAsync(Guid id);
    Task<IEnumerable<ArmourSetDto>> GetAllArmourSetsAsync();
    Task UpdateArmourSetAsync(Guid id, UpdateArmourSetDto request);
    Task DeleteArmourSetAsync(Guid id);
}
