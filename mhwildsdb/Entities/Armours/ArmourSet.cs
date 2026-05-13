using mhwildsdb.Entities.Skills;

namespace mhwildsdb.Entities.Armours;

public class ArmourSet : EntityBase
{
    public string Name { get; private set; }
    public ICollection<Armour> Pieces { get; private set; } = [];

    public Guid? SetBonusSkillId { get; private set; }  // FK
    public Skill? SetBonusSkill { get; private set; } = null!;
    public Guid? GroupBonusSkillId { get; private set; } // FK
    public Skill? GroupBonusSkill { get; private set; } = null!;

    private ArmourSet() 
    {
        Name = string.Empty;
    }

    private ArmourSet(string name, ICollection<Armour> pieces, Skill? setBonusSkill, Skill? groupBonusSkill)
    {
        Name = name;
        Pieces = pieces;
        SetBonusSkill = setBonusSkill;
        GroupBonusSkill = groupBonusSkill;
    }

    public static ArmourSet Create(string name, ICollection<Armour> pieces, Skill? setBonusSkill = null, Skill? groupBonusSkill = null)
    {
        return new ArmourSet(name, pieces, setBonusSkill, groupBonusSkill);
    }

    public void Update(string name)
    {
        Name = name;
        UpdateLastModified();
    }
}
