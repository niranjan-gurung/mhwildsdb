using mhwildsdb.Entities.Skills;

namespace mhwildsdb.Entities;

public class Decoration : EntityBase
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Type { get; private set; }
    public int Rarity { get; private set; }
    public int Slot { get; private set; }
    public ICollection<SkillRank> Skills { get; private set; } = [];

    private Decoration()
    {
        Name = string.Empty;
        Description = string.Empty;
        Type = string.Empty;
    }

    private Decoration(string name, string description, string type, int rarity, int slot, ICollection<SkillRank> skills)
    {
        Name = name;
        Description = description;
        Type = type;
        Rarity = rarity;
        Slot = slot;
        Skills = skills;
    }

    public static Decoration Create(string name, string description, string type, int rarity, int slot, ICollection<SkillRank> skills)
    {
        return new Decoration(name, description, type, rarity, slot, skills);
    }

    public void Update(string name, string description, string type, int rarity, int slot)
    {
        Name = name;
        Description = description;
        Type = type;
        Rarity = rarity;
        Slot = slot;

        UpdateLastModified();
    }
}
