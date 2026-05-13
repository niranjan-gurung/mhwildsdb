namespace mhwildsdb.DTOs.Armours.ArmourSet;

public sealed record CreateArmourSetDto(
    string Name, 
    ICollection<Guid> ArmourPieceIds,
    Guid? SetBonusSkillId,
    Guid? GroupBonusSkillId);
