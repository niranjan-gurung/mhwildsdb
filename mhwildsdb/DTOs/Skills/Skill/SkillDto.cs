using mhwildsdb.DTOs.Skills.SkillRank;

namespace mhwildsdb.DTOs.Skills.Skill;

public sealed record SkillDto(
    Guid Id, 
    string Name, 
    string Type, 
    string Description,
    ICollection<SkillRankDto> Ranks);
