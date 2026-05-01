namespace mhwildsdb.Entities.Skills;

public class SkillRank : EntityBase
{
    public int Level { get; private set; }
    public string? Description { get; private set; }
    public int SkillId { get; private set; }
    public Skill Skill { get; private set; }
    // public List<Armour> Armours { get; private set; } = [];
    // public List<Charm> Charms { get; private set; } = [];
    // public List<Decoration> Decorations { get; private set; } = [];
    
    private SkillRank()
    {
        Description = string.Empty;
        Skill = null!;
    }
    private SkillRank(int level, string description, int skillId)
    {
        Level = level;
        Description = description;
        SkillId = skillId;
    }
    public static SkillRank Create(int level, string description, int skillId)
    {
        return new SkillRank(level, description, skillId);
    }
    public void Update(int level, string description)
    {
        Level = level;
        Description = description;
        UpdateLastModified();
    }
}
