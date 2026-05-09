using mhwildsdb.DTOs.Skills.SkillRank;

namespace mhwildsdb.DTOs.Armours;

public sealed record ArmourDto(
    Guid Id,
    string Name,
    string Piece,
    string Rank,
    int Rarity,
    int Defense,
    ResistancesDto Resistances,
    ICollection<int> Slots,
    ICollection<SkillRankDetailDto> Skills);
