using mhwildsdb.DTOs.Skills.Skill;

namespace mhwildsdb.Services;

public interface ISkillService
{
    Task<SkillDto> CreateSkillAsync(CreateSkillDto request);
    Task<ICollection<SkillDto>> CreateSkillRangeAsync(ICollection<CreateSkillDto> requests);
    Task<SkillDto> GetSkillByIdAsync(Guid id);
    Task<IEnumerable<SkillDto>> GetAllSkillsAsync();
    Task UpdateSkillAsync(Guid id, UpdateSkillDto request);
    Task DeleteSkillAsync(Guid id);
}
