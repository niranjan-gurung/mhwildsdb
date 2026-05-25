using mhwildsdb.DTOs.Talismans.CharmRank;

namespace mhwildsdb.DTOs.Talismans;

public sealed record CharmDto(
    Guid Id,
    string Name,
    IReadOnlyList<CharmRankDto> Ranks);