using mhwildsdb.Persistance;

namespace mhwildsdb.Services;

public class ArmourService(
    MhwildsDbContext _context,
    ILogger<SkillService> _logger) : IArmourService
{
}
