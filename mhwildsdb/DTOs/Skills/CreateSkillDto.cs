using mhwildsdb.DTOs.Skills.SkillRank;

namespace mhwildsdb.DTOs.Skills;

public sealed record CreateSkillDto(
    string Name, 
    string Type, 
    string? Description,
    ICollection<CreateSkillRankDto> Ranks);
