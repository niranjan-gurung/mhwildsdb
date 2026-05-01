namespace mhwildsdb.Entities.Skills;

public class Skill : EntityBase
{
    public string Name { get; private set; }
    public string Type { get; private set; }
    public string Description { get; private set; }
    //public List<SkillRank> Ranks { get; set; } = [];

    private Skill() 
    {
        Name = string.Empty;
        Type = string.Empty;
        Description = string.Empty;
    }

    private Skill(string name, string type, string description)
    {
        Name = name;
        Type = type;
        Description = description;
    }

    public static Skill Create(string name, string type, string description)
    {
        return new Skill(name, type, description);
    }

    public void Update(string name, string type, string description)
    {
        Name = name;
        Type = type;
        Description = description;

        UpdateLastModified();
    }

    //private static void Validate(string name, string type, string description)
    //{
    //    // perform validation here...
    //    // ...
    //}
}
