namespace mhwildsdb.Entities.Charms;

public class Charm : EntityBase
{
    public string Name { get; private set; }
    public ICollection<CharmRank> Ranks { get; private set; } = [];

    private Charm()
    {
        Name = string.Empty;
    }

    private Charm(string name, ICollection<CharmRank> ranks)
    {
        Name = name;
        Ranks = ranks;
    }

    public static Charm Create(string name, ICollection<CharmRank> ranks)
    {
        return new Charm(name, ranks);
    }

    public void Update(string name)
    {
        Name = name;

        UpdateLastModified();
    }
}
