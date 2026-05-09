namespace mhwildsdb.Entities.Skills;

public class Skill : EntityBase
{
    public string Name { get; private set; }
    public string Type { get; private set; }
    public string Description { get; private set; }
    public ICollection<SkillRank> Ranks { get; private set; } = [];

    private Skill()
    {
        Name = string.Empty;
        Type = string.Empty;
        Description = string.Empty;
    }

    private Skill(string name, string type, string description, ICollection<SkillRank> ranks)
    {
        Name = name;
        Type = type;
        Description = description;
        Ranks = ranks;
    }

    public static Skill Create(string name, string type, string description, ICollection<SkillRank> ranks)
    {
        return new Skill(name, type, description, ranks);
    }

    public void Update(string name, string type, string description)
    {
        Name = name;
        Type = type;
        Description = description;

        UpdateLastModified();
    }
}
