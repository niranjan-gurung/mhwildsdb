namespace mhwildsdb.Entities.Skills;

public class SkillRank : EntityBase
{
    public int Level { get; private set; }
    public string Description { get; private set; }
    public string? Name { get; private set; }
    public int? SetPieceRequired { get; private set; }

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

    private SkillRank(int level, string description, string? name, int? setPieceRequired)
    {
        Level = level;
        Description = description;
        Name = name;
        SetPieceRequired = setPieceRequired;
    }

    public static SkillRank Create(int level, string description, string? name = null, int? setPieceRequired = null)
    {
        return new SkillRank(level, description, name, setPieceRequired);
    }

    public void Update(int level, string description, string? name = null)
    {
        Level = level;
        Description = description;
        Name = name;
        UpdateLastModified();
    }
}
