namespace mhwildsdb.DTOs.Armours;

public sealed record CreateArmourDto(
    string Name,
    string Piece,
    string Rank,
    int Rarity,
    int Defense,
    ResistancesDto Resistances,
    ICollection<int> Slots,
    ICollection<Guid> SkillRankIds);
