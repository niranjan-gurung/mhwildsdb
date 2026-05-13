using mhwildsdb.DTOs.Armours.ArmourSet;
using mhwildsdb.Entities.Armours;

namespace mhwildsdb.Extensions.Mapping;

public static class ArmourSetMappingExtensions
{
    public static ArmourSetSummaryDto ToSummaryDto(this ArmourSet armourSet) =>
        new(armourSet.Id, armourSet.Name);
}
