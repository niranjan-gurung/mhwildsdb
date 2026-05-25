using mhwildsdb.DTOs.Skills.SkillRank;

namespace mhwildsdb.DTOs.Charms.CharmRank;

public sealed record CharmRankDto(
    Guid Id,
    string Name, 
    string Description,
    int Level,
    int Rarity,
    IReadOnlyList<SkillRankDto> Skills);