using mhwildsdb.DTOs.Charms;
using mhwildsdb.DTOs.Charms.CharmRank;
using mhwildsdb.Entities.Charms;

namespace mhwildsdb.Helpers.Extensions.Mapping;

public static class CharmMappingExtension
{
    public static CharmDto ToDto(this Charm charm) =>
        new(
            charm.Id,
            charm.Name,
            charm.Ranks.Select(r => r.ToDto()).ToList()
        );

    public static CharmRankDto ToDto(this CharmRank charmRank) =>
        new(
            charmRank.Id,
            charmRank.Name,
            charmRank.Description,
            charmRank.Level,
            charmRank.Rarity,
            charmRank.Skills.Select(s => s.ToDto()).ToList()
        );
}
