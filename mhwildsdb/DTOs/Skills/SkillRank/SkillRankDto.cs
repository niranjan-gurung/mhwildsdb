namespace mhwildsdb.DTOs.Skills.SkillRank;

public sealed record SkillRankDto(
    Guid Id, 
    int Level, 
    string Description);

public sealed record SkillRankDetailDto(
    Guid Id,
    int Level,
    string Description,
    Guid SkillId,
    string SkillName);
