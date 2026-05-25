using mhwildsdb.DTOs.Talismans.CharmRank;

namespace mhwildsdb.DTOs.Talismans;

public sealed record CreateCharmDto(
    string Name,
    ICollection<CreateCharmRankDto> Ranks);
