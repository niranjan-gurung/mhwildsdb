using mhwildsdb.DTOs.Skills;

namespace mhwildsdb.DTOs.Armours.ArmourSet;

public sealed record ArmourSetDto(
    Guid Id,
    string Name,
    IReadOnlyList<ArmourDto> Pieces,
    SkillDto? SetBonusSkill,
    SkillDto? GroupBonusSkill);
