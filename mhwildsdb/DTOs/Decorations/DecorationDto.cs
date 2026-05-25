using mhwildsdb.DTOs.Skills.SkillRank;

namespace mhwildsdb.DTOs.Decorations;

public sealed record DecorationDto(
    Guid Id,
    string Name,
    string Description,
    string Type,
    int Rarity,
    int Slot,
    ICollection<SkillRankDetailDto> Skills);
