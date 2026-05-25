using mhwildsdb.Entities.Skills;

namespace mhwildsdb.Entities.Talismans;

public class CharmRank : EntityBase
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int Level { get; private set; }
    public int Rarity { get; private set; }
    public Guid CharmId { get; private set; }    // FK
    public Charm Charm { get; private set; } = null!;   // navigation property
    public ICollection<SkillRank> Skills { get; private set; } = [];

    private CharmRank()
    {
        Name = string.Empty;
        Description = string.Empty;
    }

    private CharmRank(string name, string description, int level, int rarity, ICollection<SkillRank> skills)
    {
        Name = name;
        Description = description;
        Level = level;
        Rarity = rarity;
        Skills = skills;
    }

    public static CharmRank Create(string name, string description, int level, int rarity, ICollection<SkillRank> skills)
    {
        return new CharmRank(name, description, level, rarity, skills);
    }

    public void Update(string name, string description, int level, int rarity)
    {
        Name = name;
        Description = description;
        Level = level;
        Rarity = rarity;

        UpdateLastModified();
    }
}
