namespace mhwildsdb.DTOs.Skills.SkillRank;

public sealed record SkillRankDto(
    Guid Id, 
    int Level, 
    string? Name,
    string Description,
    int? SetPieceRequired);

public sealed record SkillRankDetailDto(
    Guid Id,
    int Level,
    string? Name,
    string Description,
    int? SetPieceRequired,
    Guid SkillId,
    string SkillName);
