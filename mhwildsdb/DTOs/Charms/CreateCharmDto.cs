using mhwildsdb.DTOs.Charms.CharmRank;

namespace mhwildsdb.DTOs.Charms;

public sealed record CreateCharmDto(
    string Name,
    ICollection<CreateCharmRankDto> Ranks);
