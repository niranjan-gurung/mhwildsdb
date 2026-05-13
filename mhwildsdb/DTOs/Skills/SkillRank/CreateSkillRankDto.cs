namespace mhwildsdb.DTOs.Skills.SkillRank;

public sealed record CreateSkillRankDto(
    int Level, 
    string Description,
    string? Name = null,
    int? SetPieceRequired = null);
