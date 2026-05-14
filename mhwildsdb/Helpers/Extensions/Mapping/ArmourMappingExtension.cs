using mhwildsdb.DTOs.Armours;
using mhwildsdb.Entities.Armours;

namespace mhwildsdb.Helpers.Extensions.Mapping;

public static class ArmourMappingExtension
{
    public static ArmourDto ToDto(this Armour armour) =>
        new(
            armour.Id,
            armour.Name,
            armour.Piece,
            armour.Rank,
            armour.Rarity,
            armour.Defense,
            armour.Resistances.ToDto(),
            armour.Slots,
            armour.SkillRanks.Select(sr => sr.ToDetailDto()).ToList(),
            armour.ArmourSet?.ToSummaryDto()
        );
}
