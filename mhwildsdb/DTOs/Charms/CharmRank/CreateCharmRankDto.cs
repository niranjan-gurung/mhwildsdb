namespace mhwildsdb.DTOs.Talismans.CharmRank;

public sealed record CreateCharmRankDto(
    string Name,
    string Description,
    int Level,
    int Rarity,
    ICollection<Guid> Skills);
