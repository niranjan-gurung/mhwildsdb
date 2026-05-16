using mhwildsdb.DTOs.Armours.ArmourSet;
using mhwildsdb.Persistance;

namespace mhwildsdb.Services;

public class ArmourSetService(
    MhwildsDbContext _context,
    ILogger<ArmourSetService> _logger) : IArmourSetService
{
    public Task<ArmourSetDto> CreateArmourSetAsync(CreateArmourSetDto request)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ArmourSetDto>> GetAllArmourSetsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ArmourSetDto> GetArmourSetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateArmourSetAsync(Guid id, UpdateArmourSetDto request)
    {
        throw new NotImplementedException();
    }

    public Task DeleteArmourSetAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
