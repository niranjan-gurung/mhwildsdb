using mhwildsdb.DTOs.Armours;

namespace mhwildsdb.Services.Interfaces;

public interface IArmourService
{
    Task<ArmourDto> CreateArmourAsync(CreateArmourDto request);
    Task<ICollection<ArmourDto>> CreateArmourRangeAsync(ICollection<CreateArmourDto> requests);
    Task<ArmourDto> GetArmourByIdAsync(Guid id);
    Task<IEnumerable<ArmourDto>> GetAllArmoursAsync();
    Task UpdateArmourAsync(Guid id, UpdateArmourDto request);
    Task DeleteArmourAsync(Guid id);
}
