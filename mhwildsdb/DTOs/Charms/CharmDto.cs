using mhwildsdb.DTOs.Charms.CharmRank;

namespace mhwildsdb.DTOs.Charms;

public sealed record CharmDto(
    Guid Id,
    string Name,
    IReadOnlyList<CharmRankDto> Ranks);