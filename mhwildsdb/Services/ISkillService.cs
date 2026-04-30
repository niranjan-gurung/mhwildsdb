using mhwildsdb.DTOs;

namespace mhwildsdb.Services
{
    public interface ISkillService
    {
        Task<SkillDto> CreateSkillAsync(CreateSkillDto command);
        Task<SkillDto?> GetSkillByIdAsync(Guid id);
        Task<IEnumerable<SkillDto>> GetAllSkillsAsync();
        Task UpdateSkillAsync(Guid id, UpdateSkillDto command);
        Task DeleteSkillAsync(Guid id);
    }
}
