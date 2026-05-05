namespace mhwildsdb.DTOs.Skills.Skill;

public sealed record CreateSkillDto(
    string Name, 
    string Type, 
    string Description,
    ICollection<CreateSkillRankDto> Ranks);
