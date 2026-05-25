using mhwildsdb.DTOs.Skills.SkillRank;

namespace mhwildsdb.DTOs.Skills;

public sealed record SkillDto(
    Guid Id, 
    string Name, 
    string Type, 
    string? Description,
    IReadOnlyList<SkillRankDto> Ranks);
