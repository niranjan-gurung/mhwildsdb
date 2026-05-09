namespace mhwildsdb.Entities.Skills;

public class SkillRank : EntityBase
{
    public int Level { get; private set; }
    public string Description { get; private set; }

    // FK
    public Guid SkillId { get; private set; }

    // navigation property
    public Skill Skill { get; private set; } = null!;

    public List<Armour> Armours { get; private set; } = [];

    // TODO:
    // public List<Charm> Charms { get; private set; } = [];
    // public List<Decoration> Decorations { get; private set; } = [];

    private SkillRank()
    {
        Description = string.Empty;
        Skill = null!;
    }

    private SkillRank(int level, string description)
    {
        Level = level;
        Description = description;
    }

    public static SkillRank Create(int level, string description)
    {
        return new SkillRank(level, description);
    }

    public void Update(int level, string description)
    {
        Level = level;
        Description = description;
        UpdateLastModified();
    }
}
