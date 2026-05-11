using mhwildsdb.DTOs.Skills.Skill;
using mhwildsdb.DTOs.Skills.SkillRank;
using mhwildsdb.Entities.Skills;

namespace mhwildsdb.Extensions.Mapping;

public static class SkillMappingExtension
{
    public static SkillDto ToDto(this Skill skill) => 
        new(
            skill.Id,
            skill.Name,
            skill.Type,
            skill.Description,
            skill.Ranks.Select(sr => sr.ToDto()).ToList()
        );

    public static SkillRankDto ToDto(this SkillRank rank) =>
        new(rank.Id, rank.Level, rank.Description);

    public static SkillRankDetailDto ToDetailDto(this SkillRank rank) =>
        new(
            rank.Id,
            rank.Level,
            rank.Description,
            rank.Skill.Id,
            rank.Skill.Name
        );
}
