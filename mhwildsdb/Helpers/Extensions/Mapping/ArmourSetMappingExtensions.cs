using mhwildsdb.DTOs.Armours.ArmourSet;
using mhwildsdb.Entities.Armours;

namespace mhwildsdb.Helpers.Extensions.Mapping;

public static class ArmourSetMappingExtensions
{
    public static ArmourSetSummaryDto ToSummaryDto(this ArmourSet armourSet) =>
        new(armourSet.Id, armourSet.Name);

    public static ArmourSetDto ToDto(this ArmourSet armourSet) =>
        new(
            armourSet.Id,
            armourSet.Name,
            armourSet.Pieces.Select(p => p.ToDto()).ToList(),
            armourSet.SetBonusSkill?.ToDto(),
            armourSet.GroupBonusSkill?.ToDto()
        );
}
