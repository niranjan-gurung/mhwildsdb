using mhwildsdb.Entities.Skills;

namespace mhwildsdb.Entities;

public class Armour : EntityBase
{
    public string Name { get; private set; }
    public string Piece { get; private set; }
    public string Rank { get; private set; }
    public int Rarity { get; private set; }
    public int Defense { get; private set; }
    public Resistances Resistances { get; private set; }
    public ICollection<int> Slots { get; private set; } = [];
    public ICollection<SkillRank> Skills { get; private set; } = [];

    private Armour()
    {
        Name = string.Empty;
        Piece = string.Empty;
        Rank = string.Empty;
        Resistances = new Resistances(0, 0, 0, 0, 0);
    }

    private Armour(
        string name, 
        string piece, 
        string rank, 
        int rarity,
        int defense,
        Resistances resistances,
        ICollection<int> slots,
        ICollection<SkillRank> skills)
    {
        Name = name;
        Piece = piece;
        Rank = rank;
        Rarity = rarity;
        Defense = defense;
        Resistances = resistances;
        Slots = slots;
        Skills = skills;
    }

    public static Armour Create(
        string name, 
        string piece, 
        string rank, 
        int rarity, 
        int defense, 
        Resistances resistances, 
        ICollection<int> slots, 
        ICollection<SkillRank> skills)
    {
        return new Armour(name, piece, rank, rarity, defense, resistances, slots, skills);
    }

    public void Update(
        string name, 
        string piece, 
        string rank, 
        int rarity, 
        int defense, 
        Resistances resistances)
    {
        Name = name;
        Piece = piece;
        Rank = rank;
        Rarity = rarity;
        Defense = defense;
        Resistances = resistances;

        UpdateLastModified();
    }
}
