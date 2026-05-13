using mhwildsdb.DTOs.Skills.Skill;

namespace mhwildsdb.DTOs.Armours.ArmourSet;

public sealed record ArmourSetDto(
    Guid Id,
    string Name,
    ICollection<ArmourDto> Pieces,
    SkillDto? SetBonusSkill,
    SkillDto? GroupBonusSkill);
